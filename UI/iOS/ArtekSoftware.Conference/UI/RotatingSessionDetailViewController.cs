using System.Linq;
using MonoTouch.UIKit;
using ArtekSoftware.Conference;
using ArtekSoftware.Conference.LocalData;

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
			ITestFlightProxy testFlight = new TestFlightProxy();
			
			testFlight.PassCheckpoint ("RotatingSessionDetailViewController.ShowRootPopoverButtonItem - 1");
			
			toolbar = ((SessionDetailViewController)this.PortraitViewController).Toolbar;
			testFlight.PassCheckpoint ("RotatingSessionDetailViewController.ShowRootPopoverButtonItem - 2");
			
			toolbar.BackgroundColor = UIColor.FromPatternImage (UIImage.FromFile ("images/SessionsHeader3.png"));
			testFlight.PassCheckpoint ("RotatingSessionDetailViewController.ShowRootPopoverButtonItem - 3");
			
			
			if (toolbar != null) {
				testFlight.PassCheckpoint ("RotatingSessionDetailViewController.ShowRootPopoverButtonItem - 4");
			
				var itemsArray = this.toolbar.Items.ToList ();
				testFlight.PassCheckpoint ("RotatingSessionDetailViewController.ShowRootPopoverButtonItem - 5");
				
				if (itemsArray.Count == 0) {
					testFlight.PassCheckpoint ("RotatingSessionDetailViewController.ShowRootPopoverButtonItem - 6");
					//Console.WriteLine("RotatingSessionDetailViewController.RootPopoverButtonItem - itemsArray.Count == 0");
					itemsArray.Insert (0, barButtonItem);
					testFlight.PassCheckpoint ("RotatingSessionDetailViewController.ShowRootPopoverButtonItem - 7");
				} else {
					testFlight.PassCheckpoint ("RotatingSessionDetailViewController.ShowRootPopoverButtonItem - 8");
					//Console.WriteLine("RotatingSessionDetailViewController.RootPopoverButtonItem - itemsArray.Count <> 0");
					itemsArray [0] = barButtonItem;
					testFlight.PassCheckpoint ("RotatingSessionDetailViewController.ShowRootPopoverButtonItem - 9");
				}
				//[itemsArray insertObject:barButtonItem atIndex:0];
				testFlight.PassCheckpoint ("RotatingSessionDetailViewController.ShowRootPopoverButtonItem - 10");
      
				this.toolbar.SetItems (itemsArray.ToArray (), false);
				testFlight.PassCheckpoint ("RotatingSessionDetailViewController.ShowRootPopoverButtonItem - 11");
				
				//[toolbar setItems:itemsArray animated:NO];
      
				itemsArray = null;
				testFlight.PassCheckpoint ("RotatingSessionDetailViewController.ShowRootPopoverButtonItem - 12");
				
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
				var itemsArray = this.toolbar.Items.ToList ();
				itemsArray.Remove (barButtonItem);
				this.toolbar.SetItems (itemsArray.ToArray (), false);
				itemsArray = null;
			}
		}			
	}
}