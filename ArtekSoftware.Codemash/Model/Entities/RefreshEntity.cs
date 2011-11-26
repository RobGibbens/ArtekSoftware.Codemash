using System;
using Catnap.Common.Logging;
using Catnap.Database;
using Catnap.Find;
using Catnap.Maps;
using Catnap.Adapters;
using Catnap.Maps.Impl;
using Catnap;
using Catnap.Migration;
using System.Collections.Generic;
using System.Linq;

namespace ArtekSoftware.Codemash
{
	public class RefreshEntity: Entity
	{
		public static string TableName = "Refresh";
		public static string CreateTableSql = @"CREATE TABLE " + TableName + " (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, LastRefreshTime VARCHAR, EntityName VARCHAR)";
		
		public DateTime LastRefreshTime { get; set; }
		
		public string EntityName { get; set; }
	}
}

