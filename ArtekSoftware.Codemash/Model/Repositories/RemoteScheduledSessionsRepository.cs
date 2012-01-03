using RestSharp;
using MonoQueue;

namespace ArtekSoftware.Codemash
{
	public class RemoteScheduledSessionsRepository
	{
		private INetworkStatusCheck _networkStatusCheck;
		public RemoteScheduledSessionsRepository (INetworkStatusCheck networkStatusCheck)
		{
			_networkStatusCheck = networkStatusCheck;
		}
		
		public Schedule GetSchedule (string userName)
		{
			ITestFlightProxy testFlight = new TestFlightProxy();
			
			Schedule schedule;
			
			if (_networkStatusCheck.IsReachable ()) {
				testFlight.PassCheckpoint ("Started RemoteScheduledSessionsRepository.GetSchedule");
				var client = new RestClient ();
				client.BaseUrl = "http://conference.apphb.com/api/schedule/";
			
				var request = new RestRequest ();
				request.Resource = "CodeMash/" + userName;
				request.RequestFormat = DataFormat.Json;
				using (new NetworkIndicator()) {
					var response = client.Execute<Schedule> (request);

					schedule = new Schedule ();
					if (response != null && response.Data != null) {
						schedule = response.Data;
					}
				}
				testFlight.PassCheckpoint ("Finished RemoteScheduledSessionsRepository.GetSchedule");
			
				return schedule;
			} else {
				return null; //TODO UIAlertView
			}
		}
		
		public void Save (Schedule schedule)
		{
		}
		
		public void AddToRemote (string sessionUri, string userName, string conferenceName)
		{
			//{userName}/{conferenceName}/Sessions/add
			//ObjectContent<string> sessionUri, string userName, string conferenceName
			//http://conference.apphb.com/
			ITestFlightProxy testFlight = new TestFlightProxy();
			
			if (_networkStatusCheck.IsReachable ()) {
				testFlight.PassCheckpoint ("Started RemoteScheduledSessionsRepository.Save");
				
				var client = new RestClient ();
				client.BaseUrl = "http://conference.apphb.com/api/schedule/";
			
				var request = new RestRequest (Method.POST);
				request.AddParameter(new Parameter() { Name = "sessionUri", Type = ParameterType.RequestBody, Value = sessionUri  });
				request.AddParameter(new Parameter() { Name = "userName", Type = ParameterType.UrlSegment, Value = userName  });
				request.AddParameter(new Parameter() { Name = "conferenceName", Type = ParameterType.UrlSegment, Value = conferenceName  });
				request.RequestFormat = DataFormat.Json;
				
				using (new NetworkIndicator()) {
					var response = client.Execute<Schedule> (request);
				}
				testFlight.PassCheckpoint ("Finished RemoteScheduledSessionsRepository.Save");
				
			} else {
			}
		}
	}

}