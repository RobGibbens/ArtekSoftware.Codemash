using System;
using MonoTouch.Dialog;
using System.Collections.Generic;
using Catnap;
using System.Linq;


using System.Threading;

namespace ArtekSoftware.Codemash
{
	public class SessionsDialogMapper
	{
		public IEnumerable<SessionEntity> GetSessions (bool isRefresh)
		{
			IEnumerable<SessionEntity> sessions = null;
			bool shouldCache = false;
			
			if (UnitOfWork.IsUnitOfWorkStarted ()) {
				var localRepository = new LocalSessionsRepository ();
				int sessionCount = localRepository.Count ();
			
				if (sessionCount == 0 || isRefresh) {

					if (NetworkStatusCheck.IsReachable ()) {
						var remoteRepository = new RemoteSessionsRepository ();
						IList<Session> sessionDtos = remoteRepository.GetSessions ();
						var cacheRepository = new SessionsCacheRepository ();
					cacheRepository.Cache (sessionDtos);
					} else {
						ModalDialog.Alert ("Network offline", "Cannot connect to the network");
					}
				}
			
				sessions = localRepository.Find ();				
			
			} else {
				IList<Session> sessionDtos = null;
				
				using (UnitOfWork.Start()) {
					var localRepository = new LocalSessionsRepository ();
					int sessionCount = localRepository.Count ();
			
					if (sessionCount == 0 || isRefresh) {
						if (NetworkStatusCheck.IsReachable ()) {
							var remoteRepository = new RemoteSessionsRepository ();
							sessionDtos = remoteRepository.GetSessions ();
							shouldCache = true;
						} else {
							ModalDialog.Alert ("Network offline", "Cannot connect to the network");
						}
					}
				}
				
				if (shouldCache) {
					var cacheRepository = new SessionsCacheRepository ();
					cacheRepository.Cache (sessionDtos);
				}
				
				using (UnitOfWork.Start()) {
					var localRepository = new LocalSessionsRepository ();
					sessions = localRepository.Find ();
				}
			}
			
			return sessions.OrderBy (x => x.StartDate).ToList ();
		}
		
		public RootElement GetSessionDialog (IEnumerable<SessionEntity> sessions)
		{
			var root = new RootElement ("Sessions");
			
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
				var section = new Section (sessionGroup.SectionName.ToString ());
				
				
				foreach (var session in sessionGroup.Sessions) {
					//var element = new MultilineElement(session.Title);
					//var element = new SessionCell(session);
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