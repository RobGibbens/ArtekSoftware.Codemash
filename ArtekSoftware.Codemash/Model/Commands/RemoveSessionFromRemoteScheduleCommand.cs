using System;
using MonoQueue;
using System.Threading;
using Catnap;
using System.Linq;
using RestSharp;

namespace ArtekSoftware.Codemash
{
	[Serializable]
    public class RemoveSessionFromRemoteScheduleCommand : MessageBase
	{
		public string SessionUri { get; set; }
		public int ScheduleId { get; set; }
		private Uri Endpoint { get; set; }
		private INetworkStatusCheck _networkStatusCheck;
		
		public RemoveSessionFromRemoteScheduleCommand (INetworkStatusCheck networkStatusCheck)
		{
			_networkStatusCheck = networkStatusCheck;
		}
		
		public override void Execute ()
		{
			ITestFlightProxy testFlight = new TestFlightProxy();
			
			if (_networkStatusCheck.IsReachable ()) {
				testFlight.PassCheckpoint ("Started RemoveSessionFromRemoteScheduleCommand.Execute");
				
				var client = new RestClient ();
				client.BaseUrl = "http://conference.apphb.com/api/schedule";
			
				var request = new RestRequest ("{userName}/{conferenceName}/Sessions/remove", Method.POST);
				request.AddParameter(new Parameter() { Name = "userName", Type = ParameterType.UrlSegment, Value = "RobGibbens"  }); //TODO : Get username
				request.AddParameter(new Parameter() { Name = "conferenceName", Type = ParameterType.UrlSegment, Value = "CodeMash"  });
				//request.AddParameter(new Parameter() { Name = "sessionUri", Type = ParameterType.RequestBody, Value = this.SessionUri  });
				request.AddParameter("text/xml", this.SessionUri, ParameterType.RequestBody);
				
				request.RequestFormat = DataFormat.Json;
				
				using (new NetworkIndicator()) {
					var response = client.Execute (request);
					if (response.StatusCode != System.Net.HttpStatusCode.OK)
					{
						throw new ApplicationException("Error sending AddSessionToRemoteScheduleCommand - " + this.SessionUri);
					}
				}
				
				testFlight.PassCheckpoint ("Finished AddSessionToRemoteScheduleCommand.Execute");
				
			}
		}
	}
}

