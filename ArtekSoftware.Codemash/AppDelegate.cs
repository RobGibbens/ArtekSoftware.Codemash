using System;
using System.IO;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Catnap;
using MonoQueue;
using Localytics;

namespace ArtekSoftware.Codemash
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		UIWindow window;
		UISplitViewController splitViewController;
		public static AppDelegate CurrentAppDelegate;
		private TabBarControllerBase _tabBarController;
		public UINavigationController NavigationController;
		
		private static IMessageBus _messageBus;
		
		public static IMessageBus MessageBus
		{
			get
			{
//				if (_messageBus == null)
//				{
//					int checkPendingMessagesMilliseconds = 10000;
//					ISerializer serializer = new Serializer ();
//					INetworkStatusCheck networkStatusCheck = new NetworkStatusCheck();
//					_messageBus = new MessageBus(networkStatusCheck, serializer, checkPendingMessagesMilliseconds);
//				}
				
				return _messageBus;
			}
		}
		
		public TabBarControllerBase TabBar {
			get { return _tabBarController; }
			set { _tabBarController = value; }
		}
		
		string YOUR_LOCALYTICS_APP_KEY = "db527378f483397d685863d-51e8089a-33ee-11e1-a1cc-008545fe83d2";
		
		public static void LogToAnalytics(string message)
		{
			Localytics.LocalyticsSession.SharedLocalyticsSession.TagEvent(message);
			Localytics.LocalyticsSession.SharedLocalyticsSession.Upload();
		}
		
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{	
			Localytics.LocalyticsSession.SharedLocalyticsSession.StartSession(YOUR_LOCALYTICS_APP_KEY);
			Localytics.LocalyticsSession.SharedLocalyticsSession.Open();
			
			LogToAnalytics("App launched");
			
			CopyDb ();
			
			TestFlightProxy.TakeOff ("19a8eedfedeed47cf1f6d74fd7ab561c_MTkxNDIwMTEtMDktMjkgMjE6MTc6MTAuNjM0NTAw");
			
			var bootstrapper = new Bootstrapper ();
			bootstrapper.Initialize ();				
			
			CurrentAppDelegate = this;
			
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) {
				this.TabBar = new iPhoneTabBarController ();
				this.NavigationController = new UINavigationController (this.TabBar);
				this.NavigationController.NavigationBarHidden = true;
				window.RootViewController = this.NavigationController;
			} else {
				//Console.WriteLine("AppDelegate.FinishedLaunching - creating new TabBarController and DetailViewController");
			
				this.TabBar = new TabBarController ();
				var detailViewController = new DetailViewController ();
				//Console.WriteLine("AppDelegate.FinishedLaunching - creating new SplitViewController");
				splitViewController = new UISplitViewController ();
				//splitViewController.WeakDelegate = detailViewController;
				splitViewController.Delegate = new SplitDelegate ();
				splitViewController.ViewControllers = new UIViewController[] {
						this.TabBar,
						detailViewController
				};
			
				//Console.WriteLine("AppDelegate.FinishedLaunching - setting window.RootViewController");
			
				window.RootViewController = splitViewController;
			}

			window.MakeKeyAndVisible ();
			var bus = AppDelegate.MessageBus;
			
			return true;
		}
	
		public void CopyDb ()
		{
			string dbname = "codemash.db3";
			string documents = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			string db = Path.Combine (documents, dbname);
 
			string rootPath = Path.Combine (Environment.CurrentDirectory, "DefaultDatabase");
			string rootDbPath = Path.Combine (rootPath, dbname);
 
			var runtimeDbExists = File.Exists (db);
			var defaultDatabaseExists = File.Exists (rootDbPath);
			
			if (!runtimeDbExists && defaultDatabaseExists) {
				File.Copy (rootDbPath, db);
			
				TestFlightProxy.PassCheckpoint ("Copied default database");
				LogToAnalytics("Copied default database");
			} 
 
		}
		
		public void SetSession (SessionEntity session)
		{
			this.TabBar.SelectedIndex = 1; 
			
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) {
				this.NavigationController.NavigationBarHidden = false;
				var iPhoneSessionDetailViewController = new iPhoneSessionDetailViewController (session);
				((UINavigationController)window.RootViewController).PushViewController (iPhoneSessionDetailViewController, animated:true);
			} else {
				RotatingSessionDetailViewController rotatingSessionDetailViewController = new RotatingSessionDetailViewController (session);
			
				var existingVC = this.splitViewController.ViewControllers [1] as ISubstitutableDetailViewController;
				rotatingSessionDetailViewController.RootPopoverButtonItem = existingVC.RootPopoverButtonItem;
				rotatingSessionDetailViewController.PopOverController = existingVC.PopOverController;			
	
				var splitDelegate = new SplitDelegate ();
				this.splitViewController.Delegate = splitDelegate;
			
				this.splitViewController.ViewControllers = new UIViewController[] {
					this.TabBar,
					rotatingSessionDetailViewController
				};
			
				window.RootViewController = splitViewController;
			
				if (rotatingSessionDetailViewController.RootPopoverButtonItem != null) {
					rotatingSessionDetailViewController.ShowRootPopoverButtonItem (rotatingSessionDetailViewController.RootPopoverButtonItem);
				}
			
				if (rotatingSessionDetailViewController.PopOverController != null) {
					rotatingSessionDetailViewController.PopOverController.Dismiss (true);
				}
			}
			
			LogToAnalytics("Viewed session " + session.URI);
		}
		
		public void SetSpeaker (SpeakerEntity speaker)
		{
			if (speaker != null) {
				this.TabBar.SelectedIndex = 2;
			
				if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) {
					this.NavigationController.NavigationBarHidden = false;
					var iPhoneSpeakerBioViewController = new iPhoneSpeakerBioViewController (speaker);
					((UINavigationController)window.RootViewController).PushViewController (iPhoneSpeakerBioViewController, animated:true);
				} else {			
					RotatingSpeakerBioViewController rotatingSpeakerBioViewController = new RotatingSpeakerBioViewController (speaker);

					var existingVC = this.splitViewController.ViewControllers [1] as ISubstitutableDetailViewController;
					rotatingSpeakerBioViewController.RootPopoverButtonItem = existingVC.RootPopoverButtonItem;
					rotatingSpeakerBioViewController.PopOverController = existingVC.PopOverController;			 
	
					var splitDelegate = new SplitDelegate ();		
					this.splitViewController.Delegate = splitDelegate;
					this.splitViewController.ViewControllers = new UIViewController[] {
						this.TabBar,
						rotatingSpeakerBioViewController
					};
			
					if (rotatingSpeakerBioViewController.RootPopoverButtonItem != null) {
						rotatingSpeakerBioViewController.ShowRootPopoverButtonItem (rotatingSpeakerBioViewController.RootPopoverButtonItem);
					}
			
					if (rotatingSpeakerBioViewController.PopOverController != null) {
						rotatingSpeakerBioViewController.PopOverController.Dismiss (true);
					}
				}
				
				LogToAnalytics("Viewed speaker " + speaker.SpeakerURI);
				
			}
			
		}
		
		public void SetLocationMap ()
		{
			this.TabBar.SelectedIndex = 3;
			
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) {
				this.NavigationController.NavigationBarHidden = false;
				var mapFlipViewController = new iPhoneMapFlipViewController ();
				((UINavigationController)window.RootViewController).PushViewController (mapFlipViewController, animated:true);
			} else {	
				RotatingLocationMapViewController rotatingLocationMapViewController = new RotatingLocationMapViewController ();
			
				var existingVC = this.splitViewController.ViewControllers [1] as ISubstitutableDetailViewController;
				rotatingLocationMapViewController.RootPopoverButtonItem = existingVC.RootPopoverButtonItem;
				rotatingLocationMapViewController.PopOverController = existingVC.PopOverController;					
			
				var splitDelegate = new SplitDelegate ();		
				this.splitViewController.Delegate = splitDelegate;
				this.splitViewController.ViewControllers = new UIViewController[] {
					this.TabBar,
					rotatingLocationMapViewController
				};
			
				if (rotatingLocationMapViewController.RootPopoverButtonItem != null) {
					rotatingLocationMapViewController.ShowRootPopoverButtonItem (rotatingLocationMapViewController.RootPopoverButtonItem);
				}
			
				if (rotatingLocationMapViewController.PopOverController != null) {
					rotatingLocationMapViewController.PopOverController.Dismiss (true);
				}				
				
			}
			
			LogToAnalytics("Viewed location map");
		}
		
		public void SetMap ()
		{
			this.TabBar.SelectedIndex = 3;
			
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) {
				this.NavigationController.NavigationBarHidden = false;
				var iPhoneMapViewController = new iPhoneMapViewController ();
				((UINavigationController)window.RootViewController).PushViewController (iPhoneMapViewController, animated:true);
			} else {	
				MapViewController mapViewController = new MapViewController ();
			
				var existingVC = this.splitViewController.ViewControllers [1] as ISubstitutableDetailViewController;
				mapViewController.RootPopoverButtonItem = existingVC.RootPopoverButtonItem;
				mapViewController.PopOverController = existingVC.PopOverController;					
			
				var splitDelegate = new SplitDelegate ();		
				this.splitViewController.Delegate = splitDelegate;
				this.splitViewController.ViewControllers = new UIViewController[] {
					this.TabBar,
					mapViewController
				};
			
				if (mapViewController.RootPopoverButtonItem != null) {
					mapViewController.ShowRootPopoverButtonItem (mapViewController.RootPopoverButtonItem);
				}
			
				if (mapViewController.PopOverController != null) {
					mapViewController.PopOverController.Dismiss (true);
				}
			}
			
			LogToAnalytics("Viewed conference venue map");
		}
	
	
		public override void WillEnterForeground (UIApplication application)
		{
			Localytics.LocalyticsSession.SharedLocalyticsSession.Resume();
			Localytics.LocalyticsSession.SharedLocalyticsSession.Upload();
		}
		
		public override void DidEnterBackground (UIApplication application)
		{
			CloseLocalyticsSession();
		}
		
		public override void WillTerminate (UIApplication application)
		{
			CloseLocalyticsSession();
		}
		
		void CloseLocalyticsSession()
		{
			Localytics.LocalyticsSession.SharedLocalyticsSession.Close();
			Localytics.LocalyticsSession.SharedLocalyticsSession.Upload();
		}	
	
	}

	/// EXTREMELY IMPORTANT: You must include a subclass of these classes
	/// due to a known issue in MonoTouch as of 3.2.4.  The issue is expected
	/// to be resolved in the next major release of MonoTouch.
	class SubLocalyticsSession : LocalyticsSession {}
	class SubUploaderThread : UploaderThread {}
}