using System;
using MonoTouch.UIKit;
using System.Linq;

namespace ArtekSoftware.Codemash
{
	public class RotatingSpeakerBioViewController : RotatingViewController, ISubstitutableDetailViewController
	{
		private SpeakerEntity _speaker;
		UIToolbar toolbar;
		public RotatingSpeakerBioViewController (SpeakerEntity speaker) : this()
		{
			_speaker = speaker;
			LoadSpeaker ();
		}
		
		public RotatingSpeakerBioViewController ()
		{
			LoadSpeaker ();
		}
		
		public void SetSpeaker (SpeakerEntity speaker)
		{
			_speaker = speaker;
			LoadSpeaker ();
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			if (_speaker != null) {
				LoadSpeaker ();
			}
		}

		private void LoadSpeaker ()
		{
			if (_speaker != null) {
				this.PortraitViewController = new SpeakerBioViewController (_speaker);			
				this.LandscapeLeftViewController = new SpeakerBioLandscapeViewController (_speaker);
				this.LandscapeRightViewController = new SpeakerBioLandscapeViewController (_speaker);
			} else {
				this.PortraitViewController = new SpeakerBioViewController ();			
				this.LandscapeLeftViewController = new SpeakerBioLandscapeViewController ();
				this.LandscapeRightViewController = new SpeakerBioLandscapeViewController ();
			}
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
			if (this.InterfaceOrientation == UIInterfaceOrientation.Portrait || this.InterfaceOrientation == UIInterfaceOrientation.PortraitUpsideDown) {
				toolbar = ((SpeakerBioViewController)this.PortraitViewController).Toolbar;
				toolbar.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile ("images/SessionsHeader3.png"));
			}
			
			if (toolbar != null) {
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
		}

		public void InvalidateRootPopoverButtonItem (UIBarButtonItem barButtonItem)
		{
			if (this.InterfaceOrientation == UIInterfaceOrientation.Portrait || this.InterfaceOrientation == UIInterfaceOrientation.PortraitUpsideDown) {
				toolbar = ((SpeakerBioViewController)this.PortraitViewController).Toolbar;
			}
			
			if (toolbar != null) {
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
}