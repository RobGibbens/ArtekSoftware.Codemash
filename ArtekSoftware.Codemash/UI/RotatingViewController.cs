using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace ArtekSoftware.Codemash
{
	[Register("RotatingViewController")]
	public class RotatingViewController : UIViewController
	{
		public UIViewController LandscapeLeftViewController { get; set; }

		public UIViewController LandscapeRightViewController { get; set; }

		public UIViewController PortraitViewController { get; set; }

		private NSObject notificationObserver;
		
		public RotatingViewController (IntPtr handle) : base(handle)
		{
		}

		[Export("initWithCoder:")]
		public RotatingViewController (NSCoder coder) : base(coder)
		{
		}

		public RotatingViewController (string nibName, NSBundle bundle) : base(nibName, bundle)
		{
		}

		public RotatingViewController ()
		{
		}

		public override void ViewWillAppear (bool animated)
		{
			//Console.WriteLine ("RotatingViewController.ViewWillAppear");
			if (this.InterfaceOrientation == UIInterfaceOrientation.Portrait || this.InterfaceOrientation == UIInterfaceOrientation.PortraitUpsideDown) {
				//Console.WriteLine ("RotatingViewController.ViewWillAppear - IsPortrait");
				_showView (this.PortraitViewController.View);
			} else if (this.InterfaceOrientation == UIInterfaceOrientation.LandscapeLeft) {
				//Console.WriteLine ("RotatingViewController.ViewWillAppear - IsLandscapeLeft");
				_showView (this.LandscapeLeftViewController.View);
			} else if (this.InterfaceOrientation == UIInterfaceOrientation.LandscapeRight) {
				//Console.WriteLine ("RotatingViewController.ViewWillAppear - IsLandscapeRight");
				_showView (this.LandscapeRightViewController.View);
			}
		}

		private void _showView (UIView view)
		{
			//Console.WriteLine ("RotatingViewController._showView - view is null " + (view == null).ToString());
			bool skip = false; //TODO
			if (!skip) {
				if (this.NavigationController != null) {
					//Console.WriteLine ("RotatingViewController._showView - Calling NavigationController.SetNavigationBarHidden");
					NavigationController.SetNavigationBarHidden (view != PortraitViewController.View, false);
				}
				
				//Console.WriteLine ("RotatingViewController._showView - removing all views");
				_removeAllViews ();
				view.Frame = this.View.Frame;
				if (this.InterfaceOrientation == UIInterfaceOrientation.LandscapeLeft || this.InterfaceOrientation == UIInterfaceOrientation.LandscapeRight) {
					//Console.WriteLine ("RotatingViewController._showView - Is Landscape, setting view.Frame");
					view.Frame = new System.Drawing.RectangleF (0, 0, this.View.Frame.Width, this.View.Frame.Height);
				}
				//Console.WriteLine ("RotatingViewController._showView - Adding Subview");
				View.AddSubview (view);
			}

		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}

		public override void ViewDidLoad ()
		{
			//Console.WriteLine ("RotatingViewController.ViewDidLoad");
			notificationObserver = NSNotificationCenter.DefaultCenter
					.AddObserver ("UIDeviceOrientationDidChangeNotification", DeviceRotated);
		}

		public override void ViewDidAppear (bool animated)
		{
			//Console.WriteLine ("RotatingViewController.ViewDidAppear");
			UIDevice.CurrentDevice.BeginGeneratingDeviceOrientationNotifications ();
		}

		public override void ViewWillDisappear (bool animated)
		{
			UIDevice.CurrentDevice.EndGeneratingDeviceOrientationNotifications ();
		}

		private void DeviceRotated (NSNotification notification)
		{
			//Console.WriteLine ("RotatingViewController.DeviceRotated - Orientation is " + this.InterfaceOrientation);
			switch (this.InterfaceOrientation) {

			case  UIInterfaceOrientation.Portrait:
				//Console.WriteLine ("RotatingViewController.DeviceRotated - showing Portrait");
				_showView (PortraitViewController.View);
				break;

			case  UIInterfaceOrientation.PortraitUpsideDown:
				//Console.WriteLine ("RotatingViewController.DeviceRotated - showing Portrait UpsideDown");
				_showView (PortraitViewController.View);
				break;				
				
			case UIInterfaceOrientation.LandscapeLeft:
				//Console.WriteLine ("RotatingViewController.DeviceRotated - showing LandscapeLeft");
				_showView (LandscapeLeftViewController.View);

				break;
			case UIInterfaceOrientation.LandscapeRight:
				//Console.WriteLine ("RotatingViewController.DeviceRotated - showing LandscapeRight");
				_showView (LandscapeRightViewController.View);
				break;
			}
		}

		private void _removeAllViews ()
		{
			//Console.WriteLine ("RotatingViewController._removeAllViews");
			
			PortraitViewController.View.RemoveFromSuperview ();
			LandscapeLeftViewController.View.RemoveFromSuperview ();
			LandscapeRightViewController.View.RemoveFromSuperview ();
		}

		protected void OnDeviceRotated ()
		{

		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			NSNotificationCenter.DefaultCenter.RemoveObserver (notificationObserver);
		}

	}
}

