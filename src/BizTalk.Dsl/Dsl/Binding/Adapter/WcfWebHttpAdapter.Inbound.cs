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
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Configuration;
using Be.Stateless.BizTalk.Dsl.Binding.Adapter.Extensions;
using Microsoft.BizTalk.Adapter.Wcf.Config;
using Microsoft.BizTalk.Component.Interop;

namespace Be.Stateless.BizTalk.Dsl.Binding.Adapter
{
	public abstract partial class WcfWebHttpAdapter
	{
		#region Nested Type: Inbound

		/// <summary>
		/// Microsoft BizTalk Server uses the WCF-WebHttp adapter to send messages to RESTful services.
		/// </summary>
		/// <remarks>
		/// The WCF-WebHttp send adapter sends HTTP messages to a service from a BizTalk message. The receive location
		/// receives messages from a RESTful service. For GET and DELETE request, the adapter does not use any payload.
		/// For POST and PUT request, the adapter uses the BizTalk message body part to the HTTP content/payload.
		/// </remarks>
		/// <seealso href="https://msdn.microsoft.com/en-us/library/jj572846.aspx">WCF-WebHttp Adapter</seealso>
		/// <seealso href="https://msdn.microsoft.com/en-us/library/jj572859.aspx">How to Configure a WCF-WebHttp Receive Location</seealso>
		public class Inbound
			: WcfWebHttpAdapter<Uri, WebHttpRLConfig>,
				IInboundAdapter,
				IAdapterConfigInboundDisableLocationOnFailure,
				IAdapterConfigInboundIncludeExceptionDetailInFaults,
				IAdapterConfigInboundSuspendRequestMessageOnFailure,
				IAdapterConfigMaxConcurrentCalls,
				IAdapterConfigServiceBehavior,
				IAdapterConfigSSO
		{
			public Inbound()
			{
				// Binding Tab - Service Throttling Behavior Settings
				MaxConcurrentCalls = 200;

				// Behavior Tab
				ServiceBehaviors = Enumerable.Empty<BehaviorExtensionElement>();

				// Messages Tab - Error Handling Settings
				DisableLocationOnFailure = false;
				SuspendRequestMessageOnFailure = true;
				IncludeExceptionDetailInFaults = true;
			}

			public Inbound(Action<Inbound> adapterConfigurator) : this()
			{
				adapterConfigurator(this);
			}

			#region IAdapterConfigInboundDisableLocationOnFailure Members

			public bool DisableLocationOnFailure
			{
				get { return _adapterConfig.DisableLocationOnFailure; }
				set { _adapterConfig.DisableLocationOnFailure = value; }
			}

			#endregion

			#region IAdapterConfigInboundIncludeExceptionDetailInFaults Members

			public bool IncludeExceptionDetailInFaults
			{
				get { return _adapterConfig.IncludeExceptionDetailInFaults; }
				set { _adapterConfig.IncludeExceptionDetailInFaults = value; }
			}

			#endregion

			#region IAdapterConfigInboundSuspendRequestMessageOnFailure Members

			public bool SuspendRequestMessageOnFailure
			{
				get { return _adapterConfig.SuspendMessageOnFailure; }
				set { _adapterConfig.SuspendMessageOnFailure = value; }
			}

			#endregion

			#region IAdapterConfigMaxConcurrentCalls Members

			public int MaxConcurrentCalls
			{
				get { return _adapterConfig.MaxConcurrentCalls; }
				set { _adapterConfig.MaxConcurrentCalls = value; }
			}

			#endregion

			#region IAdapterConfigServiceBehavior Members

			public IEnumerable<BehaviorExtensionElement> ServiceBehaviors { get; set; }

			#endregion

			#region IAdapterConfigSSO Members

			/// <summary>
			/// Specify whether to use Enterprise Single Sign-On (SSO) to retrieve client credentials to issue an SSO
			/// ticket.
			/// </summary>
			/// <remarks>
			/// It defaults to <c>False</c>.
			/// </remarks>
			public bool UseSSO
			{
				get { return _adapterConfig.UseSSO; }
				set { _adapterConfig.UseSSO = value; }
			}

			#endregion

			#region Base Class Member Overrides

			protected override void Save(IPropertyBag propertyBag)
			{
				_adapterConfig.ServiceBehaviorConfiguration = ServiceBehaviors.GetServiceBehaviorElementXml();
				base.Save(propertyBag);
			}

			#endregion

			#region Message Tab - Inbound Message Settings

			/// <summary>
			/// Specify whether to add a empty payload message for some incoming HTTP request made for some HTTP verbs.
			/// </summary>
			/// <remarks>
			/// <para>
			/// List of coma separated HTTP Verbs.
			/// </para>
			/// <para>
			/// Message body being added: <code><![CDATA[
			/// <WCF-WebHttpMessageBody xmlns="http://schemas.microsoft.com/BizTalk/2014/01/Adapters/WCF-WebHttp.EmptyMessage">
			/// ]]></code>
			/// </para>
			/// </remarks>
			public string AddMessageBodyForHttpVerbs
			{
				get { return _adapterConfig.AddMessageBodyForHttpVerbs; }
				set { _adapterConfig.AddMessageBodyForHttpVerbs = value; }
			}

			#endregion
		}

		#endregion
	}
}
