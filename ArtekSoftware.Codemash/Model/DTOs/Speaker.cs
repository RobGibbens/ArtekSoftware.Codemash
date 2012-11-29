using System;
using System.Collections.Generic;

namespace ArtekSoftware.Codemash
{
	public class Speaker
	{
		public string SpeakerURI { get; set; }

		public string Name { get; set; }

		public string Biography { get; set; }

		public List<string> Sessions { get; set; }

		public string TwitterHandle { get; set; }

		public string BlogURL { get; set; }

		public string LookupId { get; set;}
	}
}