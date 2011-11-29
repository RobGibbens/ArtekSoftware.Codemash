using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Twitter;
using Catnap;
using Catnap.Find.Conditions;
using Catnap.Find;

namespace ArtekSoftware.Codemash
{
	public partial class SessionDetailViewController : UIViewController
	{
		
		public UIToolbar Toolbar {
			get {
				return toolbar;	
			}
		}

		public SessionDetailViewController (SessionEntity session) : base ("SessionDetailViewController", null)
		{
			_session = session;
		}
		
		public SessionDetailViewController () : base ("SessionDetailViewController", null)
		{
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			var version = Convert.ToDecimal (UIDevice.CurrentDevice.SystemVersion.Split ('.').First ());
			
			if (version < 5) {
				this.btnTweetThis.Hidden = true;
			}
			else
			{
				this.btnTweetThis.Hidden = false;
			}
			
			this.addToScheduleImage.TouchUpInside += HandleSessionAddToScheduleButtonhandleTouchUpInside;
			this.btnTweetThis.TouchUpInside += HandleTweetThisButtonHandleTouchUpInside;
			this.sessionSpeakerNameLabel.TouchUpInside += HandleSessionSpeakerNameLabelTouchUpInside;
			
			if (this.Session != null) {
				this.SetSession (this.Session);
			}
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}

		
		public void SetSession (SessionEntity session)
		{
			this.Session = session;
			
			//this.sessionAbstractLabel = abstractLabel;
			//_sessionAbstractLabel.Text = session.Abstract;
			
			this.sessionDifficultyLabel.Text = session.Difficulty;
			this.sessionRoomLabel.Text = session.Room;
			this.sessionSpeakerNameLabel.SetTitle (session.SpeakerName, UIControlState.Normal);
			this.sessionStartLabel.Text = session.Start.ToString ();
			this.sessionTechnologyLabel.Text = session.Technology;
			
			SetImageUrl ();
			//_sessionTechnologyLabel.Text = GetTechnologyName (session.Technology);
			
			this.sessionTitleLabel.Text = session.Title;
			
			HLabel titleLabel;
			if (this.View.Subviews.Count () <= 19) {
				titleLabel = new HLabel ();
			} else {
				titleLabel = (HLabel)this.View.Subviews [19];
			}
			titleLabel.VerticalAlignment = HLabel.VerticalAlignments.Top;
			titleLabel.Lines = 0;
			titleLabel.LineBreakMode = UILineBreakMode.WordWrap;
			titleLabel.Text = this.sessionTitleLabel.Text;
			titleLabel.Font = this.sessionTitleLabel.Font;
			titleLabel.Frame = this.sessionTitleLabel.Frame;
			titleLabel.BackgroundColor = UIColor.Clear;
			
			var countBeforeTitle = this.View.Subviews.Count ();
			if (this.View.Subviews.Count () <= 19) {
				this.View.AddSubview (titleLabel);
			} else {
				this.View.Subviews [19] = titleLabel;
			}
			
			this.sessionTitleLabel.Text = string.Empty;	
			
			
			HLabel abstractLabel;
			if (this.View.Subviews.Count () <= 20) {
				abstractLabel = new HLabel ();
				//this.View.AddSubview (abstractLabel);
			} else {
				abstractLabel = (HLabel)this.View.Subviews [20];
				//abstractLabel = (HLabel)this.View.Subviews[20]; 
			}
			abstractLabel.VerticalAlignment = HLabel.VerticalAlignments.Top;
			abstractLabel.Lines = 0;
			abstractLabel.LineBreakMode = UILineBreakMode.WordWrap;
			abstractLabel.Text = session.Abstract;
			abstractLabel.Font = UIFont.FromName ("STHeitiTC-Light", 16);
			abstractLabel.Frame = this.sessionAbstractLabel.Frame;
			abstractLabel.BackgroundColor = UIColor.Clear;
			
			var countBeforeAbstract = this.View.Subviews.Count ();
			if (this.View.Subviews.Count () <= 20) {
				this.View.AddSubview (abstractLabel);
			} else {
				this.View.Subviews [20] = abstractLabel;
			}			
			this.sessionAbstractLabel.Text = string.Empty;			
			this.View.SetNeedsLayout ();
			this.View.LayoutIfNeeded ();
			//_sessionTrackLabel.Text = session.Track;
			
			SetAddToScheduleLabel ();
			
			//AddPopoverButton(null, null, toolbar.Items[0], new UIPopoverController(new TabBarController()));
//			if (this.popoverController != null) {
//				this.popoverController.Dismiss (true);
//			}				
		}
		
		protected void SetTechnologyImage (UIImage image)
		{
			using (this.technologyImage.Image) {
				this.technologyImage.Image = image;
			}
		}
		
