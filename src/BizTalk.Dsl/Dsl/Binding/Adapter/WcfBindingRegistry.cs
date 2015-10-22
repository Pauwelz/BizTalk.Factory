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
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel.Configuration;

namespace Be.Stateless.BizTalk.Dsl.Binding.Adapter
{
	public class WcfBindingRegistry : Dictionary<Type, string>
	{
		static WcfBindingRegistry()
		{
			_instance = new WcfBindingRegistry();

			var machineConfiguration = ConfigurationManager.OpenMachineConfiguration();
			var modelSectionGroup = ServiceModelSectionGroup.GetSectionGroup(machineConfiguration);
			// ReSharper disable once PossibleNullReferenceException
			foreach (var binding in modelSectionGroup.Bindings.BindingCollections)
			{
				var baseType = binding.GetType().BaseType;
				if (baseType != null && baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(StandardBindingCollectionElement<,>))
				{
					_instance.Add(baseType.GenericTypeArguments[1], binding.BindingName);
				}
			}
		}

		public static string GetBindingName<TBinding>() where TBinding : StandardBindingElement
		{
			return _instance[typeof(TBinding)];
		}

		private static readonly WcfBindingRegistry _instance;
	}
}
