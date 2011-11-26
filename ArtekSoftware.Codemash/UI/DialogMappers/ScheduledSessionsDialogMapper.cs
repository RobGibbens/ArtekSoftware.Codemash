using System;
using MonoTouch.Dialog;
using System.Collections.Generic;
using Catnap;
using System.Linq;

namespace ArtekSoftware.Codemash
{
	public class ScheduledSessionsDialogMapper
	{
		public IEnumerable<ScheduledSessionEntity> GetScheduledSessions (bool isRefresh)
		{
			IEnumerable<ScheduledSessionEntity> sessions = null;
			
			if (UnitOfWork.IsUnitOfWorkStarted()) {
				var localRepository = new LocalScheduledSessionsRepository ();
				sessions = localRepository.Find ();
			} else {
				using (UnitOfWork.Start()) {
					var localRepository = new LocalScheduledSessionsRepository ();
					sessions = localRepository.Find ();
				}
			}
			
			return sessions.OrderBy (x => x.Start).ToList ();
		}
		
		public RootElement GetScheduledSessionDialog (IEnumerable<ScheduledSessionEntity> sessions, bool isRefresh)
		{
			var root = new RootElement ("Schedule");
			
			var query = (
		        from session in sessions
		        group session by session.Start.DayOfWeek into sessionGroup
		        select new { 
							DayOfWeek = sessionGroup.Key, 
							Sessions = sessionGroup 
							}
					).
		            OrderBy (letter => letter.DayOfWeek);

			foreach (var sessionGroup in query) {
				var section = new Section (sessionGroup.DayOfWeek.ToString ());
				
				foreach (var session in sessionGroup.Sessions) {
					var element = new SessionEventCell (session);
					section.Add (element);
				}				
				
				root.Add (section);
				
			}
			
			root.UnevenRows = true;
			return root;
			
		}

	}
}