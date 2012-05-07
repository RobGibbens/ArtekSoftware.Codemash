using System;
using MonoTouch.UIKit;
using System.Linq;
using ArtekSoftware.Conference;

namespace ArtekSoftware.Codemash
{
	public class RotatingLocationMapViewController : RotatingViewController, ISubstitutableDetailViewController
	{
		UIToolbar toolbar;
		UIPopoverController _popOverController;
		UIBarButtonItem _rootPopoverButtonItem;
		
		public RotatingLocationMapViewController ()
		{
			LoadMap ();
		}
		
		public UIPopoverController PopOverController {
			get {
				//Console.WriteLine("RotatingLocationMapViewController.PopOverController.get - Is null " + (_popOverController == null).ToString());
				return this._popOverController;
			}
			set {
				//Console.WriteLine("RotatingLocationMapViewController.PopOverController.set - Is null " + (value == null).ToString());
				_popOverController = value;
			}
		}
    
		public UIBarButtonItem RootPopoverButtonItem {
			get {
				//Console.WriteLine("RotatingLocationMapViewController.RootPopoverButtonItem.get - Is null " + (_rootPopoverButtonItem == null).ToString());
				return this._rootPopoverButtonItem;
			}
			set {
				//Console.WriteLine("RotatingLocationMapViewController.RootPopoverButtonItem.set - Is null " + (value == null).ToString());
				_rootPopoverButtonItem = value;
			}
		}
		
		public override void ViewDidUnload ()
		{
			//Console.WriteLine("RotatingLocationMapViewController.ViewDidUnload");
			base.ViewDidUnload ();
			this.toolbar = null;
		}
		
		private void LoadMap ()
		{
			//Console.WriteLine("RotatingLocationMapViewController.LoadSession");

			this.PortraitViewController = new MapFlipViewController ();
			this.LandscapeLeftViewController = new MapFlipLandscapeViewController ();
			this.LandscapeRightViewController = new MapFlipLandscapeViewController ();
		}
		
		public void ShowRootPopoverButtonItem (UIBarButtonItem barButtonItem)
		{
			ITestFlightProxy testFlight = new TestFlightProxy();
			
			testFlight.PassCheckpoint ("RotatingLocationMapViewController.ShowRootPopoverButtonItem - 1");
			
			toolbar = ((IMapFlipPadViewController)this.PortraitViewController).Toolbar;
			testFlight.PassCheckpoint ("RotatingLocationMapViewController.ShowRootPopoverButtonItem - 2");
			
			toolbar.BackgroundColor = UIColor.FromPatternImage (UIImage.FromFile ("images/SessionsHeader3.png"));
			testFlight.PassCheckpoint ("RotatingLocationMapViewController.ShowRootPopoverButtonItem - 3");
			
			
			if (toolbar != null) {
				testFlight.PassCheckpoint ("RotatingLocationMapViewController.ShowRootPopoverButtonItem - 4");
			
				var itemsArray = this.toolbar.Items.ToList ();
				testFlight.PassCheckpoint ("RotatingLocationMapViewController.ShowRootPopoverButtonItem - 5");
				
				if (itemsArray.Count == 0) {
					testFlight.PassCheckpoint ("RotatingLocationMapViewController.ShowRootPopoverButtonItem - 6");
					//Console.WriteLine("RotatingLocationMapViewController.RootPopoverButtonItem - itemsArray.Count == 0");
					itemsArray.Insert (0, barButtonItem);
					testFlight.PassCheckpoint ("RotatingLocationMapViewController.ShowRootPopoverButtonItem - 7");
				} else {
					testFlight.PassCheckpoint ("RotatingLocationMapViewController.ShowRootPopoverButtonItem - 8");
					//Console.WriteLine("RotatingLocationMapViewController.RootPopoverButtonItem - itemsArray.Count <> 0");
					itemsArray [0] = barButtonItem;
					testFlight.PassCheckpoint ("RotatingLocationMapViewController.ShowRootPopoverButtonItem - 9");
				}
				//[itemsArray insertObject:barButtonItem atIndex:0];
				testFlight.PassCheckpoint ("RotatingLocationMapViewController.ShowRootPopoverButtonItem - 10");
      
				this.toolbar.SetItems (itemsArray.ToArray (), false);
				testFlight.PassCheckpoint ("RotatingLocationMapViewController.ShowRootPopoverButtonItem - 11");
				
				//[toolbar setItems:itemsArray animated:NO];
      
				itemsArray = null;
				testFlight.PassCheckpoint ("RotatingLocationMapViewController.ShowRootPopoverButtonItem - 12");
				
				//[itemsArray release];
			}
		}

		public void InvalidateRootPopoverButtonItem (UIBarButtonItem barButtonItem)
		{
			//Console.WriteLine("RotatingLocationMapViewController.InvalidateRootPopoverButtonItem - barButtonItem is null " + (barButtonItem == null).ToString());
			
			if (this.InterfaceOrientation == UIInterfaceOrientation.Portrait || this.InterfaceOrientation == UIInterfaceOrientation.PortraitUpsideDown) {
				toolbar = ((IMapFlipPadViewController)this.PortraitViewController).Toolbar;
				//Console.WriteLine("RotatingLocationMapViewController.InvalidateRootPopoverButtonItem - IsInPortrait - toolbar is null" + (toolbar == null).ToString());
			}
			
			if (toolbar != null) {
				var itemsArray = this.toolbar.Items.ToList ();
				itemsArray.Remove (barButtonItem);
				this.toolbar.SetItems (itemsArray.ToArray (), false);
				itemsArray = null;
			}
		}
		
	}
}

