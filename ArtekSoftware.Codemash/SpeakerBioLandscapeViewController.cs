using MonoTouch.UIKit;
using System.Drawing;
using System;
using MonoTouch.Foundation;
using System.Linq;
using System.Collections.Generic;
using Catnap;
using MonoTouch.Twitter;
using Catnap.Find.Conditions;
using Catnap.Find;
using System.IO;

namespace ArtekSoftware.Codemash
{
	public partial class SpeakerBioLandscapeViewController : UIViewController
	{
		private SpeakerEntity _speaker;

		public SpeakerBioLandscapeViewController () : base ("SpeakerBioLandscapeViewController", null)
		{
		}
		
		public SpeakerBioLandscapeViewController (SpeakerEntity speaker) : base ("SpeakerBioLandscapeViewController", null)
		{
			_speaker = speaker;
		}		
		
		protected SpeakerEntity Speaker {
			get {
				return _speaker;
			}
			set {
				_speaker = value;
			}
		}		
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();			
			
			this.speakerBlogButton.TouchUpInside += HandleSpeakerBlogButtonhandleTouchUpInside;
			this.speakerTwitterHandleButton.TouchUpInside += HandleSpeakerTwitterHandleButtonhandleTouchUpInside;
			
			if (this.Speaker != null)
			{
				this.SetSpeaker(this.Speaker);
			}
		}
		
	
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}
		
		void HandleSpeakerTwitterHandleButtonhandleTouchUpInside (object sender, EventArgs e)
		{
			if (this.speakerTwitterHandleButton != null && this.speakerTwitterHandleButton.TitleLabel != null && !string.IsNullOrWhiteSpace (this.speakerTwitterHandleButton.TitleLabel.Text)) {
				var networkStatusCheck = new NetworkStatusCheck ();
				if (networkStatusCheck.IsOnline ()) {
					var urlString = "http://mobile.twitter.com/" + this.speakerTwitterHandleButton.TitleLabel.Text.Replace ("@", "");
					var url = new NSUrl (urlString);
					UIApplication.SharedApplication.OpenUrl (url);
				}
			}
		}
		
		void HandleSpeakerBlogButtonhandleTouchUpInside (object sender, EventArgs e)
		{
			if (this.speakerBlogButton != null && this.speakerBlogButton.TitleLabel != null && !string.IsNullOrWhiteSpace (this.speakerBlogButton.TitleLabel.Text)) {
				var networkStatusCheck = new NetworkStatusCheck ();
				if (networkStatusCheck.IsOnline ()) {
					var url = new NSUrl (this.speakerBlogButton.TitleLabel.Text);
					UIApplication.SharedApplication.OpenUrl (url);
				}
			}
		}
				
		
		private void SetSpeaker (SpeakerEntity speaker)
		{
			
			this.speakerNameLabel.Text = speaker.Name;
			this.speakerBioLabel.Text = "      " + speaker.Biography;
			
			if (!string.IsNullOrWhiteSpace (speaker.BlogURL)) {
				this.speakerBlogButton.SetTitle (speaker.BlogURL, UIControlState.Normal);
			} else {
				this.speakerBlogButton.SetTitle (string.Empty, UIControlState.Normal);
			}
			
			if (!string.IsNullOrWhiteSpace (speaker.TwitterHandle)) {
				this.speakerTwitterHandleButton.SetTitle (speaker.TwitterHandle, UIControlState.Normal);
			} else {
				this.speakerTwitterHandleButton.SetTitle (string.Empty, UIControlState.Normal);
			}

			var sessions = GetSessionsForSpeaker (speaker);
			this.speakerSessionsTable.Delegate = new SpeakerBioLandscapeDelegate (this);
			this.speakerSessionsTable.DataSource = new SpeakerBioLandscapeDataSource (sessions);
			this.speakerSessionsTable.ReloadData ();
			
			if (!string.IsNullOrEmpty (speaker.TwitterHandle)) {
				var profileImage = "images/Profiles/" + speaker.TwitterHandle.Replace ("@", "") + ".png";
			
				if (File.Exists (profileImage)) {
					UIImage image = UIImage.FromFile (profileImage);
					using (this.speakerProfileImage.Image) {
						image = Extensions.RemoveSharpEdges (image, Convert.ToInt32 (image.Size.Width), 4);
						this.speakerProfileImage.Image = image;
					}
				}
			} else {
				//var profileImage = "images/glyphicons_003_user@2x.png";
				var profileImage = "images/Profiles/DefaultUser.png";
				
				UIImage image = UIImage.FromFile (profileImage);
				using (this.speakerProfileImage.Image) {
					image = Extensions.RemoveSharpEdges (image, Convert.ToInt32 (image.Size.Width), 4);
					this.speakerProfileImage.Image = image;
				}
				
			}
		}
		
		private List<SessionEntity> GetSessionsForSpeaker (SpeakerEntity speaker)
		{
			List<SessionEntity> sessions = null;
			
			if (UnitOfWork.IsUnitOfWorkStarted ()) {
				var repo = new LocalSessionsRepository ();
				sessions = repo.GetForSpeaker (speaker.SpeakerURI);
			} else {
				using (UnitOfWork.Start()) {
					var repo = new LocalSessionsRepository ();
					sessions = repo.GetForSpeaker (speaker.SpeakerURI);
				}
			}
			
			return sessions;
		}		
		
		
		
	}
	
	public class SpeakerBioLandscapeDataSource : UITableViewDataSource
	{
		private string _section1CellId;
		private List<SessionEntity> _sessions;
		
		public SpeakerBioLandscapeDataSource (List<SessionEntity> sessions)
		{
			_sessions = sessions;
			_section1CellId = "cellid";

		}

		public override string TitleForHeader (UITableView tableView, int section)
		{
			return "";
		}

		public override int RowsInSection (UITableView tableview, int section)
		{
			return _sessions.Count;
		}
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			// For more information on why this is necessary, see the Apple docs
			var row = indexPath.Row;
			UITableViewCell cell = tableView.DequeueReusableCell (_section1CellId); 

			if (cell == null) {
				// See the styles demo for different UITableViewCellAccessory
				cell = new UITableViewCell (UITableViewCellStyle.Default, _section1CellId);
				cell.Accessory = UITableViewCellAccessory.None;
			}

			cell.TextLabel.Text = _sessions [indexPath.Row].Title;

			return cell; 
		}
	}

	public class SpeakerBioLandscapeDelegate : UITableViewDelegate
	{   	
		private SpeakerBioLandscapeViewController _controller;

		public SpeakerBioLandscapeDelegate (SpeakerBioLandscapeViewController controller)
		{
			_controller = controller;	
		}
		
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
//			UITableViewController nextController = null;
//
//			switch (indexPath.Row) {
//			case 0:
//				nextController = new CheckmarkDemoTableController (UITableViewStyle.Grouped);
//				break;
//			case 1:
//				nextController = new StyleDemoTableController (UITableViewStyle.Grouped);
//				break;
//			case 2:
//				nextController = new EditableTableController (UITableViewStyle.Plain);
//				break;
//			default:
//				break;
//			}
//
//			if (nextController != null)
//				_controller.NavigationController.PushViewController (nextController, true);
		}
	}

	
}
