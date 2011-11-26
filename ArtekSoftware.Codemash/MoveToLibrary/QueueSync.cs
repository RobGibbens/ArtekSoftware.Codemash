using System;
using System.Linq;

namespace ArtekSoftware.Codemash
{
	public class QueueSync
	{
		public void Sync ()
		{
			NetworkStatusCheck networkStatusCheck = new NetworkStatusCheck();
			if (networkStatusCheck.IsOnline())
			{
				RemoteScheduledSessionsRepository remoteScheduleRepository = new RemoteScheduledSessionsRepository();
				LocalQueueRepository localQueueRepository = new LocalQueueRepository();
				LocalSessionsRepository localSessionsRepository = new LocalSessionsRepository();
				
				if (localQueueRepository.Count() > 0)
				{
					var allSessions = localSessionsRepository.Find();
					
					var queuedItems = localQueueRepository.Find();
					foreach (var queuedItem in queuedItems)
					{
						//var remoteSchedule = new Entity
						var sessionEntity = allSessions.Where(x => x.URI == queuedItem.URI);
						var schedule = new Schedule()
						{
							ConferenceName = queuedItem.ConferenceName,
							
						};
						remoteScheduleRepository.Save(schedule);
					}
				}
			}
			
		}
	}
}

