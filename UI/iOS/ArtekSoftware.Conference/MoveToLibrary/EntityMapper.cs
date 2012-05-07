//using System;
//using System.IO;
////using Catnap;
////using Catnap.Adapters;
////using Catnap.Common.Logging;
////using Catnap.Maps;
////using Catnap.Maps.Impl;
////using MonoQueue;
//
//namespace ArtekSoftware.Codemash
//{
//	public class EntityMapper
//	{
//		public void Mapper ()
//		{
//			//var errorMessageEntity = new ErrorMessageEntity ();
//			//var pendingMessageEntity = new PendingMessageEntity ();
//			
//			Log.Level = LogLevel.Off;
//			Domain.Configure
//            (
//				Map.Entity<SessionEntity> ()
//        			.Table (SessionEntity.TableName)
//                    .Map (new ValuePropertyMap<SessionEntity, string> (x => x.URI))
//                    .Map (new ValuePropertyMap<SessionEntity, string> (x => x.Title))
//                    .Map (new ValuePropertyMap<SessionEntity, string> (x => x.Abstract))
//                    .Map (new ValuePropertyMap<SessionEntity, string> (x => x.Start))
//                    .Map (new ValuePropertyMap<SessionEntity, string> (x => x.Room))
//                    .Map (new ValuePropertyMap<SessionEntity, string> (x => x.Difficulty))
//                    .Map (new ValuePropertyMap<SessionEntity, string> (x => x.SpeakerName))
//                    .Map (new ValuePropertyMap<SessionEntity, string> (x => x.Technology))
//                    .Map (new ValuePropertyMap<SessionEntity, string> (x => x.SpeakerURI)),
//				
//				Map.Entity<SpeakerEntity> ()
//					.Table (SpeakerEntity.TableName)
//					.Map (new ValuePropertyMap<SpeakerEntity, string> (x => x.SpeakerURI))
//					.Map (new ValuePropertyMap<SpeakerEntity, string> (x => x.Name))
//					.Map (new ValuePropertyMap<SpeakerEntity, string> (x => x.Biography))
//					.Map (new ValuePropertyMap<SpeakerEntity, string> (x => x.TwitterName))
//					.Map (new ValuePropertyMap<SpeakerEntity, string> (x => x.BlogUrl)),
//				
//			   Map.Entity<RefreshEntity> ()
//					.Table (RefreshEntity.TableName)
//					.Map (new ValuePropertyMap<RefreshEntity, DateTime> (x => x.LastRefreshTime))
//					.Map (new ValuePropertyMap<RefreshEntity, string> (x => x.EntityName)),
//					
//				Map.Entity<ScheduledSessionEntity> ()
//        			.Table (ScheduledSessionEntity.TableName)
//                    .Map (new ValuePropertyMap<ScheduledSessionEntity, string> (x => x.URI))
//                    .Map (new ValuePropertyMap<ScheduledSessionEntity, string> (x => x.Title))
//                    .Map (new ValuePropertyMap<ScheduledSessionEntity, string> (x => x.Abstract))
//                    .Map (new ValuePropertyMap<ScheduledSessionEntity, string> (x => x.Start))
//                    .Map (new ValuePropertyMap<ScheduledSessionEntity, string> (x => x.Room))
//                    .Map (new ValuePropertyMap<ScheduledSessionEntity, string> (x => x.Difficulty))
//                    .Map (new ValuePropertyMap<ScheduledSessionEntity, string> (x => x.SpeakerName))
//                    .Map (new ValuePropertyMap<ScheduledSessionEntity, string> (x => x.Technology))
//                    .Map (new ValuePropertyMap<ScheduledSessionEntity, string> (x => x.SpeakerURI)),
//					
//				Map.Entity<RemoteQueueEntity> ()
//        			.Table (RemoteQueueEntity.TableName)
//                    .Map (new ValuePropertyMap<RemoteQueueEntity, string> (x => x.URI))
//                    .Map (new ValuePropertyMap<RemoteQueueEntity, string> (x => x.ConferenceName))
//                    .Map (new ValuePropertyMap<RemoteQueueEntity, string> (x => x.UserName))
//                    .Map (new ValuePropertyMap<RemoteQueueEntity, DateTime> (x => x.DateQueuedOn))
//                    .Map (new ValuePropertyMap<RemoteQueueEntity, string> (x => x.AddOrRemove))
//				
//					/*
//				Map.Entity<ErrorMessageEntity> ()
//        			.Table (errorMessageEntity.TableName)
//                    .Map (new ValuePropertyMap<ErrorMessageEntity, string> (x => x.CreatedOnString))
//                    .Map (new ValuePropertyMap<ErrorMessageEntity, byte[]> (x => x.MessageContents))
//                    .Map (new ValuePropertyMap<ErrorMessageEntity, string> (x => x.MessageType))
//                    .Map (new ValuePropertyMap<ErrorMessageEntity, int> (x => x.RetryCount)),
//				
//				Map.Entity<PendingMessageEntity> ()
//					.Table (pendingMessageEntity.TableName)
//                    .Map (new ValuePropertyMap<PendingMessageEntity, string> (x => x.CreatedOnString))
//                    .Map (new ValuePropertyMap<PendingMessageEntity, byte[]> (x => x.MessageContents))
//                    .Map (new ValuePropertyMap<PendingMessageEntity, string> (x => x.MessageType))
//                    .Map (new ValuePropertyMap<PendingMessageEntity, int> (x => x.RetryCount))		
//                    */			
//            );
//		}
//	}
//}
//
