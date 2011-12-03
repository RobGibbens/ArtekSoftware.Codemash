using System;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using System.Linq;

namespace ArtekSoftware.Codemash
{
	public class iPhoneTabBarController : TabBarControllerBase
	{
		MonoTouch.UIKit.UINavigationController navScheduleController, navSessionController, navSpeakerController, mapController;
		
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			AppDelegate.CurrentAppDelegate.NavigationController.NavigationBarHidden = true;
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			var scheduleViewController = new ScheduledSessionDialogViewController ();
			navScheduleController = new MonoTouch.UIKit.UINavigationController ();
			navScheduleController.PushViewController (scheduleViewController, false);
			SetSessionBackground (navScheduleController.NavigationBar);
			navScheduleController.NavigationBar.BarStyle = UIBarStyle.Black;
			navScheduleController.NavigationBar.BackgroundColor = UIColor.Clear;
			navScheduleController.NavigationBar.TintColor = UIColor.Clear;
			navScheduleController.TopViewController.Title = "Schedule";
			navScheduleController.TabBarItem = new UITabBarItem ("Schedule", UIImage.FromFile ("images/glyphicons_054_clock.png"), 0);
			
			var sessionsViewController = new SessionDialogViewController ();
			navSessionController = new MonoTouch.UIKit.UINavigationController ();
			navSessionController.PushViewController (sessionsViewController, false);
			SetSessionBackground (navSessionController.NavigationBar);
			navSessionController.NavigationBar.BarStyle = UIBarStyle.Black;
			navSessionController.NavigationBar.BackgroundColor = UIColor.Clear;
			navSessionController.NavigationBar.TintColor = UIColor.Clear;
			navSessionController.TabBarItem = new UITabBarItem ("Sessions", UIImage.FromFile ("images/glyphicons_061_keynote.png"), 0);
			
			var speakersViewController = new SpeakerDialogViewController ();
			navSpeakerController = new MonoTouch.UIKit.UINavigationController ();
			navSpeakerController.PushViewController (speakersViewController, false);
			SetSessionBackground (navSpeakerController.NavigationBar);
			navSpeakerController.NavigationBar.BarStyle = UIBarStyle.Black;
			navSpeakerController.NavigationBar.TintColor = UIColor.Clear;
			navSpeakerController.TabBarItem = new UITabBarItem ("Speakers", UIImage.FromFile ("images/glyphicons_042_group.png"), 0);

			var mapViewController = new MapDialogViewController ();
			mapController = new MonoTouch.UIKit.UINavigationController ();
			mapController.PushViewController (mapViewController, false);
			SetSessionBackground (mapController.NavigationBar);
			mapController.NavigationBar.BarStyle = UIBarStyle.Black;
			mapController.NavigationBar.BackgroundColor = UIColor.Clear;
			mapController.NavigationBar.TintColor = UIColor.Clear;
			mapController.TabBarItem = new UITabBarItem ("Maps", UIImage.FromFile ("images/glyphicons_060_compass.png"), 0);			
			
			var u = new UIViewController[]{
				navScheduleController, navSessionController, navSpeakerController, mapController};
			
			this.ViewControllers = u;
			
			this.SelectedViewController = navSessionController;
			
			this.MoreNavigationController.NavigationBar.BarStyle = UIBarStyle.Black;
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return true;
		}
		
		private void SetSessionBackground (UINavigationBar navigationBar)
		{
			var version = Convert.ToDecimal (UIDevice.CurrentDevice.SystemVersion.Split ('.').First ());
			
			if (version >= 5) {
				navigationBar.SetBackgroundImage (UIImage.FromFile ("images/SessionsHeader2.png"), UIBarMetrics.Default);
			} else {
				navigationBar.InsertSubview (new UIImageView (UIImage.FromFile ("images/SessionsHeader2.png")), 0);
			}
		}
	}
}

