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
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Be.Stateless.Extensions;

namespace Be.Stateless.BizTalk.Explorer
{
	public class BizTalkServerManagementDatabase
	{
		public BizTalkServerManagementDatabase(string server, string database)
		{
			if (server.IsNullOrEmpty()) throw new ArgumentNullException("server");
			if (database.IsNullOrEmpty()) throw new ArgumentNullException("database");
			Server = server;
			Database = database;
		}

		#region Base Class Member Overrides

		public override string ToString()
		{
			return string.Format("{0}:{1}", Server, Database);
		}

		#endregion

		[SuppressMessage("ReSharper", "CollectionNeverQueried.Local", Justification = "SqlConnectionStringBuilder")]
		public string ConnectionString
		{
			get
			{
				var builder = new SqlConnectionStringBuilder {
					ApplicationName = "ExplorerOM/" + Process.GetCurrentProcess().ProcessName,
					DataSource = Server,
					InitialCatalog = Database,
					IntegratedSecurity = true
				};
				return builder.ConnectionString;
			}
		}

		public string Database { get; private set; }

		public string Server { get; private set; }
	}
}
