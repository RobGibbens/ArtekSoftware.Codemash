using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Catnap;
using Catnap.Find;
using Catnap.Find.Conditions;
////using MonoQueue;
using MonoTouch.Dialog.Utilities;
using MonoTouch.Foundation;
using MonoTouch.Twitter;
using MonoTouch.UIKit;

namespace ArtekSoftware.Codemash
{
	public abstract class SessionDetailViewControllerBase : UIViewController
	{
		protected abstract UIScrollView ScrollView { get; }

		protected abstract UIImageView TechnologyImage { get; }

		protected abstract UIButton BtnAddToSchedule { get; }

		protected abstract UIButton BtnTweetThis { get; }

		protected abstract UILabel SessionTitleLabel { get; }

		protected abstract UILabel SessionStartLabel { get; }

		protected abstract UILabel SessionRoomLabel { get; }

		protected abstract UIButton SessionSpeakerNameButton { get; }

		protected abstract UILabel SessionTechnologyLabel { get; }

		protected abstract UILabel SessionDifficultyLabel { get; }

		protected abstract UILabel TweetThisLabel { get; }

		protected abstract UILabel SessionAbstractLabel { get; }

		protected abstract UILabel AddToScheduleLabel { get; }

		protected abstract UIPopoverController PopoverController { get; }

		protected abstract int NumberOfControls { get; }

		protected abstract TechnologyImages TechImages { get; }
		
		protected INetworkStatusCheck _networkStatusCheck;
		protected SessionEntity _session;
		
		public SessionDetailViewControllerBase (SessionEntity session, string nibName, NSBundle bundle) : base (nibName, bundle)
		{
			_session = session;
			_networkStatusCheck = new NetworkStatusCheck ();
		}

		public SessionDetailViewControllerBase (string nibName, NSBundle bundle) : base(nibName, bundle)
		{	
			_networkStatusCheck = new NetworkStatusCheck ();
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) {
				//TODO: This isn't in iPad
				this.ScrollView.Frame = new RectangleF (0, 0, 320, 460);
				//TODO: This isn't in iPad
				this.ScrollView.ContentSize = new SizeF (320, 1050);
			}
			
			var version = Convert.ToDecimal (UIDevice.CurrentDevice.SystemVersion.Split ('.').First ());
			
			if (version < 5) {
				this.BtnTweetThis.Hidden = true;
				this.TweetThisLabel.Hidden = true;
			} else {
				this.BtnTweetThis.Hidden = false;
				this.TweetThisLabel.Hidden = false;
			}
			
			this.BtnAddToSchedule.TouchUpInside += HandleSessionAddToScheduleButtonhandleTouchUpInside;
			this.BtnTweetThis.TouchUpInside += HandleTweetThisButtonHandleTouchUpInside;
			this.SessionSpeakerNameButton.TouchUpInside += HandleSessionSpeakerNameLabelTouchUpInside;
			
			if (this.Session != null) {
				this.SetSession (this.Session);
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
		
		protected void AddToQueue (ScheduledSessionEntity scheduledSession)
		{
//			var message = new AddSessionToRemoteScheduleCommand (_networkStatusCheck)
//			{
//				ScheduleId = scheduledSession.Id,
//				SessionUri = scheduledSession.URI,
//			};
//
//			AppDelegate.MessageBus.Send (message);
			
		}
		
		protected void RemoveFromQueue (ScheduledSessionEntity scheduledSession)
		{
//			var message = new RemoveSessionFromRemoteScheduleCommand (_networkStatusCheck)
//			{
//				ScheduleId = scheduledSession.Id,
//				SessionUri = scheduledSession.URI,
//			};
//
//			AppDelegate.MessageBus.Send (message);
		}
		
		protected void HandleSessionSpeakerNameLabelTouchUpInside (object sender, EventArgs e)
		{
			Console.WriteLine("HandleSessionSpeakerNameLabelTouchUpInside-1");
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
			
			AppDelegate.CurrentAppDelegate.Navigation.SetSpeaker (speaker);
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
						
						AddToQueue (scheduledSession);
						
						AppDelegate.CurrentAppDelegate.Analytics.Log("Added session to schedule " + _session.URI);
					}
				}
			} else {
				
				using (UnitOfWork.Start()) {
					var repository = new LocalScheduledSessionsRepository ();
					var scheduledSession = repository.GetScheduledSession (_session.URI);
					RemoveFromQueue (scheduledSession);
					repository.Delete (scheduledSession.Id);
					
					var vc = AppDelegate.CurrentAppDelegate.TabBar.ViewControllers [0];
					var uinc = (UINavigationController)vc;
					var scheduleController = (ScheduledSessionDialogViewController)uinc.TopViewController;
					scheduleController.LoadData ();
				}
				RemoveNotification (_session);
				AppDelegate.CurrentAppDelegate.Analytics.Log ("Removed session from schedule " + _session.URI);
				
			}
			
