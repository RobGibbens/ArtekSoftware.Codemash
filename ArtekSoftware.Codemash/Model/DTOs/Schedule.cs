using System;
using System.Collections.Generic;

namespace ArtekSoftware.Codemash
{
	public class Schedule
	{
		public Schedule ()
		{
			Id = Guid.NewGuid();
		}
		
		public Guid Id { get; set; }

		public string ConferenceName { get; set; }

		//public string[] SessionURIs { get; set; } 
	}
}

