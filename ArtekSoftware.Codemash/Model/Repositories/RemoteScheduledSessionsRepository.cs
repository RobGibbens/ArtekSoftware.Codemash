using System;
using System.Linq;
using Catnap;
using System.Collections.Generic;
using Catnap.Common.Logging;
using Catnap.Database;
using Catnap.Find;
using Catnap.Maps;
using Catnap.Adapters;
using Catnap.Maps.Impl;
using Catnap.Migration;
using System.Threading;
using Catnap.Find.Conditions;
using RestSharp;
using MonoTouch.TestFlight;

namespace ArtekSoftware.Codemash
{
	public class RemoteScheduledSessionsRepository
	{
		public Schedule GetSchedule (string userName)
		{
			Schedule schedule;
			var networkStatusCheck = new NetworkStatusCheck ();
			if (networkStatusCheck.IsOnline ()) {
				TestFlight.PassCheckpoint ("Started RemoteScheduledSessionsRepository.GetSchedule");
				var client = new RestClient ();
				client.BaseUrl = "http://conference.apphb.com/api/schedule/";
			
				var request = new RestRequest ();
				request.Resource = "CodeMash/" + userName;
				request.RequestFormat = DataFormat.Xml;
				using (new NetworkIndicator()) {
					var response = client.Execute<Schedule> (request);

					schedule = new Schedule ();
					if (response != null && response.Data != null) {
						schedule = response.Data;
					}
				}
				TestFlight.PassCheckpoint ("Finished RemoteScheduledSessionsRepository.GetSchedule");
			
				return schedule;
			} else {
				return null; //TODO UIAlertView
			}
		}
		
		public void Save (Schedule schedule)
		{
			var networkStatusCheck = new NetworkStatusCheck ();
			if (networkStatusCheck.IsOnline ()) {
				TestFlight.PassCheckpoint ("Started RemoteScheduledSessionsRepository.Save");
				
				var client = new RestClient ();
				client.BaseUrl = "http://conference.apphb.com/api/schedule/";
			
				var request = new RestRequest ("postedSession", Method.POST);
				request.AddObject (schedule);
				request.RequestFormat = DataFormat.Json;
				using (new NetworkIndicator()) {
					var response = client.Execute<Schedule> (request);
				}
				TestFlight.PassCheckpoint ("Finished RemoteScheduledSessionsRepository.Save");
				
			} else {
			}
		}
	}
	
	
}

