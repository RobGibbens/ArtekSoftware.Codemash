using System;
using System.Collections.Generic;
using RestSharp;
using System.Linq;
using System.Net;
using System.IO;
using MonoTouch.UIKit;

namespace ArtekSoftware.Codemash
{
	public class RemoteTwitterRepository
	{
		public List<string> GetAllTwitterImageUrls (List<string> twitterHandles)
		{
			foreach (var name in twitterHandles) {
				if (!string.IsNullOrWhiteSpace (name)) {
					var twitterName = name.Replace ("@", "");

					string profileImageUrl = "http://api.twitter.com/1/users/profile_image?screen_name=" + twitterName + "&size=bigger";
					string filename = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), twitterName + ".png");
					using (new NetworkIndicator()) {
						WebClient webClient = new WebClient ();
						webClient.DownloadFile (profileImageUrl, filename);
					}
				}
			}
			
			return null;
		}
	}
}

