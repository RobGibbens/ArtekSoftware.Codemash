using System;
using System.Collections.Generic;

namespace ArtekSoftware.Codemash
{
	
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