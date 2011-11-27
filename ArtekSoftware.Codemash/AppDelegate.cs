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
			//Console.WriteLine("AppDelegate.FinishedLaunching");
			CopyDb ();
			
			var bootstrapper = new Bootstrapper ();
			bootstrapper.Initialize ();				
			
			CurrentAppDelegate = this;
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			
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
		
		//private RotatingSessionDetailViewController rotatingSessionDetailViewController;
		
		public void SetSession (SessionEntity session)
		{
			//Console.WriteLine("AppDelegate.SetSession - session is null " + (session == null).ToString());
			
			
			this.TabBar.SelectedIndex = 1; 
			
			//Console.WriteLine("AppDelegate.SetSession - creating new RotatingSessionDetailViewController");
			ISubstitutableDetailViewController detailViewController = new RotatingSessionDetailViewController (session);
			
			//Console.WriteLine("AppDelegate.SetSession - getting existing view controller from SplitViewController");
			var existingVC = this.splitViewController.ViewControllers [1] as ISubstitutableDetailViewController;
			//Console.WriteLine("AppDelegate.SetSession - existingVC is null " + (existingVC == null).ToString());
			
			//if (existingVC.RootPopoverButtonItem != null) {
				detailViewController.RootPopoverButtonItem = existingVC.RootPopoverButtonItem;
//			} else {
//				detailViewController.RootPopoverButtonItem = new UIBarButtonItem () { Title = "CodeMash" };
//			}
//			
			//if (existingVC.PopOverController != null) {
				detailViewController.PopOverController = existingVC.PopOverController;			
//			} else {
//				detailViewController.PopOverController = new UIPopoverController (new TabBarController ());			
//				
//			}
			//Console.WriteLine("AppDelegate.SetSession - detailViewController.RootPopoverButtonItem is null" + (detailViewController.RootPopoverButtonItem == null).ToString());
			//Console.WriteLine("AppDelegate.SetSession -detailViewController.PopOverController is null " + (detailViewController.PopOverController == null).ToString());
			
			//Console.WriteLine("AppDelegate.SetSession - creating new SplitDelegate");
			this.splitViewController.Delegate = new SplitDelegate ();
			//Console.WriteLine("AppDelegate.SetSession - setting splitViewControllers.ViewControllers");
			this.splitViewController.ViewControllers = new UIViewController[] {
					this.TabBar,
					detailViewController as UIViewController
				};
			
			//Console.WriteLine("AppDelegate.SetSession - setting window.RootViewController");
			window.RootViewController = splitViewController;
			
			if (detailViewController.RootPopoverButtonItem != null) {
				//Console.WriteLine("AppDelegate.SetSession - calling detailViewController.ShowRootPopoverButtonItem (detailViewController.RootPopoverButtonItem)");
				detailViewController.ShowRootPopoverButtonItem (detailViewController.RootPopoverButtonItem);
			}
			if (detailViewController.PopOverController != null) {
				//Console.WriteLine("AppDelegate.SetSession - calling detailViewController.PopOverController.Dismiss (true)");
				
				detailViewController.PopOverController.Dismiss (true);
			}
		
		}
		
		//private RotatingSpeakerBioViewController rotatingSpeakerBioViewController;
		
		public void SetSpeaker (SpeakerEntity speaker)
		{
			//Console.WriteLine("AppDelegate.SetSpeaker - speaker is null " + (speaker == null).ToString());
			ISubstitutableDetailViewController detailViewController = new RotatingSpeakerBioViewController (speaker);
			
			var existingVC = this.splitViewController.ViewControllers [1] as ISubstitutableDetailViewController;
			detailViewController.RootPopoverButtonItem = existingVC.RootPopoverButtonItem;
			detailViewController.PopOverController = existingVC.PopOverController;			
			
			this.TabBar.SelectedIndex = 2; 
	
			this.splitViewController.Delegate = new SplitDelegate ();
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