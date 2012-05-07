using System;
using Vici.CoolStorage;

namespace ArtekSoftware.Conference.LocalData
{
	[MapTo("schedule")]
  	public class ScheduleEntity : CSObject<ScheduleEntity, string>
	{
		public ScheduleEntity ()
		{
		}
	}
}

