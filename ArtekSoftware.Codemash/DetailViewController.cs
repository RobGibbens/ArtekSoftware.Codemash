using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace ArtekSoftware.Codemash
{
	public partial class DetailViewController : UIViewController, ISubstitutableDetailViewController
	{
		object detailItem;
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
		
		public DetailViewController () : base ("DetailViewController", null)
		{
			this.Title = NSBundle.MainBundle.LocalizedString ("Detail", "Detail");
		}
		
		public void SetDetailItem (object newDetailItem)
		{
			if (detailItem != newDetailItem) {
				detailItem = newDetailItem;
				
				//Update the view
				ConfigureView ();
			}
			
			if (this._popOverController != null) {
				this._popOverController.Dismiss (true);
			}
		}
		
		void ConfigureView ()
		{
			// Update the user interface for the detail item
			if (detailItem != null) {
				this.detailDescriptionLabel.Text = detailItem.ToString ();
			}
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}
		
		[Export("splitViewController:willHideViewController:withBarButtonItem:forPopoverController:")]
		public void WillHideViewController (UISplitViewController svc, UIViewController vc,
			UIBarButtonItem barButtonItem, UIPopoverController pc)
		{
			barButtonItem.Title = "Master";
			var items = new List<UIBarButtonItem> ();
			items.Add (barButtonItem);
			items.AddRange (toolbar.Items);
			toolbar.SetItems (items.ToArray (), true);
			_popOverController = pc;
		}
		
		[Export("splitViewController:willShowViewController:invalidatingBarButtonItem:")]
		public void WillShowViewController (UISplitViewController svc, UIViewController vc,
			UIBarButtonItem button)
		{
			// Called when the view is shown again in the split view, invalidating the button and popover controller.
			var items = new List<UIBarButtonItem> (toolbar.Items);
			items.RemoveAt (0);
			toolbar.SetItems (items.ToArray (), true);
			_popOverController = null;
		}
	
	
	}
}
