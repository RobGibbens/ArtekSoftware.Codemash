using System;

namespace ArtekSoftware.Codemash
{
	public class LocalyticsAnalytics : IAnalytics
	{
		private string _localyticsKey = "db527378f483397d685863d-51e8089a-33ee-11e1-a1cc-008545fe83d2";
		
		#region IAnalytics implementation
		public void Initialize ()
		{
			Localytics.LocalyticsSession.SharedLocalyticsSession.StartSession (_localyticsKey);
			Localytics.LocalyticsSession.SharedLocalyticsSession.Open ();
		}

		public void Log (string message)
		{
			Localytics.LocalyticsSession.SharedLocalyticsSession.TagEvent (message);
			Localytics.LocalyticsSession.SharedLocalyticsSession.Upload ();
		}
		
		public void Close ()
		{
			Localytics.LocalyticsSession.SharedLocalyticsSession.Close ();
			Localytics.LocalyticsSession.SharedLocalyticsSession.Upload ();			
		}
		
		public void Resume ()
		{
			Localytics.LocalyticsSession.SharedLocalyticsSession.Resume ();
			Localytics.LocalyticsSession.SharedLocalyticsSession.Upload ();
		}
		#endregion
	}
}