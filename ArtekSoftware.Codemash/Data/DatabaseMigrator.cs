using Catnap.Adapters;
using Catnap.Migration;

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