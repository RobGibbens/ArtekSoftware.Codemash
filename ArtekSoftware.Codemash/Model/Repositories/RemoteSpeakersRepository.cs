using System.Collections.Generic;
using System.Linq;
using RestSharp;

namespace ArtekSoftware.Codemash
{
	public class RemoteSpeakersRepository
	{
		public IList<Speaker> GetSpeakers ()
		{	
			ITestFlightProxy testFlight = new TestFlightProxy();
			
			List<Speaker> speakers;
			testFlight.PassCheckpoint ("Started RemoteSpeakersRepository.GetSpeakers");
			
			var client = new RestClient ();
			client.BaseUrl = "http://codemash.org";
			
			var request = new RestRequest ();
			request.Resource = "rest/speakers";
			request.RequestFormat = DataFormat.Json;
			using (new NetworkIndicator()) {
				var response = client.Execute<SpeakersList> (request);
			
				speakers = new List<Speaker> ();
				if (response != null && response.Data != null && response.Data.Speakers != null) {
					speakers = response.Data.Speakers.OrderBy (x => x.Name.Trim ()).ToList ();
				}
			}
			testFlight.PassCheckpoint ("Finished RemoteSpeakersRepository.GetSpeakers");
			
			return speakers;
		
		}
	}
}

