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
	public static class DatabaseMigrator
	{
		private static readonly DatabaseMigratorUtility migratorUtility = new DatabaseMigratorUtility (new SqliteAdapter (typeof(Mono.Data.Sqlite.SqliteConnection)));

		public static void Execute ()
		{
			var schema = new CreateSchema ();
			migratorUtility.Migrate (schema);
		}
	}
}