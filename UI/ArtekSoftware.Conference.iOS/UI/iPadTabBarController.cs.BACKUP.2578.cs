using System;
using System.Linq;
<<<<<<< HEAD
using Catnap;
=======
using MonoTouch.UIKit;
>>>>>>> develop

namespace ArtekSoftware.Codemash
{
	public abstract class TabBarControllerBase : UITabBarController
	{
	}
	
	public class iPadTabBarController : TabBarControllerBase
	{
		MonoTouch.UIKit.UINavigationController navScheduleController, navSessionController, navSpeakerController, mapController, settingsController;
		
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
			
			var settingsViewController = GetSettingsViewController ();
			
			settingsController = new MonoTouch.UIKit.UINavigationController ();
			settingsController.PushViewController (settingsViewController, false);
			SetSessionBackground (settingsController.NavigationBar);
			settingsController.NavigationBar.BarStyle = UIBarStyle.Black;
			settingsController.NavigationBar.BackgroundColor = UIColor.Clear;
			settingsController.NavigationBar.TintColor = UIColor.Clear;
			settingsController.TabBarItem = new UITabBarItem ("Settings", UIImage.FromFile ("images/glyphicons_019_cogwheel.png"), 0);		
			
			
			var u = new UIViewController[]{
				navScheduleController, navSessionController, navSpeakerController, mapController, settingsController};
			
			this.ViewControllers = u;
			
			this.SelectedViewController = navSessionController;
			
			this.MoreNavigationController.NavigationBar.BarStyle = UIBarStyle.Black;
		}
		
		public void tapme ()
		{
			iPadTabBarController._loginBindingContext.Fetch ();
			
			if (UnitOfWork.IsUnitOfWorkStarted ()) {
				var settingsRepository = new LocalSettingsRepository ();
				var settings = settingsRepository.Find ().FirstOrDefault ();
				if (settings == null) {
					settings = new SettingsEntity ();
				}
				settings.LoginName = iPadTabBarController._login.login;
				settings.Password = iPadTabBarController._login.password;
			
				settingsRepository.Save (settings);
			} else {
				using (UnitOfWork.Start()) {
					var settingsRepository = new LocalSettingsRepository ();
					var settings = settingsRepository.Find ().FirstOrDefault ();
					if (settings == null) {
						settings = new SettingsEntity ();
					}
					settings.LoginName = iPadTabBarController._login.login;
					settings.Password = iPadTabBarController._login.password;
			
					settingsRepository.Save (settings);
				}
			}
		}
		
		public static SettingsViewModel _login;
		public static BindingContext _loginBindingContext;

		private DialogViewController GetSettingsViewController ()
		{
			string login = string.Empty;
			string password = string.Empty;
			bool shouldSync = false;
			
			if (UnitOfWork.IsUnitOfWorkStarted ()) {
				var settingsRepository = new LocalSettingsRepository ();
				
				var settings = settingsRepository.Find ().FirstOrDefault ();
				if (settings != null) {
					login = settings.LoginName;
					password = settings.Password;
					shouldSync = Convert.ToBoolean (settings.ShouldSync);
				}
				
			} else {
				using (UnitOfWork.Start()) {
					var settingsRepository = new LocalSettingsRepository ();
					var settings = settingsRepository.Find ().FirstOrDefault ();
					
					if (settings != null) {
						login = settings.LoginName;
						password = settings.Password;
						shouldSync = Convert.ToBoolean (settings.ShouldSync);
					}
					
				}
			}
			
			_login = new SettingsViewModel ()
			{
				login = login,
				password = password,
			};
			
			_loginBindingContext = new BindingContext (this, _login, "Login");
			
			var dialogcontroller = new DialogViewController (_loginBindingContext.Root);

			return dialogcontroller;
			
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) {
				return false;
			} else {
				return true;
			}
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

