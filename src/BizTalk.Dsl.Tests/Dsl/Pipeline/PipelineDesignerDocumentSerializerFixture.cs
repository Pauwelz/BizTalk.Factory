﻿#region Copyright & License

// Copyright © 2012 - 2015 François Chabot, Yves Dierick
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
using Be.Stateless.BizTalk.Component;
using Be.Stateless.BizTalk.RuleEngine;
using Be.Stateless.BizTalk.Tracking;
using Be.Stateless.BizTalk.Unit.Resources;
using Microsoft.BizTalk.Component;
using NUnit.Framework;

namespace Be.Stateless.BizTalk.Dsl.Pipeline
{
	[TestFixture]
	public class PipelineDesignerDocumentSerializerFixture
	{
		[Test]
		public void ToXml()
		{
			var pipelineDocument = ((IPipelineSerializerFactory) new TrackingXmlReceive()).GetPipelineDesignerDocumentSerializer();
			// see http://msdn.microsoft.com/en-us/library/system.xml.linq.xnode.deepequals.aspx as an alternative
			// comparison method, but not so helpful when there are differences
			Assert.That(
				pipelineDocument.Serialize(),
				Is.EqualTo(ResourceManager.LoadString("Data.TrackingXmlReceivePipelineDocument.xml")));
		}

		private class TrackingXmlReceive : ReceivePipeline
		{
			public TrackingXmlReceive()
			{
				Description = "Passthru receive pipeline with tracking.";
				Version = new Version(1, 0);
				Stages.Decode
					.AddComponent(
						new FailedMessageRoutingEnablerComponent {
							SuppressRoutingFailureReport = false
						})
					.AddComponent(
						new ActivityTrackerComponent {
							TrackingResolutionPolicy = new PolicyName("ResolutionPolicyName", 2, 2),
							TrackingModes = ActivityTrackingModes.Context
						});
				Stages.Disassemble
					.AddComponent(new XmlDasmComp());
			}
		}
	}
}