﻿#region Copyright & License

// Copyright © 2012 - 2016 François Chabot, Yves Dierick
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

namespace Be.Stateless.BizTalk.Dsl.Binding.Visitor
{
	/// <summary>
	/// Base <see cref="IApplicationBindingVisitor"/> implementation that ensures that environment overrides are applied
	/// before visit.
	/// </summary>
	public abstract class ApplicationBindingVisitorBase : IApplicationBindingVisitor
	{
		protected ApplicationBindingVisitorBase(string targetEnvironment)
		{
			Environment = targetEnvironment;
		}

		#region IApplicationBindingVisitor Members

		public void VisitApplicationBinding<TNamingConvention>(IApplicationBinding<TNamingConvention> applicationBinding) where TNamingConvention : class
		{
			((ISupportEnvironmentOverride) applicationBinding).ApplyEnvironmentOverrides(Environment);
			((ISupportValidation) applicationBinding).Validate();
			VisitApplicationCore(applicationBinding);
		}

		public void VisitOrchestration(IOrchestrationBinding orchestrationBinding)
		{
			((ISupportEnvironmentOverride) orchestrationBinding).ApplyEnvironmentOverrides(Environment);
			((ISupportValidation) orchestrationBinding).Validate();
			VisitOrchestrationCore(orchestrationBinding);
		}

		public void VisitReceivePort<TNamingConvention>(IReceivePort<TNamingConvention> receivePort) where TNamingConvention : class
		{
			((ISupportEnvironmentOverride) receivePort).ApplyEnvironmentOverrides(Environment);
			((ISupportValidation) receivePort).Validate();
			VisitReceivePortCore(receivePort);
		}

		public void VisitReceiveLocation<TNamingConvention>(IReceiveLocation<TNamingConvention> receiveLocation) where TNamingConvention : class
		{
			((ISupportEnvironmentOverride) receiveLocation).ApplyEnvironmentOverrides(Environment);
			((ISupportValidation) receiveLocation).Validate();
			VisitReceiveLocationCore(receiveLocation);
		}

		public void VisitSendPort<TNamingConvention>(ISendPort<TNamingConvention> sendPort) where TNamingConvention : class
		{
			((ISupportEnvironmentOverride) sendPort).ApplyEnvironmentOverrides(Environment);
			((ISupportValidation) sendPort).Validate();
			VisitSendPortCore(sendPort);
		}

		#endregion

		protected string Environment { get; private set; }

		protected internal abstract void VisitApplicationCore<TNamingConvention>(IApplicationBinding<TNamingConvention> applicationBinding) where TNamingConvention : class;

		protected internal abstract void VisitOrchestrationCore(IOrchestrationBinding orchestrationBinding);

		protected internal abstract void VisitReceivePortCore<TNamingConvention>(IReceivePort<TNamingConvention> receivePort) where TNamingConvention : class;

		protected internal abstract void VisitReceiveLocationCore<TNamingConvention>(IReceiveLocation<TNamingConvention> receiveLocation) where TNamingConvention : class;

		protected internal abstract void VisitSendPortCore<TNamingConvention>(ISendPort<TNamingConvention> sendPort) where TNamingConvention : class;
	}
}
