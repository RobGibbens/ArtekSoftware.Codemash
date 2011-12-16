using System;
using Catnap;

namespace ArtekSoftware.Codemash
{
	public class SessionEntity : Entity
	{
		public static string TableName = "Sessions";
		public static string CreateTableSql = @"CREATE TABLE " + TableName + " (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, URI VARCHAR, Title VARCHAR,Abstract VARCHAR, Start VARCHAR, Room VARCHAR, Difficulty VARCHAR, SpeakerName VARCHAR, Technology VARCHAR, SpeakerURI VARCHAR)";
		 
		public string URI { get; set; }
		
		public string Title { get; set; }
		
		public string Abstract { get; set; }
		
		public DateTime StartDate { 
			get
			{
				DateTime startDate = DateTime.MinValue;
				
				DateTime.TryParse(this.Start, out startDate);
				
				return startDate;
			}
		}
		
		public string Start  { get; set; }
		
		public string Room  { get; set; }
		
		public string Difficulty { get; set; }
		
		public string SpeakerName { get; set; }
		
		public string Technology { get; set; }
		
		public string SpeakerURI  { get; set; }
		
	}
}