using System;
using System.Collections.Generic;
using System.IO;
//using Catnap;
//using Catnap.Database;
//using Catnap.Find;
//using Catnap.Find.Conditions;
using Mono.Data.Sqlite;
using System.Linq;
using ArtekSoftware.Conference;
//
//namespace ArtekSoftware.Codemash
//{
	public class SessionsCacheRepository
	{
		public void Cache (IList<ArtekSoftware.Conference.Session> dtos)
		{
//			var documents = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
//			string db = Path.Combine (documents, "codemash.db3");
//			var dataSource = "Data Source=" + db;
//			var conn = new SqliteConnection (dataSource);
//			
//			
//			int count = 0;
//			using (var cmd = conn.CreateCommand()) {
//				conn.Open ();
//				cmd.CommandText = "select count(*) from " + SessionEntity.TableName;
//				var x = cmd.ExecuteScalar();
//				if (x != null)
//				{
//					count = Convert.ToInt32(x);
//				}
//			}
//			
//			if (count > 0) {
//
//				using (var cmd = conn.CreateCommand()) {
//					//conn.Open ();
//					cmd.CommandText = "delete from " + SessionEntity.TableName;
//					cmd.ExecuteNonQuery ();
//				}
//			}
//			
//			using (var command = conn.CreateCommand()) {
//				foreach (var dto in dtos) {
//					command.CommandText = @"insert into " + SessionEntity.TableName + 
//												" (URI, Title, Abstract, Start, Room, Difficulty, SpeakerName, Technology, SpeakerURI) values (" + 
//														SafeString(dto.URI) + "," +
//														SafeString(dto.Title) + "," +
//														SafeString(dto.Abstract) + "," + 
//														SafeDate(dto.Start) + "," + 
//														SafeString(dto.Room) + "," + 
//														SafeString(dto.Difficulty) + "," + 
//														SafeString(dto.SpeakerName) + "," + 
//														SafeString(dto.Technology) + "," + 
//														SafeString(dto.SpeakerURI) +
//												")";
//					command.ExecuteNonQuery ();
//					
//					command.CommandText = @"UPDATE " + ScheduledSessionEntity.TableName + " SET " +
//														"Title = " + SafeString(dto.Title) + "," +
//														"Abstract = " + SafeString(dto.Abstract) + "," + 
//														"Start = " + SafeDate(dto.Start) + "," + 
//														"Room = " + SafeString(dto.Room) + "," + 
//														"Difficulty = " + SafeString(dto.Difficulty) + "," + 
//														"SpeakerName = " + SafeString(dto.SpeakerName) + "," + 
//														"Technology = " + SafeString(dto.Technology) + "," + 
//														"SpeakerURI = " + SafeString(dto.SpeakerURI) + " " +
//														"WHERE URI = " + SafeString (dto.URI);
//					command.ExecuteNonQuery ();					
//				}
//			}
			
		}
		
		private string SafeDate(DateTime dtoValue)
		{
			string returnValue = null;
			
			if (dtoValue > DateTime.MinValue)
			{
				returnValue = " '" + dtoValue.ToString("yyyy-MM-dd H:mm") + "'";
			}
			else
			{
				returnValue = "null";
			}
			
			return returnValue;			
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
////	
////	public class LocalSessionsRepository : Repository<SessionEntity>
////	{
////		public int Count ()
////		{
////			var command = new DbCommandSpec ()
////    			.SetCommandText ("select count(*) from Sessions");
////			
////			object possibleCount = UnitOfWork.Current.Session.ExecuteScalar (command);
////			
////			int count = 0;
////			int.TryParse (possibleCount.ToString (), out count);
////			
////			return count;
////		}
////		
////		public List<SessionEntity> GetForSpeaker (string speakerUri)
////		{
////			var criteria = new Criteria (
////				Condition.Or (
////						Condition.Equal<SessionEntity> (x => x.SpeakerURI, speakerUri),
////						Condition.Equal<SessionEntity> (x => x.SpeakerURI, "/" + speakerUri)
////					)
////				);
////			
////			var sessions = this.Find (criteria);
////			
////			return sessions.ToList ();
////		}
////	}
//}