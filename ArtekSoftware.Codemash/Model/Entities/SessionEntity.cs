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
	public class SessionEntity : Entity
	{
		public static string TableName = "Sessions";
		public static string CreateTableSql = @"CREATE TABLE " + TableName + " (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, URI VARCHAR, Title VARCHAR,Abstract VARCHAR, Start VARCHAR, End VARCHAR, Room VARCHAR, Difficulty VARCHAR, SpeakerName VARCHAR, Technology VARCHAR, SpeakerURI VARCHAR)";
		 
		public string URI { get; set; }
		
		public string Title { get; set; }
		
		public string Abstract { get; set; }

		public DateTime Start  { get; set; }
		public DateTime End { get; set;}

		public string Room  { get; set; }
		
		public string Difficulty { get; set; }
		
		public string SpeakerName { get; set; }
		
		public string Technology { get; set; }
		
		public string SpeakerURI  { get; set; }
		
	}
}