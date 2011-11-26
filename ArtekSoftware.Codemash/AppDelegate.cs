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
		
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			CurrentAppDelegate = this;
			
			CopyDb ();
			
			var bootstrapper = new Bootstrapper ();
			bootstrapper.Initialize ();				
			
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			
			var tabBarController = new TabBarController ();
			var detailViewController = new DetailViewController ();
			
			splitViewController = new UISplitViewController ();
			splitViewController.WeakDelegate = detailViewController;
			splitViewController.ViewControllers = new UIViewController[] {
				tabBarController,
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
 
			if (!File.Exists (db) && File.Exists (rootDbPath)) {
				File.Copy (rootDbPath, db);
				//TestFlightSdk.TestFlight.PassCheckpoint ("Copied default database");
			}
 
		}
		
		private RotatingSessionDetailViewController rotatingSessionDetailViewController;
		
		public void SetSession (SessionEntity session)
		{
			this.splitViewController.ViewControllers [1] = null;
			var tabBarController = new TabBarController ();
			tabBarController.SelectedIndex = 1; 
			rotatingSessionDetailViewController = new RotatingSessionDetailViewController (session);
			this.splitViewController.WeakDelegate = rotatingSessionDetailViewController;
			this.splitViewController.ViewControllers = new UIViewController[] {
					tabBarController,
					rotatingSessionDetailViewController
				};
			rotatingSessionDetailViewController.ViewWillAppear (true); 
		}
		
		public void SetSpeaker (SpeakerEntity speaker)
		{
			this.splitViewController.ViewControllers [1] = null;
			RotatingSpeakerBioViewController rotatingSpeakerBioViewController;
		
			rotatingSpeakerBioViewController = new RotatingSpeakerBioViewController (speaker);
			var tabBarController = new TabBarController ();
			tabBarController.SelectedIndex = 2; 
			this.splitViewController.ViewControllers = new UIViewController[] {
					tabBarController,
					rotatingSpeakerBioViewController
				};			
		}			
	}
}