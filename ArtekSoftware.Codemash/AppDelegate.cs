using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.IO;

namespace ArtekSoftware.Codemash
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		UIWindow window;
		UISplitViewController splitViewController;
		public static AppDelegate CurrentAppDelegate;
		
		private TabBarController _tabBarController;
		public TabBarController TabBar {
			get { return _tabBarController; }
			set { _tabBarController = value; }
		}
		
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			
			CopyDb ();
			
			var bootstrapper = new Bootstrapper ();
			bootstrapper.Initialize ();				
			
			CurrentAppDelegate = this;
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			
			this.TabBar = new TabBarController ();
			var detailViewController = new DetailViewController ();
			
			splitViewController = new UISplitViewController ();
			//splitViewController.WeakDelegate = detailViewController;
			splitViewController.Delegate = new SplitDelegate();
			splitViewController.ViewControllers = new UIViewController[] {
				this.TabBar,
				detailViewController
			};
			
			window.RootViewController = splitViewController;

			window.MakeKeyAndVisible ();
			
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
				//TestFlightSdk.TestFlight.PassCheckpoint ("Copied default database");
			}
 
		}
		
		private RotatingSessionDetailViewController rotatingSessionDetailViewController;
		
		public void SetSession (SessionEntity session)
		{
			ISubstitutableDetailViewController detailViewController = null;
			
			this.TabBar.SelectedIndex = 1; 
			rotatingSessionDetailViewController = new RotatingSessionDetailViewController (session);
			
			detailViewController = rotatingSessionDetailViewController;
			
			var existingVC = this.splitViewController.ViewControllers [1] as ISubstitutableDetailViewController;
			detailViewController.RootPopoverButtonItem = existingVC.RootPopoverButtonItem;
			detailViewController.PopOverController = existingVC.PopOverController;			
			
			this.splitViewController.Delegate = new SplitDelegate();
			this.splitViewController.ViewControllers = new UIViewController[] {
					this.TabBar,
					rotatingSessionDetailViewController
				};
			
			if (detailViewController.RootPopoverButtonItem != null) {
				detailViewController.ShowRootPopoverButtonItem (detailViewController.RootPopoverButtonItem);
			}
			if (detailViewController.PopOverController != null) {
				detailViewController.PopOverController.Dismiss (true);
			}
		
		}
		
		//private RotatingSpeakerBioViewController rotatingSpeakerBioViewController;
		
		public void SetSpeaker (SpeakerEntity speaker)
		{
			ISubstitutableDetailViewController detailViewController = new RotatingSpeakerBioViewController (speaker);
			
			var existingVC = this.splitViewController.ViewControllers [1] as ISubstitutableDetailViewController;
			detailViewController.RootPopoverButtonItem = existingVC.RootPopoverButtonItem;
			detailViewController.PopOverController = existingVC.PopOverController;			
			
			this.TabBar.SelectedIndex = 2; 
	
			this.splitViewController.Delegate = new SplitDelegate();
			this.splitViewController.ViewControllers = new UIViewController[] {
					this.TabBar,
					detailViewController as UIViewController 
				};
			
			if (detailViewController.RootPopoverButtonItem != null) {
				detailViewController.ShowRootPopoverButtonItem (detailViewController.RootPopoverButtonItem);
			}
			
			if (detailViewController.PopOverController != null) {
				detailViewController.PopOverController.Dismiss (true);
			}
		}
		
		public void SetMap ()
		{
			this.splitViewController.ViewControllers [1] = null;
			MapViewController mapViewController;
			
			mapViewController = new MapViewController ();
			
			this.TabBar.SelectedIndex = 3; 
			this.splitViewController.ViewControllers = new UIViewController[] {
					this.TabBar,
					mapViewController
				};
		}
	}
}