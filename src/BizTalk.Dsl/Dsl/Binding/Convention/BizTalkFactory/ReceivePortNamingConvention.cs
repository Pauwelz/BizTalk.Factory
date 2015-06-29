#region Copyright & License

// Copyright � 2012 - 2015 Fran�ois Chabot, Yves Dierick
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
	public class ReceivePortNamingConvention<TNamingConvention> : IReceivePortNamingConvention<TNamingConvention>
		where TNamingConvention : new()
	{
		public ReceivePortNamingConvention()
		{
			_convention = new TNamingConvention();
		}

		#region IReceivePortNamingConvention<TNamingConvention> Members

		public TNamingConvention Offwards<T>(T party)
		{
			((IPartyMemento<T>) _convention).Party = party;
			return _convention;
		}

		#endregion

		private readonly TNamingConvention _convention;
	}
}
