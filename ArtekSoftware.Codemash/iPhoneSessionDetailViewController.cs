using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Catnap;
using Catnap.Find;
using Catnap.Find.Conditions;
using MonoTouch.Dialog.Utilities;
using MonoTouch.Foundation;
using MonoTouch.Twitter;
using MonoTouch.UIKit;

namespace ArtekSoftware.Codemash
{
	public partial class iPhoneSessionDetailViewController : UIViewController
	{
		private SessionEntity _session;

		public iPhoneSessionDetailViewController (SessionEntity session) : base ("iPhoneSessionDetailViewController", null)
		{
			_session = session;
		}
		
		public iPhoneSessionDetailViewController () : base ("iPhoneSessionDetailViewController", null)
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
    		this.scrollView.ContentSize = new SizeF(320, 1050);
			
			var version = Convert.ToDecimal (UIDevice.CurrentDevice.SystemVersion.Split ('.').First ());
			
			if (version < 5) {
				this.btnTweetThis.Hidden = true;
			} else {
				this.btnTweetThis.Hidden = false;
			}
			
			this.btnAddToSchedule.TouchUpInside += HandleSessionAddToScheduleButtonhandleTouchUpInside;
			this.btnTweetThis.TouchUpInside += HandleTweetThisButtonHandleTouchUpInside;
			this.sessionSpeakerNameButton.TouchUpInside += HandleSessionSpeakerNameLabelTouchUpInside;
			