			SetAddToScheduleLabel ();
		}
		
		public void SetSession (SessionEntity session)
		{
			this.Session = session;
			
			this.SessionRoomLabel.Text = session.Room;
			this.SessionSpeakerNameButton.SetTitle (session.SpeakerName, UIControlState.Normal);
			if (session.StartDate == DateTime.MinValue) {
				this.SessionStartLabel.Text = "No date/time - Please Refresh";
			} else {
				this.SessionStartLabel.Text = session.StartDate.DayOfWeek.ToString () + " " + session.StartDate.ToString ("h:mm tt");
			}
			this.SessionTechnologyLabel.Text = session.Technology + " / " + session.Difficulty;
			
			SetImageUrl ();
			
			this.SessionTitleLabel.Text = session.Title;
			
			HLabel titleLabel;
			if (this.ScrollView.Subviews.Count () <= this.NumberOfControls) {
				titleLabel = new HLabel ();
			} else {
				titleLabel = (HLabel)this.ScrollView.Subviews [this.NumberOfControls];
			}
			titleLabel.VerticalAlignment = HLabel.VerticalAlignments.Top;
			titleLabel.Lines = 0;
			titleLabel.LineBreakMode = UILineBreakMode.WordWrap;
			titleLabel.Text = this.SessionTitleLabel.Text;
			titleLabel.Font = this.SessionTitleLabel.Font;
			titleLabel.Frame = this.SessionTitleLabel.Frame;
			titleLabel.BackgroundColor = UIColor.Clear;
			
			if (this.ScrollView.Subviews.Count () <= this.NumberOfControls) {
				this.ScrollView.AddSubview (titleLabel);
			} else {
				this.ScrollView.Subviews [this.NumberOfControls] = titleLabel;
			}
			
			this.SessionTitleLabel.Text = string.Empty;	
			
			
			HLabel abstractLabel;
			if (this.ScrollView.Subviews.Count () <= this.NumberOfControls + 1) {
				abstractLabel = new HLabel ();
			} else {
				abstractLabel = (HLabel)this.ScrollView.Subviews [this.NumberOfControls + 1]; 
			}
			abstractLabel.VerticalAlignment = HLabel.VerticalAlignments.Top;
			abstractLabel.Lines = 0;
			abstractLabel.LineBreakMode = UILineBreakMode.WordWrap;
			abstractLabel.Text = session.Abstract;
			abstractLabel.Font = UIFont.FromName ("STHeitiTC-Light", 14);
			abstractLabel.Frame = this.SessionAbstractLabel.Frame;
			abstractLabel.BackgroundColor = UIColor.Clear;
			
			if (this.ScrollView.Subviews.Count () <= this.NumberOfControls + 1) {
				this.ScrollView.AddSubview (abstractLabel);
			} else {
				this.ScrollView.Subviews [this.NumberOfControls + 1] = abstractLabel;
			}			
			this.SessionAbstractLabel.Text = string.Empty;			
			
			SetAddToScheduleLabel ();
			
			//TODO : This isn't in iPad
			SetImageUrl ();
			SetLocalImage (ImageUrl);
			
			this.View.SetNeedsLayout ();
			this.View.LayoutIfNeeded ();
			
			if (this.PopoverController != null) {
				this.PopoverController.Dismiss (true);
			}
				
		}
		
		protected void SetAddToScheduleLabel ()
		{
			if (IsOnSchedule ()) {
				this.AddToScheduleLabel.Text = "Remove from schedule";
				this.BtnAddToSchedule.SetImage (UIImage.FromFile ("images/FavoritedSession.png"), UIControlState.Normal);
			} else {
				this.AddToScheduleLabel.Text = "Add to schedule";
				this.BtnAddToSchedule.SetImage (UIImage.FromFile ("images/FavoriteSession.png"), UIControlState.Normal);
			}
		}
		
		private string ImageUrl;

		private void SetImageUrl ()
		{
			
			if (_session.Technology.ToLower () == ".net") {
				this.ImageUrl = this.TechImages.DotNet;
			} else if (_session.Technology.ToLower () == "ruby") {
				this.ImageUrl = this.TechImages.Ruby;
			} else if (_session.Technology.ToLower () == "mobile") {
				this.ImageUrl = this.TechImages.Mobile;
			} else if (_session.Technology.ToLower () == "javascript") {
				this.ImageUrl = this.TechImages.JavaScript;
			} else if (_session.Technology.ToLower () == "design/ux") {
				this.ImageUrl = this.TechImages.DesignUX;
			} else if (_session.Technology.ToLower () == "java") {
				this.ImageUrl = this.TechImages.Java;
			} else if (_session.Technology.ToLower () == "windows 8") {
				this.ImageUrl = this.TechImages.Windows;
			} else if (_session.Technology.ToLower () == "other languages") {
				this.ImageUrl = this.TechImages.OtherLanguages;
			} else if (_session.Technology.ToLower () == "software process") {
				this.ImageUrl = this.TechImages.SoftwareProcess;
			} else {
				this.ImageUrl = this.TechImages.Other;
			}		
		}

		public void SetLocalImage (string url)
		{
			if (!string.IsNullOrEmpty (url)) {
				var imageBackground = new Uri ("file://" + Path.GetFullPath (url));
				var image = ImageLoader.DefaultRequestImage (imageBackground, null);
				
				using (this.TechnologyImage.Image) {
					this.TechnologyImage.Image = image;
				}
				
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
	}
	
}

