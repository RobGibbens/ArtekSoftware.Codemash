using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace ArtekSoftware.Codemash
{
	public class RotatingSessionDetailViewController : RotatingViewController
	{
		private SessionEntity _session;
		
		public RotatingSessionDetailViewController (SessionEntity session) : this()
		{
			_session = session;
			LoadSession ();
		}
		
		public RotatingSessionDetailViewController ()
		{
			LoadSession ();
		}
		
		public void SetSession (SessionEntity session)
		{
			_session = session;
			LoadSession ();
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			if (_session != null) {
				LoadSession ();
			}
		}
		
		[Export("splitViewController:willHideViewController:withBarButtonItem:forPopoverController:")]
		public void WillHideViewController (UISplitViewController svc, UIViewController vc,
			UIBarButtonItem barButtonItem, UIPopoverController pc)
		{
			if (this.PortraitViewController != null) {
				((SessionDetailViewController)this.PortraitViewController).WillHideViewController (svc, vc, barButtonItem, pc);
			}

//			if (this.LandscapeLeftViewController != null) {
//				((SessionDetailLandscapeViewController)this.LandscapeLeftViewController).WillHideViewController (svc, vc, barButtonItem, pc);
//			}
//
//			if (this.LandscapeRightViewController != null) {
//				((SessionDetailLandscapeViewController)this.LandscapeRightViewController).WillHideViewController (svc, vc, barButtonItem, pc);
//			}			
		}
		
		[Export("splitViewController:willShowViewController:invalidatingBarButtonItem:")]
		public void WillShowViewController (UISplitViewController svc, UIViewController vc,
			UIBarButtonItem button)
		{
			if (this.PortraitViewController != null) {
				((SessionDetailViewController)this.PortraitViewController).WillShowViewController (svc, vc, button);
			}	

//			if (this.LandscapeLeftViewController != null) {
//				((SessionDetailLandscapeViewController)this.LandscapeLeftViewController).WillShowViewController (svc, vc, button);
//			}	
//		
//			if (this.LandscapeRightViewController != null) {
//				((SessionDetailLandscapeViewController)this.LandscapeRightViewController).WillShowViewController (svc, vc, button);
//			}
		}
		
		public UIView View {
			get {
				UIView currentView = null;
				switch (this.InterfaceOrientation) {
				case UIInterfaceOrientation.Portrait:
					currentView = this.PortraitViewController.View;
					break;
				case UIInterfaceOrientation.PortraitUpsideDown:
					currentView = this.PortraitViewController.View;
					break;
				case UIInterfaceOrientation.LandscapeLeft:
					currentView = this.LandscapeLeftViewController.View;
					break;
				case UIInterfaceOrientation.LandscapeRight:
					currentView = this.LandscapeRightViewController.View;
					break;
				default:
					currentView = this.LandscapeLeftViewController.View;
					break;
				}	
				return currentView;
			}
			
		}
			
		private void LoadSession ()
		{
			if (_session != null) {
				this.PortraitViewController = new SessionDetailViewController (_session);
				this.LandscapeLeftViewController = new SessionDetailLandscapeViewController (_session);
				this.LandscapeRightViewController = new SessionDetailLandscapeViewController (_session);
				
			} else {
				this.PortraitViewController = new SessionDetailViewController ();
				this.LandscapeLeftViewController = new SessionDetailLandscapeViewController ();
				this.LandscapeRightViewController = new SessionDetailLandscapeViewController ();
			}
		}
	}
}