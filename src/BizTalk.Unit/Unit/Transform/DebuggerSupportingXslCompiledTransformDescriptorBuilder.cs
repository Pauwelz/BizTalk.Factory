#region Copyright & License

// Copyright � 2012 - 2017 Fran�ois Chabot, Yves Dierick
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
using System.Xml.Xsl;
using Be.Stateless.BizTalk.Xml.Xsl;
using Be.Stateless.Extensions;

namespace Be.Stateless.BizTalk.Unit.Transform
{
	///// <summary>
	///// <see cref="XslCompiledTransformDescriptorBuilder"/>-derived descriptor that loads the XSLT source file instead of the
	///// <see cref="TransformBase"/>'s embedded XSLT in order to enable XSLT debugging.
	///// </summary>
	internal class DebuggerSupportingXslCompiledTransformDescriptorBuilder : XslCompiledTransformDescriptorBuilder
	{
		public DebuggerSupportingXslCompiledTransformDescriptorBuilder(Type transform, string sourceXsltFilePath) : base(transform)
		{
			if (sourceXsltFilePath.IsNullOrEmpty()) throw new ArgumentNullException("sourceXsltFilePath");
			_sourceXsltFilePath = sourceXsltFilePath;
		}

		#region Base Class Member Overrides

		public override XslCompiledTransform BuildXslCompiledTransform()
		{
			var xslCompiledTransform = new XslCompiledTransform(true);
			xslCompiledTransform.Load(_sourceXsltFilePath, XsltSettings.TrustedXslt, new DebuggerSupportingEmbeddedXmlResolver(_transform));
			return xslCompiledTransform;
		}

		#endregion

		private readonly string _sourceXsltFilePath;
	}
}
