using System;
using System.Collections.Generic;
using RestSharp;
using System.Linq;
//using MonoTouch.TestFlight;

namespace ArtekSoftware.Codemash
{
	public class RemoteSpeakersRepository
	{
		public IList<Speaker> GetSpeakers ()
		{	
			List<Speaker> speakers = null;
			//TestFlight.PassCheckpoint ("Started RemoteSpeakersRepository.GetSpeakers");
			
			var client = new RestClient ();
			//client.BaseUrl = "http://codemash.org";
			client.BaseUrl = "http://rest.codemash.org";
			
			var request = new RestRequest ();
			//request.Resource = "rest/speakers";
			request.Resource = "api/speakers.json";
			
			//request.RequestFormat = DataFormat.Xml;
			request.RequestFormat = DataFormat.Json;
			
			using (new NetworkIndicator()) {
				//var response = client.Execute<SpeakersList> (request);
				var response = client.Execute<List<Speaker>> (request);
				var c = response.Content;
				var c2 = response.ErrorMessage;
				var c3 = response.ErrorException;

				if (response != null && response.Data != null) {
					speakers = response.Data as List<Speaker>;
					speakers = speakers.OrderBy (x => x.Name.Trim ()).ToList ();
				}
			}
			//TestFlight.PassCheckpoint ("Finished RemoteSpeakersRepository.GetSpeakers");
			
			return speakers;
		
		}
	}
}

