using System;
using System.Linq;
using Catnap;
using System.Collections.Generic;
using Catnap.Common.Logging;
using Catnap.Database;
using Catnap.Find;
using Catnap.Maps;
using Catnap.Adapters;
using Catnap.Maps.Impl;
using Catnap.Migration;
using System.Threading;
using Catnap.Find.Conditions;
using System.IO;
using Mono.Data.Sqlite;
//using MonoTouch.TestFlight;

namespace ArtekSoftware.Codemash
{
	public class SessionsCacheRepository
	{
		public void Cache (IList<Session> dtos)
		{
			var documents = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			string db = Path.Combine (documents, "codemash.db3");
			var dataSource = "Data Source=" + db;
			var conn = new SqliteConnection (dataSource);
			
			
			int count = 0;
			using (var cmd = conn.CreateCommand()) {
				conn.Open ();
				cmd.CommandText = "select count(*) from " + SessionEntity.TableName;
				var x = cmd.ExecuteScalar();
				if (x != null)
				{
					count = Convert.ToInt32(x);
				}
			}
			
			if (count > 0) {

				using (var cmd = conn.CreateCommand()) {
					//conn.Open ();
					cmd.CommandText = "delete from " + SessionEntity.TableName;
					cmd.ExecuteNonQuery ();
				}
			}
			
			using (var command = conn.CreateCommand()) {
				foreach (var dto in dtos) {
					command.CommandText = @"insert into " + SessionEntity.TableName + 
												" (URI, Title, Abstract, Start, End, Room, Difficulty, SpeakerName, Technology, SpeakerURI) values (" + 
														SafeString(dto.URI) + "," +
														SafeString(dto.Title) + "," +
														SafeString(dto.Abstract) + "," + 
														SafeDate(dto.Start) + "," + 
														SafeDate(dto.End) + "," + 
														SafeString(dto.Room) + "," + 
														SafeString(dto.Difficulty) + "," + 
														SafeString(dto.SpeakerName) + "," + 
														SafeString(dto.Technology) + "," + 
														SafeString(dto.SpeakerURI) +
												")";
					command.ExecuteNonQuery ();
				}
			}
			
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
	
	public class LocalSessionsRepository : Repository<SessionEntity>
	{
		public int Count ()
		{
			var command = new DbCommandSpec ()
    			.SetCommandText ("select count(*) from Sessions");
			
			object possibleCount = UnitOfWork.Current.Session.ExecuteScalar (command);
			
			int count = 0;
			int.TryParse (possibleCount.ToString (), out count);
			
			return count;
		}
		
//		public void Cache (IList<Session> dtos)
//		{
//			//TestFlight.PassCheckpoint ("LocalSessionsRepository.Cache - 1");
//			//Thread.Sleep(1000);
//			
//			int count = this.Count ();
//			
//			if (count > 0) {
////TestFlight.PassCheckpoint ("LocalSessionsRepository.Cache - 2");
//			//Thread.Sleep(1000);
//							
//				var allCurrentEntities = this.Find ();
//			
//				foreach (var entity in allCurrentEntities) {
//					this.Delete (entity.Id);
//				}
//				
//				//TestFlight.PassCheckpoint ("LocalSessionsRepository.Cache - 3");
//			//Thread.Sleep(1000);
//			
//			}
//			//TestFlight.PassCheckpoint ("LocalSessionsRepository.Cache - 4");
//			//Thread.Sleep(1000);
//			
//			foreach (var dto in dtos) {
//				var entity = new SessionEntity ()
//				{
//					Abstract = dto.Abstract,
//					Difficulty = dto.Difficulty,
//					Room = dto.Room,
//					SpeakerName = dto.SpeakerName,
//					SpeakerURI = dto.SpeakerURI,
//					Start = dto.Start,
//					Technology = dto.Technology,
//					Title = dto.Title,
//					URI = dto.URI,
//				};
//				
//				//Console.WriteLine ("Saving " + entity.Title);
//				try
//				{
//					this.Save (entity);
//				}
//				catch
//				{
//				}
//				//Thread.Sleep (10);
//			}
//			
//			//TestFlight.PassCheckpoint ("LocalSessionsRepository.Cache - 5");
//			//Thread.Sleep(1000);
//			
//		}
		
		public List<SessionEntity> GetForSpeaker (string speakerUri)
		{
			var criteria = new Criteria (
				Condition.Or (
						Condition.Equal<SessionEntity> (x => x.SpeakerURI, speakerUri),
						Condition.Equal<SessionEntity> (x => x.SpeakerURI, "/" + speakerUri)
					)
				);
			
			var sessions = this.Find (criteria);
			
			return sessions.ToList ();
		}
	}
}

