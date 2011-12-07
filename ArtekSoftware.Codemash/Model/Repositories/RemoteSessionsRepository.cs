using System;
using System.Collections.Generic;
using RestSharp;
using System.Linq;
using Catnap.Common.Logging;
using Catnap.Database;
using Catnap.Find;
using Catnap.Maps;
using Catnap.Adapters;
using Catnap.Maps.Impl;
using Catnap;
using Catnap.Migration;
using Catnap.Find.Conditions;
using System.Threading;


namespace ArtekSoftware.Codemash
{
	public class RemoteSessionsRepository
	{
		
		private DateTime RandomDay ()
		{
			DateTime start = new DateTime (2011, 1, 11);
			Random gen = new Random (DateTime.Now.Millisecond);

			int range = 2; //((TimeSpan)(DateTime.Today - start)).Days;           
			return start.AddDays (gen.Next (range));
		}

		public IList<Session> GetSessions ()
		{	
			IList<Session> sessions;
			TestFlightProxy.PassCheckpoint ("Started RemoteSessionsRepository.GetSessions");
			
			var client = new RestClient ();
			client.BaseUrl = "http://codemash.org";
			
			var request = new RestRequest ();
			request.Resource = "rest/sessions";
			request.RequestFormat = DataFormat.Xml;
			using (new NetworkIndicator()) {
				var response = client.Execute<SessionsList> (request);
//			var asyncHandle = client.ExecuteAsync<SessionsList>(request, response2 => {
//    			Console.WriteLine(response2.Data.Sessions.Count);
//			});
				sessions = new List<Session> ();
				if (response != null && response.Data != null && response.Data.Sessions != null) {
					sessions = response.Data.Sessions.OrderBy (x => x.Title.Trim ()).ToList ();
				}
			}
			
			foreach (var session in sessions) {
				session.Title = session.Title.Trim ();
				//Thread.Sleep (1);
			}
				
			//var refreshRepository = new LocalRefreshRepository ();
			//var refresh = refreshRepository.GetSession ();
			//refresh.LastRefreshTime = DateTime.Now;
			//refreshRepository.Save (refresh);
				
			TestFlightProxy.PassCheckpoint ("Finished RemoteSessionsRepository.GetSessions");
			
			return sessions;
		}

	}
}