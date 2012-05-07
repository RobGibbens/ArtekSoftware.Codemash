//using System;
//using Catnap;
//
//namespace ArtekSoftware.Codemash
//{
//	public class RefreshEntity: Entity
//	{
//		public static string TableName = "Refresh";
//		public static string CreateTableSql = @"CREATE TABLE " + TableName + " (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, LastRefreshTime VARCHAR, EntityName VARCHAR)";
//		
//		public DateTime LastRefreshTime { get; set; }
//		
//		public string EntityName { get; set; }
//	}
//}