using System;

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
	}
}