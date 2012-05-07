using System;
using System.IO;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
//using Catnap;
////using MonoQueue;
using Localytics;
using RestSharp;
using System.Collections.Generic;
using ArtekSoftware.Conference;

namespace ArtekSoftware.Codemash
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		public static AppDelegate CurrentAppDelegate;
		public UINavigationController NavigationController;
		public UISplitViewController splitViewController;
		public INavigation Navigation { get; set; }
		public IAnalytics Analytics { get; set; }
		
		private UIWindow window;
		private TabBarControllerBase _tabBarController;
		//private static IMessageBus _messageBus;
		public static string ConferenceSlug { get { return "MobiDevDay-2012"; } }
		
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{	
			CurrentAppDelegate = this;
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			this.Navigation = new Navigation (window);
			
			this.Analytics = new LocalyticsAnalytics ();
			this.Analytics.Initialize ();
			this.Analytics.Log ("App launched");
			
			ITestFlightProxy testFlightProxy = new TestFlightProxy ();
			testFlightProxy.TakeOff ();
			
			var x = new X ();
			x.Y();
			
			IDefaultDatabaseManager defaultDatabaseManager = new DefaultDatabaseManager (this.Analytics, testFlightProxy);
			defaultDatabaseManager.CopyDefaultDatabase ();
			
//			var catnapBootstrapper = new CatnapBootstrapper (testFlightProxy);
//			catnapBootstrapper.Initialize ();
			
//			var entityMapper = new EntityMapper ();
//			entityMapper.Mapper ();
			
			var databaseMigrationBootstrapper = new DatabaseMigrationBootstrapper ();
			databaseMigrationBootstrapper.Migrate ();
						
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) {
				this.TabBar = new iPhoneTabBarController ();
				this.NavigationController = new UINavigationController (this.TabBar);
				this.NavigationController.NavigationBarHidden = true;
				window.RootViewController = this.NavigationController;
			} else {
				this.TabBar = new TabBarController ();
				var detailViewController = new DetailViewController ();
				splitViewController = new UISplitViewController ();
				splitViewController.Delegate = new SplitDelegate ();
				splitViewController.ViewControllers = new UIViewController[] {
						this.TabBar,
						detailViewController
				};
			
				window.RootViewController = splitViewController;
			}

			window.MakeKeyAndVisible ();
			//var bus = AppDelegate.MessageBus;
			
			return true;
		}
	
//		public static IMessageBus MessageBus {
//			get {
//				if (_messageBus == null)
//				{
//					int checkPendingMessagesMilliseconds = 10000;
//					ISerializer serializer = new Serializer ();
//					INetworkStatusCheck networkStatusCheck = new NetworkStatusCheck();
//					_messageBus = new MessageBus(networkStatusCheck, serializer, checkPendingMessagesMilliseconds);
//				}
//				
//				return _messageBus;
//			}
//		}
		
		public TabBarControllerBase TabBar {
			get { return _tabBarController; }
			set { _tabBarController = value; }
		}
		
		public override void WillEnterForeground (UIApplication application)
		{
			this.Analytics.Resume ();
		}
		
		public override void DidEnterBackground (UIApplication application)
		{
			CloseLocalyticsSession ();
		}
		
		public override void WillTerminate (UIApplication application)
		{
			CloseLocalyticsSession ();
		}
		
		private void CloseLocalyticsSession ()
		{
			this.Analytics.Close ();
		}	
	
	}


	/// EXTREMELY IMPORTANT: You must include a subclass of these classes
	/// due to a known issue in MonoTouch as of 3.2.4.  The issue is expected
	/// to be resolved in the next major release of MonoTouch.
	class SubLocalyticsSession : LocalyticsSession
	{
	}

	class SubUploaderThread : UploaderThread
	{
	}
	class X
	{
		public void Y ()
		{
			var client = new RestClient ();
			client.BaseUrl = "http://conference.apphb.com/api/";
			//http://conference.apphb.com/api/MobiDevDay-2012/sessions?format=json
			var request = new RestRequest ();
			request.Resource = "MobiDevDay-2012/sessions?format=json";
			request.RequestFormat = DataFormat.Json;
			//using (new NetworkIndicator()) {
				var r2 = client.Execute(request);
				var response = client.Execute<List<ArtekSoftware.Conference.Session>> (request);

				var session = new ArtekSoftware.Conference.Session ();
				if (response != null && response.Data != null) {
					//session = response.Data;
				}
			//}
		}
	}
	
}