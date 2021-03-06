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

using Microsoft.BizTalk.Adapter.Wcf.Config;
using Moq;
using NUnit.Framework;

namespace Be.Stateless.BizTalk.Dsl.Binding.Adapter
{
	[TestFixture]
	public class WcfSapAdapterFixture
	{
		[Test]
		public void ProtocolTypeSettingsAreReadFromRegistry()
		{
			var mock = new Mock<WcfSapAdapter<CustomRLConfig>> { CallBase = true };
			var wsa = mock.Object as IAdapter;
			Assert.That(wsa.ProtocolType.Name, Is.EqualTo("WCF-SAP"));
			Assert.That(wsa.ProtocolType.Capabilities, Is.EqualTo(907));
			Assert.That(wsa.ProtocolType.ConfigurationClsid, Is.EqualTo("a5f15999-8879-472d-8c62-3b5ea9406504"));
		}
	}
}