		protected void SetAddToScheduleLabel ()
		{
			if (IsOnSchedule ()) {
				this.addToScheduleLabel.Text = "Remove from schedule";
				this.addToScheduleLabel.Frame.Width = 142;
				this.addToScheduleLabel.Frame.X = 19;
			} else {
				this.addToScheduleLabel.Text = "Add to schedule";
				this.addToScheduleLabel.Frame.Width = 98;
				this.addToScheduleLabel.Frame.X = 36;
			}
		}
		
		private SessionEntity _session;
		
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
			AppDelegate.CurrentAppDelegate.TabBar.SelectedIndex = 0;
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
			SetAddToScheduleLabel ();
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
		
		protected void AddToQueue (ScheduledSessionEntity scheduledSession)
		{
			RemoteQueueEntity queueEntity = new RemoteQueueEntity ();
			queueEntity.AddOrRemove = "ADD";
			queueEntity.ConferenceName = "CodeMash";
			queueEntity.DateQueuedOn = DateTime.Now;
			queueEntity.UserName = "gibbensr";
			queueEntity.URI = scheduledSession.URI;
			
			//using (UnitOfWork.Start()) {
			var queueRepository = new LocalQueueRepository ();
			queueRepository.Save (queueEntity);
			//}
		}
		
		protected void AddToRemote (ScheduledSessionEntity scheduledSession)
		{
			var remote = new RemoteScheduledSessionsRepository ();
			var schedule = remote.GetSchedule ("gibbensr");
		}

		protected void AddNotification (SessionEntity session)
		{	
			UILocalNotification notification = new UILocalNotification{
				  FireDate = DateTime.Now.AddSeconds (1),
				  TimeZone = NSTimeZone.LocalTimeZone,
				  AlertBody = session.Title + " will start in 10 minutes in " + session.Room + "(WHEN CODEMASH PUBLISHES REAL DATES, THIS WON'T SHOW IMMEDIATELY",
				  RepeatInterval = 0
				};
			UIApplication.SharedApplication.ScheduleLocalNotification (notification);
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
		
		protected void SetImageUrl ()
		{
			string imagePath = string.Empty;
			if (_session.Technology.ToLower () == ".net") {
				imagePath = "images/Technologies/DotNet.png";
			} else if (_session.Technology.ToLower () == "ruby") {
				imagePath = "images/Technologies/Ruby.png";
			} else if (_session.Technology.ToLower () == "mobile") {
				imagePath = "images/Technologies/mobile2.png";
			} else if (_session.Technology.ToLower () == "javascript") {
				imagePath = "images/Technologies/JavaScript.png";
			} else if (_session.Technology.ToLower () == "design/ux") {
				imagePath = "images/Technologies/DesignUX2.png";
			} else if (_session.Technology.ToLower () == "java") {
				imagePath = "images/Technologies/Java.png";
			} else if (_session.Technology.ToLower () == "windows 8") {
				imagePath = "images/Technologies/Windows.png";
			} else if (_session.Technology.ToLower () == "other languages") {
				imagePath = "images/Technologies/OtherLanguages2.png";
			} else if (_session.Technology.ToLower () == "software process") {
				imagePath = "images/Technologies/SoftwareProcess4.png";
			} else {
				imagePath = "images/Technologies/Other2.png";
			}
			
			UIImage image = GetLargeImage (imagePath);
					
			SetTechnologyImage (image);
		}
	
		public static UIImage GetLargeImage (string imageUrl)
		{
			var smallImages = LargeImages;
			UIImage image;
			if (smallImages.ContainsKey (imageUrl)) {
				image = smallImages [imageUrl];
				if (image.Size.Width == 0) {
					var imageFromFile = UIImage.FromFile (imageUrl);
					imageFromFile = Extensions.RemoveSharpEdges (imageFromFile, Convert.ToInt32 (imageFromFile.Size.Width), 4);
					smallImages [imageUrl] = imageFromFile;
					image = smallImages [imageUrl];
				}
			} else {
				var imageFromFile = UIImage.FromFile (imageUrl);
				imageFromFile = Extensions.RemoveSharpEdges (imageFromFile, Convert.ToInt32 (imageFromFile.Size.Width), 4);
				smallImages [imageUrl] = imageFromFile;
				image = smallImages [imageUrl];
			}
			
			return image;
		}

		private static Dictionary<string, UIImage> _largeImages;

		public static Dictionary<string, UIImage> LargeImages {
			get {
				if (_largeImages == null) {
					_largeImages = new Dictionary<string, UIImage> ();
				}
				
				return _largeImages;
			}
		}			
		
		
		
	}
}
