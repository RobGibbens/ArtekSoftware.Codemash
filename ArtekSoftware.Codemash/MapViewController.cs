using MonoTouch.UIKit;
using System.Drawing;
using System;
using MonoTouch.Foundation;
using System.Linq;

namespace ArtekSoftware.Codemash
{
	public partial class MapViewController : UIViewController, ISubstitutableDetailViewController
	{
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
		
		public MapViewController () : base ("MapViewController", null)
		{
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
		}
		
		public override void ViewDidLoad ()
		{
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
			
      this.toolbar = null;
			// Release any retained subviews of the main view.
			// e.g. this.myOutlet = null;
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}
		
public void ShowRootPopoverButtonItem (UIBarButtonItem barButtonItem)
    {
      // Add the popover button to the toolbar.
      
      var itemsArray = this.toolbar.Items.ToList();
      //NSMutableArray *itemsArray = [toolbar.items mutableCopy];
      
      if (itemsArray.Count == 0)
      {
        itemsArray.Insert(0, barButtonItem);
      }
      else
      {
        itemsArray[0] = barButtonItem;
      }
      //[itemsArray insertObject:barButtonItem atIndex:0];
      
      this.toolbar.SetItems (itemsArray.ToArray(), false);
      //[toolbar setItems:itemsArray animated:NO];
      
      itemsArray = null;
      //[itemsArray release];      
    }

    public void InvalidateRootPopoverButtonItem (UIBarButtonItem barButtonItem)
    {
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

