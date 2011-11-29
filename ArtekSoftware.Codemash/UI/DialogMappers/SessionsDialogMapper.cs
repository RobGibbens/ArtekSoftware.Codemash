using System;
using MonoTouch.Dialog;
using System.Collections.Generic;
using Catnap;
using System.Linq;
using MonoTouch.TestFlight;
using System.Threading;

namespace ArtekSoftware.Codemash
{
	public class SessionsDialogMapper
	{
		public IEnumerable<SessionEntity> GetSessions (bool isRefresh)
		{
			TestFlight.PassCheckpoint ("SessionsDialogMapper.GetSessions - 1");
			Thread.Sleep (1000);
			
			IEnumerable<SessionEntity> sessions = null;
			
			if (UnitOfWork.IsUnitOfWorkStarted ()) {
				TestFlight.PassCheckpoint ("SessionsDialogMapper.GetSessions - 2");
				Thread.Sleep (1000);
				
				var localRepository = new LocalSessionsRepository ();
				int sessionCount = localRepository.Count ();
			
				if (sessionCount == 0 || isRefresh) {
					TestFlight.PassCheckpoint ("SessionsDialogMapper.GetSessions - 3");
					Thread.Sleep (1000);	
					
					var networkStatusCheck = new NetworkStatusCheck ();
					if (networkStatusCheck.IsOnline ()) {
						TestFlight.PassCheckpoint ("SessionsDialogMapper.GetSessions - 4");
						Thread.Sleep (1000);
						
						var remoteRepository = new RemoteSessionsRepository ();
						IList<Session> sessionDtos = remoteRepository.GetSessions ();
						TestFlight.PassCheckpoint ("SessionsDialogMapper.GetSessions - 5");
						Thread.Sleep (1000);
						
						localRepository.Cache (sessionDtos);
						TestFlight.PassCheckpoint ("SessionsDialogMapper.GetSessions - 6");
						Thread.Sleep (1000);
					} else {
						ModalDialog.Alert ("Network offline", "Cannot connect to the network");
					}
				}
			
				sessions = localRepository.Find ();				
			} else {
				using (UnitOfWork.Start()) {
					var localRepository = new LocalSessionsRepository ();
					int sessionCount = localRepository.Count ();
			
					if (sessionCount == 0 || isRefresh) {
						var networkStatusCheck = new NetworkStatusCheck ();
						if (networkStatusCheck.IsOnline ()) {
							var remoteRepository = new RemoteSessionsRepository ();
							IList<Session> sessionDtos = remoteRepository.GetSessions ();
							localRepository.Cache (sessionDtos);
						} else {
							ModalDialog.Alert ("Network offline", "Cannot connect to the network");
						}
					}
			
					sessions = localRepository.Find ();
				}
			}
			
			return sessions.OrderBy (x => x.Start).ToList ();
		}
		
		public RootElement GetSessionDialog (IEnumerable<SessionEntity> sessions)
		{
			var root = new RootElement ("Sessions");
			
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