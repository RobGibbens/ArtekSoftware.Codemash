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
			
			return sessions.OrderBy (x => x.StartDate).ToList ();
		}
		
		public RootElement GetScheduledSessionDialog (IEnumerable<ScheduledSessionEntity> sessions, bool isRefresh)
		{
			var root = new RootElement ("Schedule");
			
			var query = (
		        from session in sessions
		        group session by (session.StartDate) into sessionGroup
		        select new { 
							StartDate = sessionGroup.Key, 
				           	SectionName = sessionGroup.Key.DayOfWeek + " " + sessionGroup.Key.ToString("h:mm tt"),
							Sessions = sessionGroup 
							}
					).
		            OrderBy (letter => letter.StartDate);			
			
			foreach (var sessionGroup in query) {
				var section = new Section (sessionGroup.StartDate.DayOfWeek.ToString() + " " + sessionGroup.StartDate.ToString ("h:mm tt"));
				
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