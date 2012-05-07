using System.Drawing;
using MonoTouch.UIKit;

namespace ArtekSoftware.Codemash
{
	public partial class iPhoneMapViewController : UIViewController
	{
		public iPhoneMapViewController () : base ("iPhoneMapViewController", null)
		{
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			this.scrollView.Frame = new RectangleF (0, 0, 320, 460);
			this.scrollView.ContentSize = new SizeF (1000, 1000);
			
			this.scrollView.MinimumZoomScale = 1.0f;
			this.scrollView.MaximumZoomScale = 5.0f;
			
			this.scrollView.ViewForZoomingInScrollView = delegate(UIScrollView scrollView) {
				return this.mapImage;
			};
		}
		
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return false;
		}
	}
}

