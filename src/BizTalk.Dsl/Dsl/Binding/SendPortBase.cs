﻿#region Copyright & License

// Copyright © 2012 - 2017 François Chabot, Yves Dierick
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using Be.Stateless.BizTalk.Dsl.Binding.Convention;
using Be.Stateless.BizTalk.Dsl.Binding.Extensions;
using Be.Stateless.BizTalk.Dsl.Binding.Subscription;
using Be.Stateless.BizTalk.Dsl.Pipeline;
using Be.Stateless.Extensions;

namespace Be.Stateless.BizTalk.Dsl.Binding
{
	// see also SendPort, http://msdn.microsoft.com/en-us/library/aa560374.aspx
	public abstract class SendPortBase<TNamingConvention>
		: ISendPort<TNamingConvention>,
			ISupportEnvironmentOverride,
			ISupportNamingConvention,
			ISupportValidation,
			IVisitable<IApplicationBindingVisitor>
		where TNamingConvention : class
	{
		protected internal SendPortBase()
		{
			Priority = Priority.Normal;
			Transport = new SendPortTransport();
		}

		protected internal SendPortBase(Action<ISendPort<TNamingConvention>> sendPortConfigurator) : this()
		{
			sendPortConfigurator(this);
			((ISupportValidation) this).Validate();
		}

		#region ISendPort<TNamingConvention> Members

		public IApplicationBinding<TNamingConvention> ApplicationBinding { get; internal set; }

		public SendPortTransport BackupTransport
		{
			get { return _backupTransport ?? (_backupTransport = new SendPortTransport()); }
		}

		public string Description { get; set; }

		public Filter Filter { get; set; }

		public bool IsTwoWay
		{
			get { return ReceivePipeline != null; }
		}

		public bool OrderedDelivery { get; set; }

		public Priority Priority { get; set; }

		public TNamingConvention Name { get; set; }

		public ReceivePipeline ReceivePipeline { get; set; }

		public SendPipeline SendPipeline { get; set; }

		public ServiceState State { get; set; }

		public bool StopSendingOnOrderedDeliveryFailure { get; set; }

		public SendPortTransport Transport { get; private set; }

		#endregion

		#region ISupportEnvironmentOverride Members

		void ISupportEnvironmentOverride.ApplyEnvironmentOverrides(string environment)
		{
			if (!environment.IsNullOrEmpty())
			{
				ApplyEnvironmentOverrides(environment);
				// ReSharper disable once SuspiciousTypeConversion.Global
				(Filter as ISupportEnvironmentOverride).IfNotNull(f => f.ApplyEnvironmentOverrides(environment));
				((ISupportEnvironmentOverride) ReceivePipeline).IfNotNull(rp => rp.ApplyEnvironmentOverrides(environment));
				((ISupportEnvironmentOverride) SendPipeline).IfNotNull(sp => sp.ApplyEnvironmentOverrides(environment));
				((ISupportEnvironmentOverride) Transport).ApplyEnvironmentOverrides(environment);
				((ISupportEnvironmentOverride) _backupTransport).IfNotNull(bt => bt.ApplyEnvironmentOverrides(environment));
			}
		}

		#endregion

		#region ISupportNamingConvention Members

		string ISupportNamingConvention.Name
		{
			get { return NamingConventionThunk.ComputeSendPortName(this); }
		}

		#endregion

		#region ISupportValidation Members

		void ISupportValidation.Validate()
		{
			if (Name == null) throw new BindingException("Send Port's Name is not defined.");
			if (SendPipeline == null) throw new BindingException("Send Port's Send Pipeline is not defined.");
			Transport.Validate("Send Port's Primary Transport");
			_backupTransport.IfNotNull(bt => bt.Validate("Send Port's Backup Transport"));
		}

		#endregion

		#region IVisitable<IApplicationBindingVisitor> Members

		public void Accept(IApplicationBindingVisitor visitor)
		{
			visitor.VisitSendPort(this);
		}

		#endregion

		protected virtual void ApplyEnvironmentOverrides(string environment) { }

		private SendPortTransport _backupTransport;
	}
}
