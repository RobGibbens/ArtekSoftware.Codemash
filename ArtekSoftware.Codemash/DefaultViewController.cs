using MonoTouch.UIKit;
using System.Drawing;
using System;
using MonoTouch.Foundation;
using System.Linq;

namespace ArtekSoftware.Codemash
{
	public partial class DefaultViewController : UIViewController, ISubstitutableDetailViewController
	{
		UIToolbar toolbar; //TODO
		public DefaultViewController () : base ("DefaultViewController", null)
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
			base.ViewDidLoad ();
			
			//any additional setup after loading the view, typically from a nib.
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return true;
		}
		
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

		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			this.toolbar = null;
		}

		public void ShowRootPopoverButtonItem (UIBarButtonItem barButtonItem)
		{
			// Add the popover button to the toolbar.
      
			var itemsArray = this.toolbar.Items.ToList ();
			//NSMutableArray *itemsArray = [toolbar.items mutableCopy];
      
			if (itemsArray.Count == 0) {
				itemsArray.Insert (0, barButtonItem);
			} else {
				itemsArray [0] = barButtonItem;
			}
			//[itemsArray insertObject:barButtonItem atIndex:0];
      
			this.toolbar.SetItems (itemsArray.ToArray (), false);
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

