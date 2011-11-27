using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Linq;

namespace ArtekSoftware.Codemash
{
	public class RotatingSessionDetailViewController : RotatingViewController, ISubstitutableDetailViewController
	{
		private SessionEntity _session;
		UIToolbar toolbar;
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
		
//		[Export("splitViewController:willHideViewController:withBarButtonItem:forPopoverController:")]
//		public void WillHideViewController (UISplitViewController svc, UIViewController vc,
//			UIBarButtonItem barButtonItem, UIPopoverController pc)
//		{
//			if (this.PortraitViewController != null) {
//				((SessionDetailViewController)this.PortraitViewController).WillHideViewController (svc, vc, barButtonItem, pc);
//			}
//
////			if (this.LandscapeLeftViewController != null) {
////				((SessionDetailLandscapeViewController)this.LandscapeLeftViewController).WillHideViewController (svc, vc, barButtonItem, pc);
////			}
////
////			if (this.LandscapeRightViewController != null) {
////				((SessionDetailLandscapeViewController)this.LandscapeRightViewController).WillHideViewController (svc, vc, barButtonItem, pc);
////			}			
//		}
		
//		[Export("splitViewController:willShowViewController:invalidatingBarButtonItem:")]
//		public void WillShowViewController (UISplitViewController svc, UIViewController vc,
//			UIBarButtonItem button)
//		{
//			if (this.PortraitViewController != null) {
//				((SessionDetailViewController)this.PortraitViewController).WillShowViewController (svc, vc, button);
//			}	
//
////			if (this.LandscapeLeftViewController != null) {
////				((SessionDetailLandscapeViewController)this.LandscapeLeftViewController).WillShowViewController (svc, vc, button);
////			}	
////		
////			if (this.LandscapeRightViewController != null) {
////				((SessionDetailLandscapeViewController)this.LandscapeRightViewController).WillShowViewController (svc, vc, button);
////			}
//		}
			
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
		
		UIPopoverController _popOverController;
		UIBarButtonItem _rootPopoverButtonItem;

		public UIPopoverController PopOverController {
			get {
				return this._popOverController;
			}
			set {
				_popOverController = value;
			}
		}
    
		public UIBarButtonItem RootPopoverButtonItem {
			get {
				return this._rootPopoverButtonItem;
			}
			set {
				_rootPopoverButtonItem = value;
			}
		}

		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			this.toolbar = null;
		}

		public void ShowRootPopoverButtonItem (UIBarButtonItem barButtonItem)
		{
			// Add the popover button to the toolbar.
			if (this.InterfaceOrientation == UIInterfaceOrientation.Portrait || this.InterfaceOrientation == UIInterfaceOrientation.PortraitUpsideDown) {
				toolbar = ((SessionDetailViewController)this.PortraitViewController).Toolbar;
				toolbar.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile ("images/SessionsHeader3.png"));
			}
			
			if (toolbar != null) {
				var itemsArray = this.toolbar.Items.ToList ();
				//NSMutableArray *itemsArray = [toolbar.items mutableCopy];
      
				if (itemsArray.Count == 0) {
					itemsArray.Insert (0, barButtonItem);
				} else {
					itemsArray [0] = barButtonItem;
				}
				//[itemsArray insertObject:barButtonItem atIndex:0];
      
				this.toolbar.SetItems (itemsArray.ToArray (), false);
				//[toolbar setItems:itemsArray animated:NO];
      
				itemsArray = null;
				//[itemsArray release];
			}
		}

		public void InvalidateRootPopoverButtonItem (UIBarButtonItem barButtonItem)
		{
			if (this.InterfaceOrientation == UIInterfaceOrientation.Portrait || this.InterfaceOrientation == UIInterfaceOrientation.PortraitUpsideDown) {
				toolbar = ((SessionDetailViewController)this.PortraitViewController).Toolbar;
			}
			
			if (toolbar != null) {
				// Remove the popover button from the toolbar.
				var itemsArray = this.toolbar.Items.ToList ();
				//NSMutableArray *itemsArray = [toolbar.items mutableCopy];
      
				itemsArray.Remove (barButtonItem);
				//[itemsArray removeObject:barButtonItem];
      
				this.toolbar.SetItems (itemsArray.ToArray (), false);
				//[toolbar setItems:itemsArray animated:NO];
      
				itemsArray = null;
				//[itemsArray release];  
			}
		}			
	}
}