﻿#region Copyright & License

// Copyright © 2012 - 2018 François Chabot
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

using Be.Stateless.BizTalk.Component;
using Be.Stateless.BizTalk.Message.Extensions;
using Be.Stateless.BizTalk.Streaming;
using Be.Stateless.Logging;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;

namespace Be.Stateless.BizTalk.MicroComponent
{
	/// <summary>
	/// Wraps the original message stream by a <see cref="MultipartFormDataContentStream"/>.
	/// </summary>
	public class MultipartFormDataContentEncoder : IMicroPipelineComponent
	{
		#region IMicroPipelineComponent Members

		public IBaseMessage Execute(IPipelineContext pipelineContext, IBaseMessage message)
		{
			message.BodyPart.WrapOriginalDataStream(
				originalStream => {
					if (_logger.IsDebugEnabled) _logger.Debug("Wrapping message stream in a MultipartFormDataContentStream.");
					var multipartFormDataContentStream = new MultipartFormDataContentStream(originalStream);
					message.BodyPart.ContentType = multipartFormDataContentStream.ContentType;
					return multipartFormDataContentStream;
				},
				pipelineContext.ResourceTracker);
			return message;
		}

		#endregion

		private static readonly ILog _logger = LogManager.GetLogger(typeof(MultipartFormDataContentEncoder));
	}
}
