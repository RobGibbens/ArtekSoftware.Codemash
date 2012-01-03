using System.Collections.Generic;
using System.Linq;
using RestSharp;

namespace ArtekSoftware.Codemash
{
	public class RemoteSessionsRepository
	{
		public IList<Session> GetSessions ()
		{	
			var regularSessions = GetRegularSessions();
			var precompilerSessions = GetPrecompilerSessions();
			
			var combinedSessions = regularSessions.Union(precompilerSessions);
			
			return combinedSessions.ToList();
		}
		
		private IList<Session> GetRegularSessions ()
		{
			ITestFlightProxy testFlight = new TestFlightProxy();
			
			IList<Session> sessions;
			testFlight.PassCheckpoint ("Started RemoteSessionsRepository.GetSessions");
			
			var client = new RestClient ();
			client.BaseUrl = "http://codemash.org";
			
			var request = new RestRequest ();
			request.Resource = "rest/sessions";
			request.RequestFormat = DataFormat.Json;
			using (new NetworkIndicator()) {
				var response = client.Execute<SessionsList> (request);
				sessions = new List<Session> ();
				if (response != null && response.Data != null && response.Data.Sessions != null) {
					sessions = response.Data.Sessions.OrderBy (x => x.Title.Trim ()).ToList ();
				}
			}
			
			foreach (var session in sessions) {
				session.Title = session.Title.Trim ();
			}
				
			testFlight.PassCheckpoint ("Finished RemoteSessionsRepository.GetSessions");
			
			return sessions;
		}
		
		private IList<Session> GetPrecompilerSessions ()
		{
			ITestFlightProxy testFlight = new TestFlightProxy();
			
			IList<Session> sessions;
			testFlight.PassCheckpoint ("Started RemoteSessionsRepository.GetSessions");
			
			var client = new RestClient ();
			client.BaseUrl = "http://codemash.org";
			
			var request = new RestRequest ();
			request.Resource = "rest/precompiler";
			request.RequestFormat = DataFormat.Json;
			using (new NetworkIndicator()) {
				var response = client.Execute<SessionsList> (request);
				sessions = new List<Session> ();
				if (response != null && response.Data != null && response.Data.Sessions != null) {
					sessions = response.Data.Sessions.OrderBy (x => x.Title.Trim ()).ToList ();
				}
			}
			
			foreach (var session in sessions) {
				session.Title = session.Title.Trim ();
			}
				
			testFlight.PassCheckpoint ("Finished RemoteSessionsRepository.GetSessions");
			
			return sessions;
		}		

	}
}