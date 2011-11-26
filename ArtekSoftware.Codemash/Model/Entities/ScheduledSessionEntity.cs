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

namespace ArtekSoftware.Codemash
{
	public class ScheduledSessionEntity : SessionEntity
	{
		public new static string TableName = "ScheduledSessions";
		public new static string CreateTableSql = @"CREATE TABLE " + TableName + " (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, URI VARCHAR, Title VARCHAR,Abstract VARCHAR, Start VARCHAR, Room VARCHAR, Difficulty VARCHAR, SpeakerName VARCHAR, Technology VARCHAR, SpeakerURI VARCHAR)";
		
	}
}