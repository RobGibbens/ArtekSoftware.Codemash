using System;
using MonoTouch.UIKit;
using ArtekSoftware.Conference.LocalData;

namespace ArtekSoftware.Codemash
{
	public class Navigation : INavigation
	{
		private UIWindow _window;

		public Navigation (UIWindow window)
		{
			_window = window;
		}
		
		#region INavigation implementation
		public void SetSession (SessionEntity session)
		{
			AppDelegate.CurrentAppDelegate.TabBar.SelectedIndex = 1; 
			
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) {
				AppDelegate.CurrentAppDelegate.NavigationController.NavigationBarHidden = false;
				var iPhoneSessionDetailViewController = new iPhoneSessionDetailViewController (session);
				((UINavigationController)_window.RootViewController).PushViewController (iPhoneSessionDetailViewController, animated:true);
			} else {
				RotatingSessionDetailViewController rotatingSessionDetailViewController = new RotatingSessionDetailViewController (session);
			
				var existingVC = AppDelegate.CurrentAppDelegate.splitViewController.ViewControllers [1] as ISubstitutableDetailViewController;
				rotatingSessionDetailViewController.RootPopoverButtonItem = existingVC.RootPopoverButtonItem;
				rotatingSessionDetailViewController.PopOverController = existingVC.PopOverController;			
	
				var splitDelegate = new SplitDelegate ();
				AppDelegate.CurrentAppDelegate.splitViewController.Delegate = splitDelegate;
			
				AppDelegate.CurrentAppDelegate.splitViewController.ViewControllers = new UIViewController[] {
					AppDelegate.CurrentAppDelegate.TabBar,
					rotatingSessionDetailViewController
				};
			
				_window.RootViewController = AppDelegate.CurrentAppDelegate.splitViewController;
			
				if (rotatingSessionDetailViewController.RootPopoverButtonItem != null) {
					rotatingSessionDetailViewController.ShowRootPopoverButtonItem (rotatingSessionDetailViewController.RootPopoverButtonItem);
				}
			
				if (rotatingSessionDetailViewController.PopOverController != null) {
					rotatingSessionDetailViewController.PopOverController.Dismiss (true);
				}
			}
			
			AppDelegate.CurrentAppDelegate.Analytics.Log ("Viewed session " + session.Slug);
		}

		public void SetSpeaker (SpeakerEntity speaker)
		{
			if (speaker != null) {
				AppDelegate.CurrentAppDelegate.TabBar.SelectedIndex = 2;
			
				if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) {
					AppDelegate.CurrentAppDelegate.NavigationController.NavigationBarHidden = false;
					var iPhoneSpeakerBioViewController = new iPhoneSpeakerBioViewController (speaker);
					((UINavigationController)_window.RootViewController).PushViewController (iPhoneSpeakerBioViewController, animated:true);
				} else {			
					RotatingSpeakerBioViewController rotatingSpeakerBioViewController = new RotatingSpeakerBioViewController (speaker);

					var existingVC = AppDelegate.CurrentAppDelegate.splitViewController.ViewControllers [1] as ISubstitutableDetailViewController;
					rotatingSpeakerBioViewController.RootPopoverButtonItem = existingVC.RootPopoverButtonItem;
					rotatingSpeakerBioViewController.PopOverController = existingVC.PopOverController;			 
	
					var splitDelegate = new SplitDelegate ();		
					AppDelegate.CurrentAppDelegate.splitViewController.Delegate = splitDelegate;
					AppDelegate.CurrentAppDelegate.splitViewController.ViewControllers = new UIViewController[] {
						AppDelegate.CurrentAppDelegate.TabBar,
						rotatingSpeakerBioViewController
					};
			
					if (rotatingSpeakerBioViewController.RootPopoverButtonItem != null) {
						rotatingSpeakerBioViewController.ShowRootPopoverButtonItem (rotatingSpeakerBioViewController.RootPopoverButtonItem);
					}
			
					if (rotatingSpeakerBioViewController.PopOverController != null) {
						rotatingSpeakerBioViewController.PopOverController.Dismiss (true);
					}
				}
				
				AppDelegate.CurrentAppDelegate.Analytics.Log ("Viewed speaker " + speaker.Slug);
				
			}
			
		}

		public void SetLocationMap ()
		{
			AppDelegate.CurrentAppDelegate.TabBar.SelectedIndex = 3;
			
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) {
				AppDelegate.CurrentAppDelegate.NavigationController.NavigationBarHidden = false;
				var mapFlipViewController = new iPhoneMapFlipViewController ();
				((UINavigationController)_window.RootViewController).PushViewController (mapFlipViewController, animated:true);
			} else {	
				RotatingLocationMapViewController rotatingLocationMapViewController = new RotatingLocationMapViewController ();
			
				var existingVC = AppDelegate.CurrentAppDelegate.splitViewController.ViewControllers [1] as ISubstitutableDetailViewController;
				rotatingLocationMapViewController.RootPopoverButtonItem = existingVC.RootPopoverButtonItem;
				rotatingLocationMapViewController.PopOverController = existingVC.PopOverController;					
			
				var splitDelegate = new SplitDelegate ();		
				AppDelegate.CurrentAppDelegate.splitViewController.Delegate = splitDelegate;
				AppDelegate.CurrentAppDelegate.splitViewController.ViewControllers = new UIViewController[] {
					AppDelegate.CurrentAppDelegate.TabBar,
					rotatingLocationMapViewController
				};
			
				if (rotatingLocationMapViewController.RootPopoverButtonItem != null) {
					rotatingLocationMapViewController.ShowRootPopoverButtonItem (rotatingLocationMapViewController.RootPopoverButtonItem);
				}
			
				if (rotatingLocationMapViewController.PopOverController != null) {
					rotatingLocationMapViewController.PopOverController.Dismiss (true);
				}				
				
			}
			
			AppDelegate.CurrentAppDelegate.Analytics.Log ("Viewed location map");
		}

		public void SetMap ()
		{
			AppDelegate.CurrentAppDelegate.TabBar.SelectedIndex = 3;
			
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) {
				AppDelegate.CurrentAppDelegate.NavigationController.NavigationBarHidden = false;
				var iPhoneMapViewController = new iPhoneMapViewController ();
				((UINavigationController)_window.RootViewController).PushViewController (iPhoneMapViewController, animated:true);
			} else {	
				MapViewController mapViewController = new MapViewController ();
			
				var existingVC = AppDelegate.CurrentAppDelegate.splitViewController.ViewControllers [1] as ISubstitutableDetailViewController;
				mapViewController.RootPopoverButtonItem = existingVC.RootPopoverButtonItem;
				mapViewController.PopOverController = existingVC.PopOverController;					
			
				var splitDelegate = new SplitDelegate ();		
				AppDelegate.CurrentAppDelegate.splitViewController.Delegate = splitDelegate;
				AppDelegate.CurrentAppDelegate.splitViewController.ViewControllers = new UIViewController[] {
					AppDelegate.CurrentAppDelegate.TabBar,
					mapViewController
				};
			
				if (mapViewController.RootPopoverButtonItem != null) {
					mapViewController.ShowRootPopoverButtonItem (mapViewController.RootPopoverButtonItem);
				}
			
				if (mapViewController.PopOverController != null) {
					mapViewController.PopOverController.Dismiss (true);
				}
			}
			
			AppDelegate.CurrentAppDelegate.Analytics.Log ("Viewed conference venue map");
		}
		#endregion
	}
	
}