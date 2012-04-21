using System;
using System.Collections.Generic;

namespace ArtekSoftware.Codemash
{
	[MonoTouch.Foundation.Preserve(AllMembers=true)]
	public class Session
	{
		public string URI { get; set; }
		public string Title { get; set; }
		public string Abstract { get; set; }
		public DateTime Start  { get; set; }
		public string Room  { get; set; }
		public string Difficulty { get; set; }
		public string SpeakerName { get; set; }
		public string Technology {get;set;}
		public string SpeakerURI  { get; set; }

	}
	
	[MonoTouch.Foundation.Preserve(AllMembers=true)]
	public class Session2
	{
		public string slug { get; set; }
		public string title { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string room { get; set; }
	}
}