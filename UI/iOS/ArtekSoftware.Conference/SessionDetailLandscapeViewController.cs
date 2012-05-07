using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using Catnap;
//using Catnap.Find;
//using Catnap.Find.Conditions;
using MonoTouch.Dialog.Utilities;
using MonoTouch.Foundation;
using MonoTouch.Twitter;
using MonoTouch.UIKit;
////using MonoQueue;
using ArtekSoftware.Conference.LocalData;

namespace ArtekSoftware.Codemash
{
	public partial class SessionDetailLandscapeViewController : SessionDetailViewControllerBase
	{
		
		public SessionDetailLandscapeViewController (SessionEntity session) : base (session, "SessionDetailLandscapeViewController", null)
		{
		}
		
		public SessionDetailLandscapeViewController () : base ("SessionDetailLandscapeViewController", null)
		{
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}		

//		//TODO : This isn't in iPhone
//		protected void SetTechnologyImage (UIImage image)
//		{
//			using (this.technologyImage.Image) {
//				this.technologyImage.Image = image;
//			}
//		}
		
//		protected void SetAddToScheduleLabel ()
//		{
//			if (IsOnSchedule ()) {
//				//TODO : This isn't on iPhone
//				this.addToScheduleLabel.Frame.Width = 142;
//				this.addToScheduleLabel.Frame.X = 19;
//			} else {
//				//TODO : This isn't on iPhone
//				this.addToScheduleLabel.Frame.Width = 98;
//				this.addToScheduleLabel.Frame.X = 36;
//			}
//		}

		#region implemented abstract members of ArtekSoftware.Codemash.SessionDetailViewControllerBase
		protected override TechnologyImages TechImages {
			get {
					return new TechnologyImages()
					{
						DotNet = "images/Technologies/DotNet.png",
						Ruby = "images/Technologies/Ruby.png",
						Mobile = "images/Technologies/mobile2.png",
						JavaScript = "images/Technologies/JavaScript.png",
						DesignUX = "images/Technologies/DesignUX2.png",
						Java = "images/Technologies/Java.png",
						Windows = "images/Technologies/Windows.png",
						OtherLanguages = "images/Technologies/OtherLanguages2.png",
						SoftwareProcess = "images/Technologies/SoftwareProcess4.png",
						Other = "images/Technologies/Other2.png",
					};
			}
		}
		
		protected override UIPopoverController PopoverController {
			get {
				//TODO : Probably shouldn't be null
				return null;
			}
		}
		
		protected override int NumberOfControls {
			get {
				return 18;
			}
		}		
		
		protected override MonoTouch.UIKit.UIScrollView ScrollView {
			get {
				return this.scrollView;
			}
		}

		protected override UIImageView TechnologyImage {
			get {
				return this.technologyImage;
			}
		}

		protected override UIButton BtnAddToSchedule {
			get {
				return this.btnAddToSchedule;
			}
		}

		protected override UIButton BtnTweetThis {
			get {
				return this.btnTweetThis;
			}
		}

		protected override MonoTouch.UIKit.UILabel SessionTitleLabel {
			get {
				return sessionTitleLabel;
			}
		}

		protected override MonoTouch.UIKit.UILabel SessionStartLabel {
			get {
				return this.sessionStartLabel;
			}
		}

		protected override MonoTouch.UIKit.UILabel SessionRoomLabel {
			get {
				return this.sessionRoomLabel;
			}
		}

		protected override UIButton SessionSpeakerNameButton {
			get {
				return this.sessionSpeakerNameButton;
			}
		}

		protected override MonoTouch.UIKit.UILabel SessionTechnologyLabel {
			get {
				return this.sessionTechnologyLabel;
			}
		}

		protected override MonoTouch.UIKit.UILabel SessionDifficultyLabel {
			get {
				return this.sessionDifficultyLabel;
			}
		}

		protected override MonoTouch.UIKit.UILabel TweetThisLabel {
			get {
				return this.tweetThisLabel;
			}
		}

		protected override MonoTouch.UIKit.UILabel SessionAbstractLabel {
			get {
				return sessionAbstractLabel;
			}
		}

		protected override MonoTouch.UIKit.UILabel AddToScheduleLabel {
			get {
				return this.addToScheduleLabel;
			}
		}
		#endregion		

	}
}

