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

namespace Be.Stateless.BizTalk.Dsl.Binding.Convention.BizTalkFactory
{
	public class ApplicationNamingConvention<TNamingConvention>
		where TNamingConvention : new()
	{
		public ApplicationNamingConvention()
		{
			_convention = new TNamingConvention();
		}

		public TNamingConvention Is<T>(T name)
		{
			((IApplicationNameMemento<T>) _convention).Name = name;
			return _convention;
		}

		private readonly TNamingConvention _convention;
	}
}
