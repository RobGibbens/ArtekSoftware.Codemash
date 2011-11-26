using System;
using MonoTouch.UIKit;

namespace ArtekSoftware.Codemash
{
	public class RotatingSpeakerBioViewController : RotatingViewController
	{
		private SpeakerEntity _speaker;
		
		public RotatingSpeakerBioViewController (SpeakerEntity speaker) : this()
		{
			_speaker = speaker;
			LoadSpeaker ();
		}
		
		public RotatingSpeakerBioViewController ()
		{
			LoadSpeaker();
		}
		
		public void SetSpeaker (SpeakerEntity speaker)
		{
			_speaker = speaker;
			LoadSpeaker ();
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			if (_speaker != null) {
				LoadSpeaker ();
			}
		}

		private void LoadSpeaker ()
		{
			if (_speaker != null) {
				this.PortraitViewController = new SpeakerBioViewController (_speaker);			
				this.LandscapeLeftViewController = new SpeakerBioLandscapeViewController (_speaker);
				this.LandscapeRightViewController = new SpeakerBioLandscapeViewController (_speaker);
			} else {
				this.PortraitViewController = new SpeakerBioViewController ();			
				this.LandscapeLeftViewController = new SpeakerBioLandscapeViewController ();
				this.LandscapeRightViewController = new SpeakerBioLandscapeViewController ();
			}
		}
	}
}