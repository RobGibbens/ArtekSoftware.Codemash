using System;
using System.Collections.Generic;
using Catnap;

namespace ArtekSoftware.Codemash
{
	[MonoTouch.Foundation.Preserve(AllMembers=true)]
	public class Session
	{
		public string Title { get; set; }
		public string Abstract { get; set; }
		public DateTime Start  { get; set; }
		public DateTime End  { get; set; }
		public string Difficulty { get; set; }
		public string SpeakerName { get; set; }
		public string Technology {get;set;}
		public string URI { get; set; }
		public string EventType  { get; set; }
		public string SessionLookupId {get;set;}
		public string SpeakerURI  { get; set; }
		public string Room {get;set;}
	}

}