using System;
using System.Collections.Generic;
using RestSharp;
using System.Linq;

namespace ArtekSoftware.Codemash
{
	public class RemoteSpeakersRepository
	{
		public IList<Speaker> GetSpeakers ()
		{	
			List<Speaker> speakers;
			//TestFlightSdk.TestFlight.PassCheckpoint ("Started RemoteSpeakersRepository.GetSpeakers");
			
			var client = new RestClient ();
			client.BaseUrl = "http://codemash.org";
			
			var request = new RestRequest ();
			request.Resource = "rest/speakers";
			request.RequestFormat = DataFormat.Xml;
			using (new NetworkIndicator()) {
				var response = client.Execute<SpeakersList> (request);
			
				speakers = new List<Speaker> ();
				if (response != null && response.Data != null && response.Data.Speakers != null) {
					speakers = response.Data.Speakers.OrderBy (x => x.Name.Trim ()).ToList ();
				}
			}
			//TestFlightSdk.TestFlight.PassCheckpoint ("Finished RemoteSpeakersRepository.GetSpeakers");
			
			return speakers;
		
		}
	}
}

