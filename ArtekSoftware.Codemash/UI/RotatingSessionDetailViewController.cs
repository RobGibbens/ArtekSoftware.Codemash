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
			//Console.WriteLine("RotatingSessionDetailViewController.ctor(session)");
			_session = session;
			LoadSession ();
		}
		
		public RotatingSessionDetailViewController ()
		{
			//Console.WriteLine("RotatingSessionDetailViewController.ctor()");
			LoadSession ();
		}
		
		public void SetSession (SessionEntity session)
		{
			//Console.WriteLine("RotatingSessionDetailViewController.SetSession - Session is null " + (session == null).ToString());
			_session = session;
			LoadSession ();
		}
		
		public override void ViewDidLoad ()
		{
			//Console.WriteLine("RotatingSessionDetailViewController.ViewDidLoad");
			base.ViewDidLoad ();
			

			if (_session != null) {
				LoadSession ();
			}

		}
			
		private void LoadSession ()
		{
			//Console.WriteLine("RotatingSessionDetailViewController.LoadSession");
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
				//Console.WriteLine("RotatingSessionDetailViewController.PopOverController.get - Is null " + (_popOverController == null).ToString());
				return this._popOverController;
			}
			set {
				//Console.WriteLine("RotatingSessionDetailViewController.PopOverController.set - Is null " + (value == null).ToString());
				_popOverController = value;
			}
		}
    
		public UIBarButtonItem RootPopoverButtonItem {
			get {
				//Console.WriteLine("RotatingSessionDetailViewController.RootPopoverButtonItem.get - Is null " + (_rootPopoverButtonItem == null).ToString());
				return this._rootPopoverButtonItem;
			}
			set {
				//Console.WriteLine("RotatingSessionDetailViewController.RootPopoverButtonItem.set - Is null " + (value == null).ToString());
				_rootPopoverButtonItem = value;
			}
		}

		public override void ViewDidUnload ()
		{
			//Console.WriteLine("RotatingSessionDetailViewController.ViewDidUnload");
			base.ViewDidUnload ();
			this.toolbar = null;
		}

		public void ShowRootPopoverButtonItem (UIBarButtonItem barButtonItem)
		{
			//Console.WriteLine("RotatingSessionDetailViewController.ShowRootPopoverButtonItem - barButtonItem is null " + (barButtonItem == null).ToString());
			// Add the popover button to the toolbar.
			//if (this.InterfaceOrientation == UIInterfaceOrientation.Portrait || this.InterfaceOrientation == UIInterfaceOrientation.PortraitUpsideDown) {
				//Console.WriteLine("RotatingSessionDetailViewController.ShowRootPopoverButtonItem - IsInPortrait");
				toolbar = ((SessionDetailViewController)this.PortraitViewController).Toolbar;
				//Console.WriteLine("RotatingSessionDetailViewController.ShowRootPopoverButtonItem - IsInPortrait - toolbar is null " + (toolbar == null).ToString());
				toolbar.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile ("images/SessionsHeader3.png"));
			//}
			
			if (toolbar != null) {
				//Console.WriteLine("RotatingSessionDetailViewController.ShowRootPopoverButtonItem - toolbar is null " + (toolbar == null).ToString());
				var itemsArray = this.toolbar.Items.ToList ();
				//NSMutableArray *itemsArray = [toolbar.items mutableCopy];
      
				if (itemsArray.Count == 0) {
					//Console.WriteLine("RotatingSessionDetailViewController.RootPopoverButtonItem - itemsArray.Count == 0");
					itemsArray.Insert (0, barButtonItem);
				} else {
					//Console.WriteLine("RotatingSessionDetailViewController.RootPopoverButtonItem - itemsArray.Count <> 0");
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
			//Console.WriteLine("RotatingSessionDetailViewController.InvalidateRootPopoverButtonItem - barButtonItem is null " + (barButtonItem == null).ToString());
			
			if (this.InterfaceOrientation == UIInterfaceOrientation.Portrait || this.InterfaceOrientation == UIInterfaceOrientation.PortraitUpsideDown) {
				toolbar = ((SessionDetailViewController)this.PortraitViewController).Toolbar;
				//Console.WriteLine("RotatingSessionDetailViewController.InvalidateRootPopoverButtonItem - IsInPortrait - toolbar is null" + (toolbar == null).ToString());
			}
			
			if (toolbar != null) {
				// Remove the popover button from the toolbar.
				var itemsArray = this.toolbar.Items.ToList ();
				//NSMutableArray *itemsArray = [toolbar.items mutableCopy];
      			//Console.WriteLine("RotatingSessionDetailViewController.InvalidateRootPopoverButtonItem - Removing barButtonItem");
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