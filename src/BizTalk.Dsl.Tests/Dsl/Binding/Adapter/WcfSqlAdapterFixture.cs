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
	public class WcfSqlAdapterFixture
	{
		[Test]
		public void ProtocolTypeSettingsAreReadFromRegistry()
		{
			var mock = new Mock<WcfSqlAdapter<CustomRLConfig>> { CallBase = true };
			var wsa = mock.Object as IAdapter;
			Assert.That(wsa.ProtocolType.Name, Is.EqualTo("WCF-SQL"));
			Assert.That(wsa.ProtocolType.Capabilities, Is.EqualTo(779));
			Assert.That(wsa.ProtocolType.ConfigurationClsid, Is.EqualTo("59b35d03-6a06-4734-a249-ef561254ecf7"));
		}
	}
}
