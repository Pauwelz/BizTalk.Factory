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
using BizTalkReceivePortCollection = Microsoft.BizTalk.ExplorerOM.ReceivePortCollection;

namespace Be.Stateless.BizTalk.Explorer
{
	public class ReceivePortCollection
	{
		public ReceivePortCollection(BizTalkReceivePortCollection ports)
		{
			if (ports == null) throw new ArgumentNullException("ports");
			BizTalkReceivePortCollection = ports;
		}

		public ReceivePort this[string name]
		{
			get
			{
				var explorerReceivePort = BizTalkReceivePortCollection[name];
				if (explorerReceivePort == null)
					throw new Exception(
						string.Format(
							"BizTalk Receive Port '{0}' cannot be found in BizTalk Server Group [{1}].",
							name,
							BizTalkServerGroup.ManagementDatabase));
				return new ReceivePort(explorerReceivePort);
			}
		}

		private BizTalkReceivePortCollection BizTalkReceivePortCollection { get; set; }
	}
}
