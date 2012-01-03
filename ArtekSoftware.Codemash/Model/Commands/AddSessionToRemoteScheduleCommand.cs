using System;
using MonoQueue;
using System.Threading;
using Catnap;
using System.Linq;
using RestSharp;

namespace ArtekSoftware.Codemash
{
	[Serializable]
    public class AddSessionToRemoteScheduleCommand : MessageBase
	{
		public string SessionUri { get; set; }
		public int ScheduleId { get; set; }
		private Uri Endpoint { get; set; }
		private INetworkStatusCheck _networkStatusCheck;
		private ITestFlightProxy _testFlight;
		private IRestClient _restClient;
		
		public AddSessionToRemoteScheduleCommand (INetworkStatusCheck networkStatusCheck, ITestFlightProxy testFlight, IRestClient restClient)
		{
			_networkStatusCheck = networkStatusCheck;
			_testFlight = testFlight;
			_restClient = restClient;
		}
		
		public override void Execute ()
		{
			if (_networkStatusCheck.IsReachable ()) {
				_testFlight.PassCheckpoint ("Started RemoteScheduledSessionsRepository.Save");
				
				//var client = new RestClient ();
				_restClient.BaseUrl = "http://conference.apphb.com/api/schedule";
			
				var request = new RestRequest ("{userName}/{conferenceName}/Sessions/add", Method.POST);
				request.AddParameter(new Parameter() { Name = "userName", Type = ParameterType.UrlSegment, Value = "RobGibbens"  }); //TODO : Get username
				request.AddParameter(new Parameter() { Name = "conferenceName", Type = ParameterType.UrlSegment, Value = "CodeMash"  });
				//request.AddParameter(new Parameter() { Name = "sessionUri", Type = ParameterType.RequestBody, Value = this.SessionUri  });
				request.AddParameter("text/xml", this.SessionUri, ParameterType.RequestBody);
				
				request.RequestFormat = DataFormat.Json;
				
				using (new NetworkIndicator()) {
					var response = _restClient.Execute (request);
					if (response.StatusCode != System.Net.HttpStatusCode.OK)
					{
						throw new ApplicationException("Error sending AddSessionToRemoteScheduleCommand - " + this.SessionUri);
					}
				}
				
				_testFlight.PassCheckpoint ("Finished AddSessionToRemoteScheduleCommand.Execute");
				
			}
		}
	}
}

