using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.IO;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreLocation;

namespace ArtekSoftware.Codemash
{
	public class iPhoneMapFlipViewController : UIViewController, IMapFlipViewController
	{
		MapKitViewController mapView;
		MapLocationViewController locationView;
		
		public override void ViewDidLoad ()
        {
			base.ViewDidLoad ();

			
			float width = 282;
			float height = 30;
			var x = (this.View.Frame.Width - width) / 2;
			var y = (this.View.Frame.Height - 120);
			Console.WriteLine("FRAME HEIGHT " + this.View.Frame.Height);
			Console.WriteLine("BOUNDS HEIGHT" + this.View.Bounds.Height);			
			RectangleF segmentedControlFrame = new RectangleF(x, y, width, height);
			mapView = new MapKitViewController (this, segmentedControlFrame);
			mapView.View.Frame = new RectangleF (0, 0, this.View.Frame.Width, this.View.Frame.Height);
			
			this.View.AddSubview(mapView.View);
		}
		
		public void Flip(CLLocationCoordinate2D toLocation)
		{
			mapView.SetLocation(toLocation); // assume not null, since it's created in ViewDidLoad ??
			
			Flip();
		}
		
		public void Flip()
		{
			// lazy load the non-default view
			if (locationView == null)
			{
				locationView = new MapLocationViewController(this); 
				locationView.View.Frame = new RectangleF(0,0,this.View.Frame.Width, this.View.Frame.Height);
			}
			Console.WriteLine("Flip");
			
			UIView.BeginAnimations("Flipper");
			UIView.SetAnimationDuration(1.25);
			UIView.SetAnimationCurve(UIViewAnimationCurve.EaseInOut);
			if (mapView.View.Superview == null)
			{
				Console.WriteLine("to map");
				UIView.SetAnimationTransition (UIViewAnimationTransition.FlipFromRight, this.View, true);
				locationView.ViewWillAppear(true);
				mapView.ViewWillDisappear(true);
				
				locationView.View.RemoveFromSuperview();
				this.View.AddSubview(mapView.View);
				
				mapView.ViewDidDisappear(true);
				locationView.ViewDidAppear(true);
			}
			else
			{
				Console.WriteLine("to list");
				UIView.SetAnimationTransition (UIViewAnimationTransition.FlipFromLeft, this.View, true);
				mapView.ViewWillAppear(true);
				locationView.ViewWillDisappear(true);
				
				mapView.View.RemoveFromSuperview();
				this.View.AddSubview(locationView.View);
				
				locationView.ViewDidDisappear(true);
				mapView.ViewDidAppear(true);
			}
			UIView.CommitAnimations();
		}
	}
}
