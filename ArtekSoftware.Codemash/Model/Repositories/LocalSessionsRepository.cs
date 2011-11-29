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
using MonoTouch.TestFlight;

namespace ArtekSoftware.Codemash
{
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
		
		public void Cache (IList<Session> dtos)
		{
			TestFlight.PassCheckpoint ("LocalSessionsRepository.Cache - 1");
			Thread.Sleep(1000);
			
			int count = this.Count ();
			
			if (count > 0) {
TestFlight.PassCheckpoint ("LocalSessionsRepository.Cache - 2");
			Thread.Sleep(1000);
							
				var allCurrentEntities = this.Find ();
			
				foreach (var entity in allCurrentEntities) {
					this.Delete (entity.Id);
				}
				
				TestFlight.PassCheckpoint ("LocalSessionsRepository.Cache - 3");
			Thread.Sleep(1000);
			
			}
			TestFlight.PassCheckpoint ("LocalSessionsRepository.Cache - 4");
			Thread.Sleep(1000);
			
			foreach (var dto in dtos) {
				var entity = new SessionEntity ()
				{
					Abstract = dto.Abstract,
					Difficulty = dto.Difficulty,
					Room = dto.Room,
					SpeakerName = dto.SpeakerName,
					SpeakerURI = dto.SpeakerURI,
					Start = dto.Start,
					Technology = dto.Technology,
					Title = dto.Title,
					URI = dto.URI,
				};
				
				//Console.WriteLine ("Saving " + entity.Title);
				this.Save (entity);
				Thread.Sleep (10);
			}
			
			TestFlight.PassCheckpoint ("LocalSessionsRepository.Cache - 5");
			Thread.Sleep(1000);
			
		}
		
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

