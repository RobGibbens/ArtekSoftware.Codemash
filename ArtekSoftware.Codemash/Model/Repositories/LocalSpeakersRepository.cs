using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Catnap;
using Catnap.Database;
using Catnap.Find;
using Catnap.Find.Conditions;
using Mono.Data.Sqlite;

namespace ArtekSoftware.Codemash
{
	public class SpeakersCacheRepository
	{
		public void Cache (IList<Speaker> dtos)
		{
			var documents = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			string db = Path.Combine (documents, "codemash.db3");
			var dataSource = "Data Source=" + db;
			var conn = new SqliteConnection (dataSource);
			
			
			int count = 0;
			using (var cmd = conn.CreateCommand()) {
				conn.Open ();
				cmd.CommandText = "select count(*) from " + SpeakerEntity.TableName;
				var x = cmd.ExecuteScalar();
				if (x != null)
				{
					count = Convert.ToInt32(x);
				}
			}
			
			if (count > 0) {

				using (var cmd = conn.CreateCommand()) {
					//conn.Open ();
					cmd.CommandText = "delete from " + SpeakerEntity.TableName;
					cmd.ExecuteNonQuery ();
				}
			}
			
			using (var command = conn.CreateCommand()) {
				foreach (var dto in dtos) {
					command.CommandText = @"insert into " + SpeakerEntity.TableName + 
												" (SpeakerURI, Name, Biography, TwitterHandle, BlogURL) values (" + 
														SafeString(dto.SpeakerURI) + "," +
														SafeString(dto.Name) + "," +
														SafeString(dto.Biography) + "," + 
														SafeString(dto.TwitterHandle) + "," + 
														SafeString(dto.BlogURL) +
												")";
					command.ExecuteNonQuery ();
				}
			}
			
		}
		
		private string SafeString(string dtoValue)
		{
			string returnValue = null;
			
			if (!string.IsNullOrWhiteSpace(dtoValue))
			{
				returnValue = " '" + dtoValue.Replace ("'", "''") + "'";
			}
			else
			{
				returnValue = "null";
			}
			
			return returnValue;
		}
			
	}
	
	public class LocalSpeakersRepository : Repository<SpeakerEntity>
	{
		public int Count ()
		{
			var command = new DbCommandSpec ()
    			.SetCommandText ("select count(*) from Speakers");
			
			object possibleCount = UnitOfWork.Current.Session.ExecuteScalar (command);
			
			int count = 0;
			int.TryParse (possibleCount.ToString (), out count);
			
			return count;
		}
		
		public SpeakerEntity FindByName (string name)
		{
			var criteria = new Criteria ();
			criteria.Add (Condition.Equal<SpeakerEntity> (x => x.Name, name));
			var speaker = this.Find (criteria).SingleOrDefault ();
			
			return speaker;
		}
		
//		public void Cache (IList<Speaker> dtos)
//		{
//			var documents = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
//			string db = Path.Combine (documents, "codemash.db3");
//			var dataSource = "Data Source=" + db;
//			var conn = new SqliteConnection (dataSource);
//
//			
//			int count = this.Count ();
//			
//			if (count > 0) {
//
//				using (var cmd = conn.CreateCommand()) {
//					conn.Open ();
//					cmd.CommandText = "delete from " + SpeakerEntity.TableName;
//					cmd.ExecuteNonQuery ();
//				}
//				
////				var allCurrentEntities = this.Find ();
////			
////				foreach (var entity in allCurrentEntities) {
////					this.Delete (entity.Id);
////				}	
//			}
//			
//			using (var command = conn.CreateCommand()) {
//				//(Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, SpeakerURI VARCHAR, Name VARCHAR, Biography VARCHAR, TwitterHandle VARCHAR, BlogURL VARCHAR)";
//				foreach (var dto in dtos) {
//				
//					//conn.Open ();
//					command.CommandText = "insert into " + SpeakerEntity.TableName + " (SpeakerURI, Name, Biography, TwitterHandle, BlogURL) values ('" + dto.SpeakerURI.Replace ("'", "''") + "','" + dto.Name.Replace ("'", "''") + "','" + dto.Biography.Replace ("'", "''") + "','" + dto.TwitterHandle.Replace ("'", "''") + "','" + dto.BlogURL.Replace ("'", "''") + "')";
//					command.ExecuteNonQuery ();
//				
//				
//				
////				var entity = new SpeakerEntity ()
////				{
////					Biography = dto.Biography,
////					BlogURL = dto.BlogURL,
////					Name = dto.Name,
////				//Sessions = dto.Sessions,
////					SpeakerURI = dto.SpeakerURI,
////					TwitterHandle = dto.TwitterHandle,
////				};
////				
////				this.Save (entity);
//				}
//			}
//			
//		}
		
	}
}

