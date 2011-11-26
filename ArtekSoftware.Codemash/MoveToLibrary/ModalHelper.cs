using System;
using MonoTouch.UIKit;

namespace ArtekSoftware.Codemash
{
	public class ModalDialog
	{
		public static void Alert (string title, string message)
		{
			UIAlertView alert = new UIAlertView ();
			alert.Title = title;
			alert.AddButton ("Ok");
			alert.Message = message;
			alert.Show ();
		}

		public static void AskYesNo (string title, string message)
		{
			UIAlertView alert = new UIAlertView ();
			alert.Title = title;
			alert.AddButton ("Yes");
			alert.AddButton ("No");
			alert.Message = message;
			alert.Delegate = new MyAlertViewDelegate ();
			alert.Show ();
		}

		public class MyAlertViewDelegate : UIAlertViewDelegate
		{
			public override void Clicked (UIAlertView alertview, int buttonIndex)
			{
				// Don't call base or you'll get:
				// Unhandled Exception: MonoTouch.Foundation.You_Should_Not_Call_base_In_This_Method
			}

			public override void Canceled (UIAlertView alertView)
			{
				// Don't call base or you'll get:
				// Unhandled Exception: MonoTouch.Foundation.You_Should_Not_Call_base_In_This_Method
			}
		}	
	}
}

