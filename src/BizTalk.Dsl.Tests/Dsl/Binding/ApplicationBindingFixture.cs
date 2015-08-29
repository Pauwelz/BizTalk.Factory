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

using System.Diagnostics;
using Be.Stateless.BizTalk.Dsl.Binding.Diagnostics;
using NUnit.Framework;

namespace Be.Stateless.BizTalk.Dsl.Binding
{
	[TestFixture]
	public class ApplicationBindingFixture
	{
		[Test]
		public void NameOfNonUserDerivedTypeIsNotUsedInErrorMessage()
		{
			var ab = new ApplicationBinding();

			var sfi = (IProvideSourceFileInformation) ab;
			Assert.That(sfi.Name, Is.EqualTo(new StackFrame(0, true).GetFileName()));
			Assert.That(sfi.Line, Is.GreaterThan(0));
			Assert.That(
				() => ((ISupportValidation) ab).Validate(),
				Throws.InstanceOf<BindingException>().With.Message.EqualTo(
					string.Format(
						"Application's Name is not defined.\r\n{0}, line {1}, column {2}.",
						sfi.Name,
						sfi.Line,
						sfi.Column)));
		}
	}
}