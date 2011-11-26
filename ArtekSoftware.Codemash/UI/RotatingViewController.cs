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
			
			if (this.InterfaceOrientation == UIInterfaceOrientation.Portrait || this.InterfaceOrientation == UIInterfaceOrientation.PortraitUpsideDown) {
				_showView (this.PortraitViewController.View);
			} else if (this.InterfaceOrientation == UIInterfaceOrientation.LandscapeLeft) {
				_showView (this.LandscapeLeftViewController.View);
			} else if (this.InterfaceOrientation == UIInterfaceOrientation.LandscapeRight) {
				_showView (this.LandscapeRightViewController.View);
			}
		}

		private void _showView (UIView view)
		{

			if (this.NavigationController != null)
				NavigationController.SetNavigationBarHidden (view != PortraitViewController.View, false);

			_removeAllViews ();
			view.Frame = this.View.Frame;
//			view.Frame.Y = 0;
//			view.Frame.Height = view.Frame.Height + 20;
			View.AddSubview (view);

		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}

		public override void ViewDidLoad ()
		{
			notificationObserver = NSNotificationCenter.DefaultCenter
					.AddObserver ("UIDeviceOrientationDidChangeNotification", DeviceRotated);
		}

		public override void ViewDidAppear (bool animated)
		{
			UIDevice.CurrentDevice.BeginGeneratingDeviceOrientationNotifications ();
		}

		public override void ViewWillDisappear (bool animated)
		{
			UIDevice.CurrentDevice.EndGeneratingDeviceOrientationNotifications ();
		}

		private void DeviceRotated (NSNotification notification)
		{

			Console.WriteLine ("rotated! " + this.InterfaceOrientation);
			switch (this.InterfaceOrientation) {

			case  UIInterfaceOrientation.Portrait:
				_showView (PortraitViewController.View);
				break;

			case  UIInterfaceOrientation.PortraitUpsideDown:
				_showView (PortraitViewController.View);
				break;				
				
			case UIInterfaceOrientation.LandscapeLeft:
				_showView (LandscapeLeftViewController.View);

				break;
			case UIInterfaceOrientation.LandscapeRight:
				_showView (LandscapeRightViewController.View);
				break;
			}
		}

		private void _removeAllViews ()
		{
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

