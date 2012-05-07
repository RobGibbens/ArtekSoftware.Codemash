using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;

namespace ArtekSoftware.CodeMash
{
	[Register("MyDetailViewController")]
	public partial class MyDetailViewController : UIViewController
	{
		AppDelegate _appDelegate;

		public MyDetailViewController ()
		{
		}
		
		public MyDetailViewController (AppDelegate appDelegate)
		{
			_appDelegate = appDelegate;
		}
		
		public MyDetailViewController (IntPtr handle) : base(handle)
		{
		
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}

		public override void ViewDidUnload ()
		{
		}
		
		public void SetMap ()
		{
			RectangleF frame = new RectangleF (0, 40, this.View.Bounds.Width, this.View.Bounds.Height - 40);
			//RectangleF fullFrame = new RectangleF(0, 0, 2000, 2000);
			UIView view = new UIView(frame);
			view.BackgroundColor = UIColor.White;
			//TODO view.AddSubview(_appDelegate.MapViewController.View);
			this.View.AddSubview (view);
			this.View.Subviews.Last ().Frame = frame;
		}
		
		public void SetDefaultView ()
		{
			RectangleF frame = new RectangleF (0, 40, this.View.Bounds.Width, this.View.Bounds.Height - 40);
			
			//TODO this.View.AddSubview (_appDelegate.DefaultView.View);
			this.View.Subviews.Last ().Frame = frame;
		}
		
//		public void SetSessionDetail ()
//		{
//			RectangleF frame = new RectangleF (0, 40, this.View.Bounds.Width, this.View.Bounds.Height - 40);
//			this.View.AddSubview (_appDelegate.SessionDetail.View);
//			this.View.Subviews.Last ().Frame = frame;
//		}
		
//		public void SetSpeakerDetail ()
//		{
//			RectangleF frame = new RectangleF (0, 40, this.View.Bounds.Width, this.View.Bounds.Height - 40);
//
//			this.View.AddSubview (_appDelegate.SpeakerBioViewController.View);
//			//this.View.AddSubview (_appDelegate.RotatingViewController.View);
//			
//			//this.View.InsertSubview(_appDelegate.SpeakerBioViewController.View, 1);
//			this.View.Subviews.Last ().Frame = frame;
//		}
	}
}
