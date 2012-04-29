using System;
using System.IO;
using Catnap.Adapters;
using ArtekSoftware.Conference;

namespace ArtekSoftware.Codemash
{
	public class CatnapBootstrapper
	{
		ITestFlightProxy _testFlight;
		
		public CatnapBootstrapper (ITestFlightProxy testFlight)
		{
			_testFlight = testFlight;
		}
		
		public void Initialize ()
		{
			var documents = Environment.GetFolderPath (Environment.SpecialFolder.Personal);

			string db = Path.Combine (documents, "codemash.db3");
			_testFlight.PassCheckpoint ("Initialized CatNap");
			
			Catnap.SessionFactory.Initialize ("Data Source=" + db, new SqliteAdapter (typeof(Mono.Data.Sqlite.SqliteConnection)));
		}
	}
}

