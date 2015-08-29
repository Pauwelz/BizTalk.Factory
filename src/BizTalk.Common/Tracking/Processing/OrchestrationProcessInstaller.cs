﻿#region Copyright & License

// Copyright © 2012 - 2013 François Chabot, Yves Dierick
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
using Microsoft.XLANGs.Core;

namespace Be.Stateless.BizTalk.Tracking.Processing
{
	public abstract class OrchestrationProcessInstaller : ProcessInstaller
	{
		protected OrchestrationProcessInstaller()
		{
			ExcludedTypes = new List<Type>();
		}

		#region Base Class Member Overrides

		protected override string[] ProcessNames
		{
			get
			{
				var orchestrationAssembly = GetType().Assembly;
				var orchestrationTypes = orchestrationAssembly.GetTypes()
					.Where(type => typeof(Service).IsAssignableFrom(type) && !ExcludedTypes.Contains(type));
				return orchestrationTypes.Select(type => type.Namespace).ToArray();
			}
		}

		#endregion

		protected ICollection<Type> ExcludedTypes { get; set; }
	}
}