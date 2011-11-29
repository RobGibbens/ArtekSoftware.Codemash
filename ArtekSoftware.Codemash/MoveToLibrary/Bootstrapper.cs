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
using System.IO;
using MonoTouch.TestFlight;

namespace ArtekSoftware.Codemash
{
	public class Bootstrapper
	{
		public void Initialize ()
		{
			InitializeCatnap ();
			MapEntities ();
			CreateDatabase ();
		}
		
		private void InitializeCatnap ()
		{
			var documents = Environment.GetFolderPath (Environment.SpecialFolder.Personal);

        	string db = Path.Combine (documents, "codemash.db3");
			//TestFlight.PassCheckpoint ("Initialized CatNap");
			
			Catnap.SessionFactory.Initialize ("Data Source=" + db, new SqliteAdapter (typeof(Mono.Data.Sqlite.SqliteConnection)));
		}
		
		private void MapEntities ()
		{
			Log.Level = LogLevel.Off;
			Domain.Configure
            (
				Map.Entity<SessionEntity> ()
        			.Table (SessionEntity.TableName)
                    .Map (new ValuePropertyMap<SessionEntity, string> (x => x.URI))
                    .Map (new ValuePropertyMap<SessionEntity, string> (x => x.Title))
                    .Map (new ValuePropertyMap<SessionEntity, string> (x => x.Abstract))
                    .Map (new ValuePropertyMap<SessionEntity, DateTime> (x => x.Start))
                    .Map (new ValuePropertyMap<SessionEntity, string> (x => x.Room))
                    .Map (new ValuePropertyMap<SessionEntity, string> (x => x.Difficulty))
                    .Map (new ValuePropertyMap<SessionEntity, string> (x => x.SpeakerName))
                    .Map (new ValuePropertyMap<SessionEntity, string> (x => x.Technology))
                    .Map (new ValuePropertyMap<SessionEntity, string> (x => x.SpeakerURI)),
				
				Map.Entity<SpeakerEntity>()
					.Table(SpeakerEntity.TableName)
					.Map (new ValuePropertyMap<SpeakerEntity, string> (x => x.SpeakerURI))
					.Map (new ValuePropertyMap<SpeakerEntity, string> (x => x.Name))
					.Map (new ValuePropertyMap<SpeakerEntity, string> (x => x.Biography))
					.Map (new ValuePropertyMap<SpeakerEntity, string> (x => x.TwitterHandle))
					.Map (new ValuePropertyMap<SpeakerEntity, string> (x => x.BlogURL)),
				
			   Map.Entity<RefreshEntity>()
					.Table (RefreshEntity.TableName)
					.Map (new ValuePropertyMap<RefreshEntity, DateTime> (x => x.LastRefreshTime))
					.Map (new ValuePropertyMap<RefreshEntity, string> (x => x.EntityName)),
					
				Map.Entity<ScheduledSessionEntity> ()
        			.Table (ScheduledSessionEntity.TableName)
                    .Map (new ValuePropertyMap<ScheduledSessionEntity, string> (x => x.URI))
                    .Map (new ValuePropertyMap<ScheduledSessionEntity, string> (x => x.Title))
                    .Map (new ValuePropertyMap<ScheduledSessionEntity, string> (x => x.Abstract))
                    .Map (new ValuePropertyMap<ScheduledSessionEntity, DateTime> (x => x.Start))
                    .Map (new ValuePropertyMap<ScheduledSessionEntity, string> (x => x.Room))
                    .Map (new ValuePropertyMap<ScheduledSessionEntity, string> (x => x.Difficulty))
                    .Map (new ValuePropertyMap<ScheduledSessionEntity, string> (x => x.SpeakerName))
                    .Map (new ValuePropertyMap<ScheduledSessionEntity, string> (x => x.Technology))
                    .Map (new ValuePropertyMap<ScheduledSessionEntity, string> (x => x.SpeakerURI)),
					
				Map.Entity<RemoteQueueEntity> ()
        			.Table (RemoteQueueEntity.TableName)
                    .Map (new ValuePropertyMap<RemoteQueueEntity, string> (x => x.URI))
                    .Map (new ValuePropertyMap<RemoteQueueEntity, string> (x => x.ConferenceName))
                    .Map (new ValuePropertyMap<RemoteQueueEntity, string> (x => x.UserName))
                    .Map (new ValuePropertyMap<RemoteQueueEntity, DateTime> (x => x.DateQueuedOn))
                    .Map (new ValuePropertyMap<RemoteQueueEntity, string> (x => x.AddOrRemove))
					
					
            );
		}
		
		private void CreateDatabase ()
		{
			using (UnitOfWork.Start()) {
				DatabaseMigrator.Execute ();
			}
		}

	}
}