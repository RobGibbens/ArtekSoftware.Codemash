using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Catnap;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
////using MonoQueue;

namespace ArtekSoftware.Codemash
{
	public partial class SpeakerBioLandscapeViewController : UIViewController
	{
		private SpeakerEntity _speaker;
		private INetworkStatusCheck _networkStatusCheck;
		
		public SpeakerBioLandscapeViewController () : base ("SpeakerBioLandscapeViewController", null)
		{
			Console.WriteLine("SpeakerBioLandscapeViewController.ctor");
			_networkStatusCheck = new NetworkStatusCheck();
		}
		
		public SpeakerBioLandscapeViewController (SpeakerEntity speaker) : base ("SpeakerBioLandscapeViewController", null)
		{
			Console.WriteLine("SpeakerBioLandscapeViewController.ctor - speaker");
			_speaker = speaker;
			_networkStatusCheck = new NetworkStatusCheck();
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
			Console.WriteLine("SpeakerBioLandscapeViewController.ViewDidLoad - 1");
			base.ViewDidLoad ();			
			Console.WriteLine("SpeakerBioLandscapeViewController.ViewDidLoad - 2");
			
			this.speakerBlogButton.TouchUpInside += HandleSpeakerBlogButtonhandleTouchUpInside;
			this.speakerTwitterHandleButton.TouchUpInside += HandleSpeakerTwitterHandleButtonhandleTouchUpInside;
			
			if (this.Speaker != null)
			{
				this.SetSpeaker(this.Speaker);
			}
			Console.WriteLine("SpeakerBioLandscapeViewController.ViewDidLoad - 3");
			
		}
		
	
		public override void ViewDidUnload ()
		{
			Console.WriteLine("SpeakerBioLandscapeViewController.ViewDidUnload - 1");
			
			base.ViewDidUnload ();
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}
		
		void HandleSpeakerTwitterHandleButtonhandleTouchUpInside (object sender, EventArgs e)
		{
			if (this.speakerTwitterHandleButton != null && this.speakerTwitterHandleButton.TitleLabel != null && !string.IsNullOrWhiteSpace (this.speakerTwitterHandleButton.TitleLabel.Text)) {
				
				if (_networkStatusCheck.IsReachable ()) {
					var urlString = "http://mobile.twitter.com/" + this.speakerTwitterHandleButton.TitleLabel.Text.Replace ("@", "");
					var url = new NSUrl (urlString);
					UIApplication.SharedApplication.OpenUrl (url);
				}
			}
		}
		
		void HandleSpeakerBlogButtonhandleTouchUpInside (object sender, EventArgs e)
		{
			if (this.speakerBlogButton != null && this.speakerBlogButton.TitleLabel != null && !string.IsNullOrWhiteSpace (this.speakerBlogButton.TitleLabel.Text)) {
				
				if (_networkStatusCheck.IsReachable ()) {
					var url = new NSUrl (this.speakerBlogButton.TitleLabel.Text);
					UIApplication.SharedApplication.OpenUrl (url);
				}
			}
		}
				
		private void SetSpeaker (SpeakerEntity speaker)
		{
			Console.WriteLine("SpeakerBioLandscapeViewController.SetSpeaker - 1");
			
			this.speakerNameLabel.Text = speaker.Name;
			
			this.speakerBioLabel.Text = "      " + speaker.Biography;
			
			HLabel bioLabel;
			if (this.View.Subviews.Count () <= 10) {
				bioLabel = new HLabel ();
			} else {
				bioLabel = (HLabel)this.View.Subviews [10];
			}
				
			bioLabel.VerticalAlignment = HLabel.VerticalAlignments.Top;
			bioLabel.Lines = 0;
			bioLabel.LineBreakMode = UILineBreakMode.WordWrap;
			if (!string.IsNullOrWhiteSpace (this.speakerBioLabel.Text)) {
				bioLabel.Text = this.speakerBioLabel.Text;
			} else {
				bioLabel.Text = "      No information provided.";
			}
			bioLabel.Font = UIFont.FromName ("STHeitiTC-Light", 14);
			bioLabel.Frame = this.speakerBioLabel.Frame;
			bioLabel.BackgroundColor = UIColor.Clear;
			this.View.AddSubview (bioLabel);
			this.speakerBioLabel.Text = string.Empty;
			
			if (!string.IsNullOrWhiteSpace (speaker.BlogURL)) {
				this.speakerBlogImage.Hidden = false;
				this.speakerBlogButton.SetTitle (speaker.BlogURL, UIControlState.Normal);
			} else {
				this.speakerBlogImage.Hidden = true;
				this.speakerBlogButton.SetTitle (string.Empty, UIControlState.Normal);
			}
			
			if (!string.IsNullOrWhiteSpace (speaker.TwitterHandle)) {
				this.speakerTwitterImage.Hidden = false;
				this.speakerTwitterHandleButton.SetTitle (speaker.TwitterHandle, UIControlState.Normal);
			} else {
				this.speakerTwitterImage.Hidden = true;
				this.speakerTwitterHandleButton.SetTitle (string.Empty, UIControlState.Normal);
			}

			var sessions = GetSessionsForSpeaker (speaker);
			this.speakerSessionsTable.BackgroundColor = UIColor.Clear;
			this.speakerSessionsTable.ScrollEnabled = false;
			this.speakerSessionsTable.Delegate = new SpeakerBioLandscapeDelegate (this, sessions);
			this.speakerSessionsTable.DataSource = new SpeakerBioLandscapeDataSource (sessions);
			this.speakerSessionsTable.ReloadData ();
			
			if (!string.IsNullOrEmpty (speaker.TwitterHandle)) {
				var profileImage = "images/Profiles/" + speaker.TwitterHandle.Replace ("@", "") + ".png";
			
				if (File.Exists (profileImage)) {
					UIImage image = UIImage.FromFile (profileImage);
					
					using (this.speakerProfileImage.Image) {
						this.speakerProfileImage.Image = image;
					}
					
				}
			} else if (File.Exists ("images/Profiles/" + speaker.Name.Replace (" ", "") + ".png")) {
				var profileImage = "images/Profiles/" + speaker.Name.Replace (" ", "") + ".png";
				
				UIImage image = UIImage.FromFile (profileImage);
					
				using (this.speakerProfileImage.Image) {
					this.speakerProfileImage.Image = image;
				}
			} else {
				//var profileImage = "images/glyphicons_003_user@2x.png";
				var profileImage = "images/Profiles/DefaultUser.png";
				
				UIImage image = UIImage.FromFile (profileImage);
					
				using (this.speakerProfileImage.Image) {
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
				cell.TextLabel.Font = UIFont.FromName ("STHeitiTC-Light", 14);
				cell.TextLabel.TextColor = UIColor.Orange;
				cell.BackgroundColor = UIColor.Clear;
			}

			cell.TextLabel.Text = _sessions [indexPath.Row].Title;

			return cell; 
		}
	}

	public class SpeakerBioLandscapeDelegate : UITableViewDelegate
	{   	
		private SpeakerBioLandscapeViewController _controller;
		private List<SessionEntity> _sessions;

		public SpeakerBioLandscapeDelegate (SpeakerBioLandscapeViewController controller, List<SessionEntity> sessions)
		{
			_controller = controller;
			_sessions = sessions;
		}
		
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			int selectedRow = CalculateSelectedRow (indexPath, tableView);
			SessionEntity session = _sessions.ToList () [selectedRow];
			AppDelegate.CurrentAppDelegate.Navigation.SetSession (session);
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