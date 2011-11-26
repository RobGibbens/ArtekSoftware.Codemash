using System;
using MonoTouch.UIKit;

namespace ArtekSoftware.Codemash
{
	public class MapViewController : UIViewController
	{
		public override void ViewDidLoad ()
		{
			var sv = new UIScrollView (this.View.Frame);
			var image = UIImage.FromFile ("images/KalahariMap.png");
			var iv = new UIImageView (image);

			sv.AddSubview (iv);
			sv.ContentSize = iv.Frame.Size;
			sv.MinimumZoomScale = 1.0f;
			sv.MaximumZoomScale = 3.0f;
			sv.MultipleTouchEnabled = true;
			sv.ViewForZoomingInScrollView = delegate(UIScrollView scrollView) {
				return iv;
			};
			
			this.View.AddSubview(sv);			
			
			base.ViewDidLoad ();
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}
	}
}