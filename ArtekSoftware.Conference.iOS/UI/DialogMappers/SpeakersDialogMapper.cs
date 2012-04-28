using System;
using System.Collections.Generic;
using System.Linq;
using Catnap;
using MonoTouch.Dialog;
////using MonoQueue;
using ArtekSoftware.Conference;

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
			IEnumerable<SpeakerEntity> speakers = null;
			
			if (UnitOfWork.IsUnitOfWorkStarted ()) {
				var localRepository = new LocalSpeakersRepository ();
				int speakerCount = localRepository.Count ();
			
				if (speakerCount == 0 || isRefresh) {
					
					if (_networkStatusCheck.IsReachable ()) {
						var remoteRepository = new RemoteSpeakersRepository ();
						IList<Speaker> speakerDtos = remoteRepository.GetSpeakers ();
						var cache = new SpeakersCacheRepository ();
					cache.Cache (speakerDtos);
					} else {
						ModalDialog.Alert ("Network offline", "Cannot connect to the network");
					}
				}
			
				speakers = localRepository.Find ();
				
			} else {
				bool shouldCache = false;
				IList<Speaker> speakerDtos = null;
				
				using (UnitOfWork.Start()) {
					var localRepository = new LocalSpeakersRepository ();
					int speakerCount = localRepository.Count ();
			
					if (speakerCount == 0 || isRefresh) {
						
						if (_networkStatusCheck.IsReachable ()) {
							var remoteRepository = new RemoteSpeakersRepository ();
							speakerDtos = remoteRepository.GetSpeakers ();
							shouldCache = true;
						} else {
							ModalDialog.Alert ("Network offline", "Cannot connect to the network");
						}
					}
			
				}
				
				if (shouldCache) {
					var cache = new SpeakersCacheRepository ();
					cache.Cache (speakerDtos);
				}
				
				using (UnitOfWork.Start()) {
					var localRepository = new LocalSpeakersRepository ();
					speakers = localRepository.Find ();
				}
			}
			
			return speakers.OrderBy (x => x.Name).ToList ();
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