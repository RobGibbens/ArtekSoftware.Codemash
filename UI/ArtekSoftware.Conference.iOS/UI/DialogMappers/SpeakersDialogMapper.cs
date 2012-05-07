using System;
using System.Collections.Generic;
using System.Linq;
//using Catnap;
using MonoTouch.Dialog;
////using MonoQueue;
using ArtekSoftware.Conference;
using ArtekSoftware.Conference.LocalData;
using RestSharp;
using ArtekSoftware.Conference.Data;

namespace ArtekSoftware.Codemash
{
	public class SpeakersDialogMapper
	{
		private INetworkStatusCheck _networkStatusCheck;

		public SpeakersDialogMapper (INetworkStatusCheck networkStatusCheck)
		{
			_networkStatusCheck = networkStatusCheck;
		}		
		
		public IEnumerable<SpeakerEntity> GetSpeakers (bool isRefresh)
		{
			var testFlightProxy = new TestFlightProxy();
			var restClient = new RestClient();
			var remoteConfiguration = new RemoteConfiguration();
			var remoteRepository = new RemoteSpeakersRepository(testFlightProxy, restClient, remoteConfiguration);
			var networkStatusCheck = new NetworkStatusCheck();
			
			string conferenceSlug = AppDelegate.ConferenceSlug;
			var repo = new SpeakerRepository(remoteRepository, networkStatusCheck, testFlightProxy, restClient, remoteConfiguration, conferenceSlug);
			var entities = repo.GetEntities(isRefresh:false);

			
			return entities;			
		}
		
		public RootElement GetSpeakerDialog (IEnumerable<SpeakerEntity> speakers, List<string> sectionTitles)
		{
			var root = new RootElement ("Speakers");
			foreach (var sectionTitle in sectionTitles) {               
				var section = new Section (sectionTitle, String.Empty);
				foreach (var speaker in speakers.Where(e => e.Name.Substring(0,1) == sectionTitle)) { 
					var element = new SpeakerEventCell (speaker);
					
					section.Add (element);
					//section.Add(new StringElement(speaker.Name, "Title"));
				}
				root.Add (section);
			}
			
			
			
			//var root = new RootElement ("Speakers");
			
//			var query = (
//		        from speaker in speakers
//		        group speaker by speaker.Name.Substring (1, 0) into speakerGroup
//		        select new { 
//							DayOfWeek = speakerGroup.Key, 
//							Speakers = speakerGroup 
//							}
//					).
//		            OrderBy (letter => letter.DayOfWeek);
//
//			foreach (var speakerGroup in query) {
//				var section = new Section (speakerGroup.DayOfWeek.ToString ());
//				
//				
//				foreach (var speaker in speakerGroup.Speakers) {
//					//var element = new MultilineElement(session.Title);
//					//var element = new SessionCell(session);
//					var element = new SpeakerEventCell (speaker);
//					section.Add (element);
//				}				
//				
//				root.Add (section);
//				
//			}
//			
//			root.UnevenRows = true;
			return root;
			
		}

	}
}