using MonoTouch.UIKit;
using ArtekSoftware.Conference.LocalData;

namespace ArtekSoftware.Codemash
{
	public partial class iPhoneSessionDetailViewController : SessionDetailViewControllerBase
	{
		public iPhoneSessionDetailViewController (SessionEntity session) : base (session, "iPhoneSessionDetailViewController", null)
		{
		}
		
		public iPhoneSessionDetailViewController () : base ("iPhoneSessionDetailViewController", null)
		{
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return false;
		}
		
		#region implemented abstract members of ArtekSoftware.Codemash.SessionDetailViewControllerBase
		protected override TechnologyImages TechImages {
			get {
					return new TechnologyImages()
					{
						DotNet = "images/Technologies/DotNetSmall.png",
						Ruby = "images/Technologies/RubySmall.png",
						Mobile = "images/Technologies/mobile2Small.png",
						JavaScript = "images/Technologies/JavaScriptSmall.png",
						DesignUX = "images/Technologies/DesignUX2Small.png",
						Java = "images/Technologies/JavaSmall.png",
						Windows = "images/Technologies/WindowsSmall.png",
						OtherLanguages = "images/Technologies/OtherLanguages2Small.png",
						SoftwareProcess = "images/Technologies/SoftwareProcess4Small.png",
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
				return 21;
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