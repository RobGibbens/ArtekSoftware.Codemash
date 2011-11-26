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
	public class RemoteQueueEntity : Entity
	{
		public static string TableName = "RemoteQueue";
		public static string CreateTableSql = @"CREATE TABLE " + TableName + " (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, URI VARCHAR, DateQueuedOn VARCHAR, ConferenceName VARCHAR, UserName VARCHAR)";

		public string URI { get; set; }

		public DateTime DateQueuedOn { get; set; }

		public string ConferenceName { get; set; }

		public string UserName { get; set; }
		
		public string AddOrRemove {get;set;}
	}
}

