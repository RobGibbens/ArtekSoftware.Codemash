using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using Catnap;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
////using MonoQueue;
using ArtekSoftware.Conference;
using ArtekSoftware.Conference.LocalData;
using RestSharp;
using ArtekSoftware.Conference.Data;

namespace ArtekSoftware.Codemash
{
	public partial class SpeakerBioViewController : UIViewController
	{
		private SpeakerEntity _speaker;
		private INetworkStatusCheck _networkStatusCheck;
		
		public SpeakerBioViewController (SpeakerEntity speaker) : base ("SpeakerBioViewController", null)
		{
			_speaker = speaker;
			//_networkStatusCheck = new NetworkStatusCheck();
		}
		
		public UIToolbar Toolbar {
			get {
				return toolbar;	
			}
		}
		
		public SpeakerBioViewController () : base ("SpeakerBioViewController", null)
		{
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			this.speakerBlogButton.TouchUpInside += HandleSpeakerBlogButtonhandleTouchUpInside;
			this.speakerTwitterHandleButton.TouchUpInside += HandleSpeakerTwitterHandleButtonhandleTouchUpInside;
			
			if (_speaker != null) {
				SetSpeaker ();
			}
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
		
		private void SetSpeaker ()
		{
			this.speakerNameLabel.Text = _speaker.Name;
			
			this.speakerBioLabel.Text = "      " + _speaker.Biography;
			
			HLabel bioLabel;
			if (this.View.Subviews.Count () == 10) {
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
			
			if (!string.IsNullOrWhiteSpace (_speaker.BlogUrl)) {
				this.speakerBlogImage.Hidden = false;
				this.speakerBlogButton.SetTitle (_speaker.BlogUrl, UIControlState.Normal);
			} else {
				this.speakerBlogImage.Hidden = true;
				this.speakerBlogButton.SetTitle (string.Empty, UIControlState.Normal);
			}
			
			if (!string.IsNullOrWhiteSpace (_speaker.TwitterName)) {
				this.speakerTwitterImage.Hidden = false;
				this.speakerTwitterHandleButton.SetTitle (_speaker.TwitterName, UIControlState.Normal);
			} else {
				this.speakerTwitterImage.Hidden = true;
				this.speakerTwitterHandleButton.SetTitle (string.Empty, UIControlState.Normal);
			}

			var sessions = GetSessionsForSpeaker (_speaker);
			this.speakerSessionsTable.BackgroundColor = UIColor.Clear;
			this.speakerSessionsTable.ScrollEnabled = false;
			this.speakerSessionsTable.Delegate = new MainTableDelegate (this, sessions);
			this.speakerSessionsTable.DataSource = new MainTableDataSource (sessions);
			this.speakerSessionsTable.ReloadData ();
			
			if (!string.IsNullOrEmpty (_speaker.TwitterName)) {
				var profileImage = "images/Profiles/" + _speaker.TwitterName.Replace ("@", "") + ".png";
			
				if (File.Exists (profileImage)) {
					UIImage image = UIImage.FromFile (profileImage);
					
					using (this.speakerProfileImage.Image) {
						this.speakerProfileImage.Image = image;
					}
					
				}
			} else if (File.Exists ("images/Profiles/" + _speaker.Name.Replace (" ", "") + ".png")) {
				var profileImage = "images/Profiles/" + _speaker.Name.Replace (" ", "") + ".png";
				
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
		
		private IEnumerable<SessionEntity> GetSessionsForSpeaker (SpeakerEntity speaker)
		{
			var testFlightProxy = new TestFlightProxy();
			var restClient = new RestClient();
			var remoteConfiguration = new RemoteConfiguration();
			var remoteRepository = new RemoteSessionsRepository(testFlightProxy, restClient, remoteConfiguration);
			var networkStatusCheck = new NetworkStatusCheck();
			
			string conferenceSlug = AppDelegate.ConferenceSlug;
			var repo = new SessionRepository(remoteRepository, networkStatusCheck, testFlightProxy, restClient, remoteConfiguration, conferenceSlug);
			var entities = repo.GetEntities(isRefresh:false);
			//TODO: Filter by speaker
			
			return entities;
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}
	}
	
	public class MainTableDataSource : UITableViewDataSource
	{
		private string _section1CellId;
		private List<SessionEntity> _sessions;
		
		public MainTableDataSource (IEnumerable<SessionEntity> sessions)
		{
			_sessions = sessions.ToList();
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

	public class MainTableDelegate : UITableViewDelegate
	{   	
		private SpeakerBioViewController _controller;
		private List<SessionEntity> _sessions;

		public MainTableDelegate (SpeakerBioViewController controller, IEnumerable<SessionEntity> sessions)
		{
			_controller = controller;
			_sessions = sessions.ToList();
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