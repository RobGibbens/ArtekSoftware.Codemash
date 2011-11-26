using MonoTouch.UIKit;
using System.Drawing;
using System;
using MonoTouch.Foundation;

namespace ArtekSoftware.Codemash
{
	public partial class MapViewController : UIViewController
	{
		public MapViewController () : base ("MapViewController", null)
		{
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		
		public override void ViewDidLoad ()
		{
			//var sv = new UIScrollView (this.View.Frame);
			//var image = UIImage.FromFile ("images/KalahariMap.png");
			//var iv = new UIImageView (image);

			//sv.AddSubview (iv);
			//sv.ContentSize = iv.Frame.Size;
			this.scrollView.MinimumZoomScale = 1.0f;
			this.scrollView.MaximumZoomScale = 5.0f;
			
			this.scrollView.ViewForZoomingInScrollView = delegate(UIScrollView scrollView) {
				return this.imageView;
			};
			
			//this.View.AddSubview(sv);			
			
			base.ViewDidLoad ();
		}
		
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			
			// Release any retained subviews of the main view.
			// e.g. this.myOutlet = null;
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}
	}
}