			if (this.Session != null) {
				this.SetSession (this.Session);
			}
		}
		
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return false;
		}
		
		public void SetSession (SessionEntity session)
		{
			this.Session = session;
			
			this.sessionRoomLabel.Text = session.Room;
			this.sessionSpeakerNameButton.SetTitle (session.SpeakerName, UIControlState.Normal);
			if (session.StartDate == DateTime.MinValue) {
				this.sessionStartLabel.Text = "No date/time - Please Refresh";
			} else {
				this.sessionStartLabel.Text = session.StartDate.DayOfWeek.ToString() + " " + session.StartDate.ToString ("h:mm tt");
			}
			this.sessionTechnologyLabel.Text = session.Technology + " / " + session.Difficulty;
			
			SetImageUrl ();
			
			this.sessionTitleLabel.Text = session.Title;
			
			HLabel titleLabel;
			if (this.scrollView.Subviews.Count () <= 21) {
				titleLabel = new HLabel ();
			} else {
				titleLabel = (HLabel)this.scrollView.Subviews [21];
			}
			titleLabel.VerticalAlignment = HLabel.VerticalAlignments.Top;
			titleLabel.Lines = 0;
			titleLabel.LineBreakMode = UILineBreakMode.WordWrap;
			titleLabel.Text = this.sessionTitleLabel.Text;
			titleLabel.Font = this.sessionTitleLabel.Font;
			titleLabel.Frame = this.sessionTitleLabel.Frame;
			titleLabel.BackgroundColor = UIColor.Clear;
			
			if (this.scrollView.Subviews.Count () <= 21) {
				this.scrollView.AddSubview (titleLabel);
			} else {
				this.scrollView.Subviews [21] = titleLabel;
			}
			
			this.sessionTitleLabel.Text = string.Empty;	
			
			
			HLabel abstractLabel;
			if (this.scrollView.Subviews.Count () <= 22) {
				abstractLabel = new HLabel ();
				//this.View.AddSubview (abstractLabel);
			} else {
				abstractLabel = (HLabel)this.scrollView.Subviews [22];
				//abstractLabel = (HLabel)this.View.Subviews[20]; 
			}
			abstractLabel.VerticalAlignment = HLabel.VerticalAlignments.Top;
			abstractLabel.Lines = 0;
			abstractLabel.LineBreakMode = UILineBreakMode.WordWrap;
			abstractLabel.Text = session.Abstract;
			abstractLabel.Font = UIFont.FromName ("STHeitiTC-Light", 14);
			abstractLabel.Frame = this.sessionAbstractLabel.Frame;
			abstractLabel.BackgroundColor = UIColor.Clear;
			
			if (this.scrollView.Subviews.Count () <= 22) {
				this.scrollView.AddSubview (abstractLabel);
			} else {
				this.scrollView.Subviews [22] = abstractLabel;
			}			
			this.sessionAbstractLabel.Text = string.Empty;			
			
			SetAddToScheduleLabel ();
			
			SetImageUrl();
			SetLocalImage (ImageUrl);
			
			this.View.SetNeedsLayout ();
			this.View.LayoutIfNeeded ();
				
		}
		
		protected void SetAddToScheduleLabel ()
		{
			if (IsOnSchedule ()) {
				this.addToScheduleLabel.Text = "Remove from schedule";
				this.btnAddToSchedule.SetImage(UIImage.FromFile("images/FavoritedSession.png"), UIControlState.Normal);
			} else {
				this.addToScheduleLabel.Text = "Add to schedule";
				this.btnAddToSchedule.SetImage(UIImage.FromFile("images/FavoriteSession.png"), UIControlState.Normal);
			}
		}
		
		protected bool IsOnSchedule ()
		{
			IEnumerable<ScheduledSessionEntity> sessions = null;
			
			if (UnitOfWork.IsUnitOfWorkStarted ()) {
				var repo = new LocalScheduledSessionsRepository ();
				var criteria = new Criteria ();
				criteria.Add (Condition.Equal<ScheduledSessionEntity> (x => x.Title, _session.Title));
				sessions = repo.Find (criteria);			
			} else {
				using (UnitOfWork.Start()) {
					var repo = new LocalScheduledSessionsRepository ();
					var criteria = new Criteria ();
					criteria.Add (Condition.Equal<ScheduledSessionEntity> (x => x.Title, _session.Title));
					sessions = repo.Find (criteria);
				}
			}
			
			if (sessions != null && sessions.Count () > 0) {
				return true;
			} else {
				return false;
			}
				
		}		
		
		protected SessionEntity Session {
			get {
				return _session;
			}
			set {
				_session = value;
			}
		}
		
		protected void HandleSessionAddToScheduleButtonhandleTouchUpInside (object sender, EventArgs e)
		{
			AppDelegate.CurrentAppDelegate.TabBar.SelectedIndex = 1;
			
			if (!IsOnSchedule ()) {
				using (UnitOfWork.Start()) {
					var repository = new LocalScheduledSessionsRepository ();
					var scheduledSession = repository.GetScheduledSession (_session.URI);
				
					if (scheduledSession == null) {
						scheduledSession = new ScheduledSessionEntity ()
											{
												Abstract = _session.Abstract,
												Difficulty = _session.Difficulty,
												Room = _session.Room,
												SpeakerName = _session.SpeakerName,
												SpeakerURI = _session.SpeakerURI,
												Start = _session.Start,
												Technology = _session.Technology,
												Title = _session.Title,
												URI = _session.URI,
											};
					
						repository.Save (scheduledSession);
						var vc = AppDelegate.CurrentAppDelegate.TabBar.ViewControllers [0];
						var uinc = (UINavigationController)vc;
						var scheduleController = (ScheduledSessionDialogViewController)uinc.TopViewController;
						scheduleController.LoadData ();
						AddNotification (_session);
					}
				}
			} else {
				
				using (UnitOfWork.Start()) {
					var repository = new LocalScheduledSessionsRepository ();
					var scheduledSession = repository.GetScheduledSession (_session.URI);
					repository.Delete (scheduledSession.Id);
					
					var vc = AppDelegate.CurrentAppDelegate.TabBar.ViewControllers [0];
					var uinc = (UINavigationController)vc;
					var scheduleController = (ScheduledSessionDialogViewController)uinc.TopViewController;
					scheduleController.LoadData ();
				}
				RemoveNotification (_session);
			}
			
			SetAddToScheduleLabel();
		}
		
		protected void HandleSessionSpeakerNameLabelTouchUpInside (object sender, EventArgs e)
		{
			SpeakerEntity speaker;
			if (UnitOfWork.IsUnitOfWorkStarted ()) {
				var localRepository = new LocalSpeakersRepository ();
				speaker = localRepository.FindByName (_session.SpeakerName);
			} else {
				using (UnitOfWork.Start()) {
					var localRepository = new LocalSpeakersRepository ();
					speaker = localRepository.FindByName (_session.SpeakerName);	
				}
			}
			
			AppDelegate.CurrentAppDelegate.SetSpeaker (speaker);
		}

		protected void AddNotification (SessionEntity session)
		{	
			if (session != null && session.StartDate != DateTime.MinValue) {
				UILocalNotification notification = new UILocalNotification{
				  FireDate = session.StartDate.AddMinutes (-10),
				  TimeZone = NSTimeZone.LocalTimeZone,
				  AlertBody = session.Title + " will start in 10 minutes in " + session.Room,
				  RepeatInterval = 0
				};
				UIApplication.SharedApplication.ScheduleLocalNotification (notification);
			}
		}
		
		protected void RemoveNotification (SessionEntity session)
		{	
			var allNotifications = UIApplication.SharedApplication.ScheduledLocalNotifications.ToList ();
			var sessionNotification = allNotifications.Where (x => x.AlertBody.StartsWith (session.Title)).SingleOrDefault ();
			if (sessionNotification != null) {
				UIApplication.SharedApplication.CancelLocalNotification (sessionNotification);
			}
		}
		
		protected void HandleTweetThisButtonHandleTouchUpInside (object sender, EventArgs e)
		{
			var version = Convert.ToDecimal (UIDevice.CurrentDevice.SystemVersion.Split ('.').First ());
			if (version >= (decimal)5.0) {
				//TODO: Add Reachability
				if (TWTweetComposeViewController.CanSendTweet) {
					var tvc = new TWTweetComposeViewController ();
					tvc.SetInitialText ("I'm attending " + _session.Title + " at #CodeMash");
					tvc.SetCompletionHandler ((MonoTouch.Twitter.TWTweetComposeViewControllerResult r) => {
						this.DismissModalViewControllerAnimated (true);
						if (r == MonoTouch.Twitter.TWTweetComposeViewControllerResult.Cancelled) {
							//display.Text = "Cancelled~";
						} else {
							//display.Text = "Sent!";
						}
					});
					this.PresentModalViewController (tvc, true);
				} else {
					ModalDialog.Alert ("Unable to tweet", "Twitter credentials must be entered in Settings before tweeting.");
				}
			}
		}
		

		string ImageUrl;
		void SetImageUrl ()
		{
			if (this.Session.Technology.ToLower () == ".net") {
				ImageUrl = "images/Technologies/DotNetSmall.png";
			} else if (this.Session.Technology.ToLower () == "ruby") {
				ImageUrl = "images/Technologies/RubySmall.png";
			} else if (this.Session.Technology.ToLower () == "mobile") {
				ImageUrl = "images/Technologies/mobile2Small.png";
			} else if (this.Session.Technology.ToLower () == "javascript") {
				ImageUrl = "images/Technologies/JavaScriptSmall.png";
			} else if (this.Session.Technology.ToLower () == "design/ux") {
				ImageUrl = "images/Technologies/DesignUX2Small.png";
			} else if (this.Session.Technology.ToLower () == "java") {
				ImageUrl = "images/Technologies/JavaSmall.png";
			} else if (this.Session.Technology.ToLower () == "windows 8") {
				ImageUrl = "images/Technologies/WindowsSmall.png";
			} else if (this.Session.Technology.ToLower () == "other languages") {
				ImageUrl = "images/Technologies/OtherLanguages2Small.png";
			} else if (this.Session.Technology.ToLower () == "software process") {
				ImageUrl = "images/Technologies/SoftwareProcess4Small.png";
			} else {
				ImageUrl = "images/Technologies/Other2.png";
			}
		}	
		string imgurl;
		public void SetLocalImage (string url)
		{
			if (!string.IsNullOrEmpty (url)) {
				this.imgurl = url;
				var imageBackground = new Uri ("file://" + Path.GetFullPath (url));
				var image = ImageLoader.DefaultRequestImage (imageBackground, null);
				
				using (this.technologyImage.Image) {
					this.technologyImage.Image = image;
				}
				
			}
		}
		
	}
}

