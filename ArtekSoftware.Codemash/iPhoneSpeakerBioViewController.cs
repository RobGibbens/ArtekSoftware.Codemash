using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.IO;
using Catnap;
using System.Drawing;

namespace ArtekSoftware.Codemash
{
	public partial class iPhoneSpeakerBioViewController : UIViewController
	{
		private SpeakerEntity _speaker;

		public iPhoneSpeakerBioViewController (SpeakerEntity speaker) : base ("iPhoneSpeakerBioViewController", null)
		{
			_speaker = speaker;
		}
		
		public iPhoneSpeakerBioViewController () : base ("iPhoneSpeakerBioViewController", null)
		{
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			this.scrollView.Frame = new RectangleF(0,0, 320, 460);
    		this.scrollView.ContentSize = new SizeF(320, 650);
			
			this.speakerBlogButton.TouchUpInside += HandleSpeakerBlogButtonhandleTouchUpInside;
			this.speakerTwitterButton.TouchUpInside += HandlespeakerTwitterButtonhandleTouchUpInside;
			
			if (_speaker != null) {
				SetSpeaker ();
			}
		}
		
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}
		
		void HandlespeakerTwitterButtonhandleTouchUpInside (object sender, EventArgs e)
		{
			if (this.speakerTwitterButton != null && this.speakerTwitterButton.TitleLabel != null && !string.IsNullOrWhiteSpace (this.speakerTwitterButton.TitleLabel.Text)) {
				
				if (NetworkStatusCheck.IsReachable ()) {
					var urlString = "http://mobile.twitter.com/" + this.speakerTwitterButton.TitleLabel.Text.Replace ("@", "");
					var url = new NSUrl (urlString);
					UIApplication.SharedApplication.OpenUrl (url);
				}
			}
		}
		
		void HandleSpeakerBlogButtonhandleTouchUpInside (object sender, EventArgs e)
		{
			if (this.speakerBlogButton != null && this.speakerBlogButton.TitleLabel != null && !string.IsNullOrWhiteSpace (this.speakerBlogButton.TitleLabel.Text)) {
				
				if (NetworkStatusCheck.IsReachable ()) {
					var url = new NSUrl (this.speakerBlogButton.TitleLabel.Text);
					UIApplication.SharedApplication.OpenUrl (url);
				}
			}
		}
		
		private void SetSpeaker ()
		{
			this.speakerNameLabel.Text = _speaker.Name;
			this.speakerBioLabel.Text = _speaker.Biography;
			
			HLabel bioLabel;
			if (this.scrollView.Subviews.Count () == 12) {
				bioLabel = new HLabel ();
			} else {
				bioLabel = (HLabel)this.scrollView.Subviews [12];
			}
				
			bioLabel.VerticalAlignment = HLabel.VerticalAlignments.Top;
			bioLabel.Lines = 0;
			bioLabel.LineBreakMode = UILineBreakMode.WordWrap;
			if (!string.IsNullOrWhiteSpace (this.speakerBioLabel.Text)) {
				bioLabel.Text = this.speakerBioLabel.Text;
			} else {
				bioLabel.Text = "No information provided.";
			}
			bioLabel.Font = UIFont.FromName ("STHeitiTC-Light", 14);
			bioLabel.Frame = this.speakerBioLabel.Frame;
			bioLabel.BackgroundColor = UIColor.Clear;
			
			if (this.scrollView.Subviews.Count () == 12) {
			this.scrollView.AddSubview (bioLabel);
				
			} else {
				this.scrollView.Subviews [12] = bioLabel;
			}			
			
			this.speakerBioLabel.Text = string.Empty;
			
			if (!string.IsNullOrWhiteSpace (_speaker.BlogURL)) {
				this.speakerBlogImage.Hidden = false;
				this.speakerBlogButton.SetTitle (_speaker.BlogURL, UIControlState.Normal);
			} else {
				this.speakerBlogImage.Hidden = true;
				this.speakerBlogButton.SetTitle (string.Empty, UIControlState.Normal);
			}
			
			if (!string.IsNullOrWhiteSpace (_speaker.TwitterHandle)) {
				this.speakerTwitterImage.Hidden = false;
				this.speakerTwitterButton.SetTitle (_speaker.TwitterHandle, UIControlState.Normal);
			} else {
				this.speakerTwitterImage.Hidden = true;
				this.speakerTwitterButton.SetTitle (string.Empty, UIControlState.Normal);
			}

			var sessions = GetSessionsForSpeaker (_speaker);
			this.speakerSessionsTable.BackgroundColor = UIColor.Clear;
			this.speakerSessionsTable.ScrollEnabled = false;
			this.speakerSessionsTable.Delegate = new iPhoneSpeakerSessionsDelegate (this, sessions);
			this.speakerSessionsTable.DataSource = new iPhoneSpeakerSessionsDataSource (sessions);
			this.speakerSessionsTable.ReloadData ();
			
			if (!string.IsNullOrEmpty (_speaker.TwitterHandle)) {
				var profileImage = "images/Profiles/" + _speaker.TwitterHandle.Replace ("@", "") + ".png";
			
				if (File.Exists (profileImage)) {
					UIImage image = UIImage.FromFile (profileImage);
					
					using (this.speakerProfileImage.Image) {
						image = Extensions.RemoveSharpEdges (image, Convert.ToInt32 (image.Size.Width), 4);
						this.speakerProfileImage.Image = image;
					}
					
				}
			} else if (File.Exists ("images/Profiles/" + _speaker.Name.Replace (" ", "") + ".png")) {
				var profileImage = "images/Profiles/" + _speaker.Name.Replace (" ", "") + ".png";
				
				UIImage image = UIImage.FromFile (profileImage);
					
				using (this.speakerProfileImage.Image) {
					image = Extensions.RemoveSharpEdges (image, Convert.ToInt32 (image.Size.Width), 4);
					this.speakerProfileImage.Image = image;
				}
			} else {
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
	
	public class iPhoneSpeakerSessionsDataSource : UITableViewDataSource
	{
		private string _section1CellId;
		private List<SessionEntity> _sessions;
		
		public iPhoneSpeakerSessionsDataSource (List<SessionEntity> sessions)
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
				cell.TextLabel.Font = UIFont.FromName ("STHeitiTC-Light", 14);
				cell.BackgroundColor = UIColor.Clear;
			}

			cell.TextLabel.Text = _sessions [indexPath.Row].Title;

			return cell; 
		}
	}

	public class iPhoneSpeakerSessionsDelegate : UITableViewDelegate
	{   	
		private iPhoneSpeakerBioViewController _controller;
		private List<SessionEntity> _sessions;

		public iPhoneSpeakerSessionsDelegate (iPhoneSpeakerBioViewController controller, List<SessionEntity> sessions)
		{
			_controller = controller;
			_sessions = sessions;
		}
		
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			int selectedRow = CalculateSelectedRow (indexPath, tableView);
			SessionEntity session = _sessions.ToList () [selectedRow];
			AppDelegate.CurrentAppDelegate.SetSession (session);
		}
		
		private int CalculateSelectedRow (NSIndexPath indexPath, UITableView tableView)
		{
			int totalCountOfRows = 0;
			int selectedSectionNumber = indexPath.Section;
			
			for (int currentSectionNumber = 0; currentSectionNumber < selectedSectionNumber; ++ currentSectionNumber) {
				totalCountOfRows += tableView.NumberOfRowsInSection (currentSectionNumber);
			}
			
			int selectedRow = totalCountOfRows + indexPath.Row;
			
			return selectedRow;
		}
		
	}
	
	
}

