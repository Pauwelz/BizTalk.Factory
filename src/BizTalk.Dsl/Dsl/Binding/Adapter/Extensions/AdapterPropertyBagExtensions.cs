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

using Microsoft.BizTalk.Component.Interop;

namespace Be.Stateless.BizTalk.Dsl.Binding.Adapter.Extensions
{
	internal static class AdapterPropertyBagExtensions
	{
		internal static void WriteAdapterCustomProperty<T>(this IPropertyBag propertyBag, string name, T value) where T : struct
		{
			propertyBag.WriteAdapterProperty(name, value);
		}

		internal static void WriteAdapterCustomProperty(this IPropertyBag propertyBag, string name, string value)
		{
			propertyBag.WriteAdapterProperty(name, value);
		}

		private static void WriteAdapterProperty(this IPropertyBag propertyBag, string name, object value)
		{
			propertyBag.Write(name, value);
		}
	}
}
