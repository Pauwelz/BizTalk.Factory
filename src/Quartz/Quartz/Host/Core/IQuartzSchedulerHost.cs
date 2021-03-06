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

namespace Be.Stateless.Quartz.Host.Core
{
	/// <summary>
	/// Service interface for core Quartz.NET server.
	/// </summary>
	public interface IQuartzSchedulerHost : IDisposable
	{
		/// <summary>
		/// Initializes the instance of <see cref="IQuartzSchedulerHost"/>.
		/// </summary>
		/// <remarks>
		/// Initialization will only be called once in the host's lifetime.
		/// </remarks>
		void Initialize();

		/// <summary>
		/// Starts this host instance.
		/// </summary>
		void Start();

		/// <summary>
		/// Stops this host instance.
		/// </summary>
		void Stop();
	}
}
