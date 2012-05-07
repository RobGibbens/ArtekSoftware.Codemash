using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

//using Catnap;
using MonoTouch.Dialog;

////using MonoQueue;
using RestSharp;
using ArtekSoftware.Conference;
using ArtekSoftware.Conference.LocalData;
using ArtekSoftware.Conference.Data;

namespace ArtekSoftware.Codemash
{
	public class SessionsDialogMapper
	{
		private INetworkStatusCheck _networkStatusCheck;

		public SessionsDialogMapper (INetworkStatusCheck networkStatusCheck)
		{
			_networkStatusCheck = networkStatusCheck;
		}

		public IEnumerable<SessionEntity> GetSessions (bool isRefresh)
		{
			var testFlightProxy = new TestFlightProxy ();
			var restClient = new RestClient ();
			var remoteConfiguration = new RemoteConfiguration ();
			var remoteRepository = new RemoteSessionsRepository (
				testFlightProxy,
				restClient,
				remoteConfiguration
			);
			var networkStatusCheck = new NetworkStatusCheck ();
			
			string conferenceSlug = AppDelegate.ConferenceSlug;
			var repo = new SessionRepository (
				remoteRepository,
				networkStatusCheck,
				testFlightProxy,
				restClient,
				remoteConfiguration,
				conferenceSlug
			);
			var entities = repo.GetEntities (isRefresh: false);
			
			return entities;
		}
		
		public RootElement GetSessionDialog (IEnumerable<SessionEntity> sessions)
		{
			var root = new RootElement ("Sessions");
			
			var query = (
		        from session in sessions
		        group session by (session.Start) into sessionGroup
		        select new { 
							StartDate = sessionGroup.Key, 
				           	SectionName = sessionGroup.Key.DayOfWeek + " " + sessionGroup.Key.ToString ("h:mm tt"),
							Sessions = sessionGroup 
							}
					).
		            OrderBy (s => s.StartDate);

			foreach (var sessionGroup in query) {
				string sectionTitle;
				if (sessionGroup.SectionName == "Monday 12:00 AM") {
					sectionTitle = "Time Not Posted - Refresh";
				} else {
					sectionTitle = sessionGroup.SectionName.ToString ();
				}
				
				var section = new Section (sectionTitle);

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