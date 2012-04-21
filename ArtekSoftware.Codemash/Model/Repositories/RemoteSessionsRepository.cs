using System.Collections.Generic;
using System.Linq;
using RestSharp;
using System.Net;
using System;
using System.IO;
using System.Text;

namespace ArtekSoftware.Codemash
{
	public class RemoteSessionsRepository
	{
		public IList<Session> GetSessions ()
		{	
			var regularSessions2 = GetRegularSessions2();
			
			var regularSessions = GetRegularSessions();
			var precompilerSessions = GetPrecompilerSessions();
			
			var combinedSessions = regularSessions.Union(precompilerSessions);
			
			return combinedSessions.ToList();
		}
		
		private IList<Session2> GetRegularSessions2 ()
		{
			StringBuilder pageContent = new StringBuilder();

			Uri uri = new Uri("http://conference.apphb.com/api/MobiDevDay-2012/sessions?format=json");
			HttpWebRequest request =
                    (HttpWebRequest)WebRequest.Create (uri);

                request.Method = "GET";

                HttpWebResponse response =
                    (HttpWebResponse)request.GetResponse();

                StreamReader sr = new StreamReader(
                    response.GetResponseStream(), Encoding.UTF8);

                string line;
                while ((line = sr.ReadLine ()) != null)
                {
                    pageContent.Append (line);
				}
                sr.Close ();
                response.Close ();
			var x = pageContent.ToString();
			return null;
			
//			ITestFlightProxy testFlight = new TestFlightProxy();
//			
//			IList<Session2> sessions;
//			testFlight.PassCheckpoint ("Started RemoteSessionsRepository.GetSessions");
//			
//			var client = new RestClient ();
//			//client.BaseUrl = "http://conference.apphb.com/";
//			var x = new WebRequest();
//			var request = new RestRequest ();
//			request.Resource = "http://conference.apphb.com/api/MobiDevDay-2012/sessions?format=json";
//			//request.Resource = "api/MobiDevDay-2012/sessions?format=json";
//			request.RequestFormat = DataFormat.Json;
//			using (new NetworkIndicator()) {
//				var response = client.Execute<SessionsList2> (request);
//				sessions = new List<Session2> ();
//				if (response != null && response.Data != null && response.Data.Sessions != null) {
//					sessions = response.Data.Sessions.OrderBy (x => x.title.Trim ()).ToList ();
//				}
//			}
//			
//			foreach (var session in sessions) {
//				session.title = session.title.Trim ();
//			}
//				
//			testFlight.PassCheckpoint ("Finished RemoteSessionsRepository.GetSessions");
//			
//			return sessions;
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