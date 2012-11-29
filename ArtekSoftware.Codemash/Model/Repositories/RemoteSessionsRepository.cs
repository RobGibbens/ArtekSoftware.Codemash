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
//using MonoTouch.TestFlight;

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
			IList<Session> sessions = new List<Session>();
			//TestFlight.PassCheckpoint ("Started RemoteSessionsRepository.GetSessions");
			
			var client = new RestClient ();
			//client.BaseUrl = "http://codemash.org";
			client.BaseUrl = "http://rest.codemash.org";
			var request = new RestRequest ();
			request.DateFormat = "yyyy-MM-ddTHH:mm:ssZ"; //2013-01-08 18:30
			//request.Resource = "rest/sessions";
			request.Resource = "api/sessions.json";
			//request.RequestFormat = DataFormat.Xml;
			request.RequestFormat = DataFormat.Json;
			
			using (new NetworkIndicator()) {
				//var response = client.Execute<SessionsList> (request);
				var response = client.Execute<List<Session>>(request);
				if (response != null && response.Data != null) {
					sessions = response.Data as List<Session>;

					sessions = sessions.OrderBy (x => x.Title.Trim ()).ToList ();
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
				
			//TestFlight.PassCheckpoint ("Finished RemoteSessionsRepository.GetSessions");
			
			return sessions;
		}

	}
}