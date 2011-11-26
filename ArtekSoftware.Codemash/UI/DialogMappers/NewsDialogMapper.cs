using System;
using MonoTouch.Dialog;

namespace ArtekSoftware.Codemash
{
	public class NewsDialogMapper
	{
		public RootElement GetNewsDialog (bool isRefresh)
		{
			var root = new RootElement ("News");
			
//			var query = (
//		        from session in sessions
//		        group session by session.Start.DayOfWeek into sessionGroup
//		        select new { 
//							DayOfWeek = sessionGroup.Key, 
//							Sessions = sessionGroup 
//							}
//					).
//		            OrderBy (letter => letter.DayOfWeek);
//
//			foreach (var sessionGroup in query) {
//				var section = new Section (sessionGroup.DayOfWeek.ToString ());
//				
//				foreach (var session in sessionGroup.Sessions) {
//					var element = new SessionEventCell (session);
//					section.Add (element);
//				}				
//				
//				root.Add (section);
//				
//			}
			
			root.UnevenRows = true;
			return root;
			
		}
	}
}

