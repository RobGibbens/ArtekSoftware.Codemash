using System;
using System.Linq;
using MonoTouch.UIKit;
using ArtekSoftware.Conference;
using ArtekSoftware.Conference.LocalData;

namespace ArtekSoftware.Codemash
{
	public class RotatingSpeakerBioViewController : RotatingViewController, ISubstitutableDetailViewController
	{
		private SpeakerEntity _speaker;
		UIToolbar toolbar;
		
		public RotatingSpeakerBioViewController (SpeakerEntity speaker) : this()
		{
			Console.WriteLine("RotatingSpeakerBioViewController.ctor - speaker");
			_speaker = speaker;
			LoadSpeaker ();
		}
		
		public RotatingSpeakerBioViewController ()
		{
			Console.WriteLine("RotatingSpeakerBioViewController.ctor");
			LoadSpeaker ();
		}
		
		public void SetSpeaker (SpeakerEntity speaker)
		{
			Console.WriteLine("RotatingSpeakerBioViewController.SetSpeaker");
			_speaker = speaker;
			LoadSpeaker ();
		}
		
		public override void ViewDidLoad ()
		{
			Console.WriteLine("RotatingSpeakerBioViewController.ViewDidLoad");
			base.ViewDidLoad ();
			
			if (_speaker != null) {
				LoadSpeaker ();
			}
		}

		private void LoadSpeaker ()
		{
			Console.WriteLine("RotatingSpeakerBioViewController.LoadSpeaker");
			
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
			Console.WriteLine("RotatingSpeakerBioViewController.ViewDidUnload");
			base.ViewDidUnload ();
			this.toolbar = null;
		}

		public void ShowRootPopoverButtonItem (UIBarButtonItem barButtonItem)
		{
			ITestFlightProxy testFlight = new TestFlightProxy();
			
			testFlight.PassCheckpoint ("RotatingSpeakerBioViewController.ShowRootPopoverButtonItem - 1");
			
			toolbar = ((SpeakerBioViewController)this.PortraitViewController).Toolbar;
			testFlight.PassCheckpoint ("RotatingSpeakerBioViewController.ShowRootPopoverButtonItem - 2");
			
			toolbar.BackgroundColor = UIColor.FromPatternImage (UIImage.FromFile ("images/SessionsHeader3.png"));
			testFlight.PassCheckpoint ("RotatingSpeakerBioViewController.ShowRootPopoverButtonItem - 3");
			
			
			if (toolbar != null) {
				testFlight.PassCheckpoint ("RotatingSpeakerBioViewController.ShowRootPopoverButtonItem - 4");
			
				var itemsArray = this.toolbar.Items.ToList ();
				testFlight.PassCheckpoint ("RotatingSpeakerBioViewController.ShowRootPopoverButtonItem - 5");
				
				if (itemsArray.Count == 0) {
					testFlight.PassCheckpoint ("RotatingSpeakerBioViewController.ShowRootPopoverButtonItem - 6");
					//Console.WriteLine("RotatingSpeakerBioViewController.RootPopoverButtonItem - itemsArray.Count == 0");
					itemsArray.Insert (0, barButtonItem);
					testFlight.PassCheckpoint ("RotatingSpeakerBioViewController.ShowRootPopoverButtonItem - 7");
				} else {
					testFlight.PassCheckpoint ("RotatingSpeakerBioViewController.ShowRootPopoverButtonItem - 8");
					//Console.WriteLine("RotatingSpeakerBioViewController.RootPopoverButtonItem - itemsArray.Count <> 0");
					itemsArray [0] = barButtonItem;
					testFlight.PassCheckpoint ("RotatingSpeakerBioViewController.ShowRootPopoverButtonItem - 9");
				}
				//[itemsArray insertObject:barButtonItem atIndex:0];
				testFlight.PassCheckpoint ("RotatingSpeakerBioViewController.ShowRootPopoverButtonItem - 10");
      
				this.toolbar.SetItems (itemsArray.ToArray (), false);
				testFlight.PassCheckpoint ("RotatingSpeakerBioViewController.ShowRootPopoverButtonItem - 11");
				
				//[toolbar setItems:itemsArray animated:NO];
      
				itemsArray = null;
				testFlight.PassCheckpoint ("RotatingSpeakerBioViewController.ShowRootPopoverButtonItem - 12");
				
				//[itemsArray release];
			}
		}

		public void InvalidateRootPopoverButtonItem (UIBarButtonItem barButtonItem)
		{
			//Console.WriteLine("RotatingSpeakerBioViewController.InvalidateRootPopoverButtonItem - barButtonItem is null " + (barButtonItem == null).ToString());
			
			if (this.InterfaceOrientation == UIInterfaceOrientation.Portrait || this.InterfaceOrientation == UIInterfaceOrientation.PortraitUpsideDown) {
				toolbar = ((SpeakerBioViewController)this.PortraitViewController).Toolbar;
				//Console.WriteLine("RotatingSpeakerBioViewController.InvalidateRootPopoverButtonItem - IsInPortrait - toolbar is null" + (toolbar == null).ToString());
			}
			
			if (toolbar != null) {
				var itemsArray = this.toolbar.Items.ToList ();
				itemsArray.Remove (barButtonItem);
				this.toolbar.SetItems (itemsArray.ToArray (), false);
				itemsArray = null;
			}
		}			
	}
}