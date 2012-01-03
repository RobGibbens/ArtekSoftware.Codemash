using System;
using System.IO;

namespace ArtekSoftware.Codemash
{
	public class DefaultDatabaseManager : IDefaultDatabaseManager
	{
		IAnalytics _analytics;
		ITestFlightProxy _testFlight;
		
		public DefaultDatabaseManager (IAnalytics analytics, ITestFlightProxy testFlight)
		{
			_testFlight = testFlight;
			_analytics = analytics;
		}
		
		public void CopyDefaultDatabase()
		{
			string dbname = "codemash.db3";
			string documents = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			string db = Path.Combine (documents, dbname);
 
			string rootPath = Path.Combine (Environment.CurrentDirectory, "DefaultDatabase");
			string rootDbPath = Path.Combine (rootPath, dbname);
 
			var runtimeDbExists = File.Exists (db);
			var defaultDatabaseExists = File.Exists (rootDbPath);
			
			if (!runtimeDbExists && defaultDatabaseExists) {
				File.Copy (rootDbPath, db);
				_testFlight.PassCheckpoint ("Copied default database");
				_analytics.Log ("Copied default database");
			} 			
		}
	}
}