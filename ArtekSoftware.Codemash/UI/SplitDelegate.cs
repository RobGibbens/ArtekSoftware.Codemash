using System;
using MonoTouch.UIKit;

namespace ArtekSoftware.Codemash
{
	public class SplitDelegate : UISplitViewControllerDelegate
	{

		public override void WillHideViewController (UISplitViewController svc, 
                                                 UIViewController aViewController, 
                                                 UIBarButtonItem barButtonItem, 
                                                 UIPopoverController pc)
		{
			//Console.WriteLine("SplitDelegate.WillHideViewController");
			barButtonItem.Title = "Sessions";
      
			var detailViewController = svc.ViewControllers [1] as ISubstitutableDetailViewController;

			detailViewController.PopOverController = pc;
			//Console.WriteLine("SplitDelegate.WillHideViewController - detailViewController.PopOverController is null " + (detailViewController.PopOverController == null).ToString());
			
			detailViewController.RootPopoverButtonItem = barButtonItem;
			//Console.WriteLine("SplitDelegate.WillHideViewController -  detailViewController.RootPopoverButtonItem is null " + (detailViewController.RootPopoverButtonItem == null).ToString());
			
			//Console.WriteLine("SplitDelegate.WillHideViewController - showRootPopoverButtonItem");
			detailViewController.ShowRootPopoverButtonItem (detailViewController.RootPopoverButtonItem);
		}

		public override void WillShowViewController (UISplitViewController svc, UIViewController aViewController, UIBarButtonItem button)
		{
			//Console.WriteLine("SplitDelegate.WillShowViewController");
			var detailViewController = svc.ViewControllers [1] as ISubstitutableDetailViewController;
			
			//Console.WriteLine("SplitDelegate.WillShowViewController - detailViewController.RootPopoverButtonItem is null " +  (detailViewController.RootPopoverButtonItem == null).ToString());
			detailViewController.InvalidateRootPopoverButtonItem (detailViewController.RootPopoverButtonItem);
			
			//Console.WriteLine("SplitDelegate.WillShowViewController - setting detailViewController.PopOverController = null");
			detailViewController.PopOverController = null;

			//Console.WriteLine("SplitDelegate.WillShowViewController - setting detailViewController.RootPopoverButtonItem = null");
			detailViewController.RootPopoverButtonItem = null;
		}
    
	}
}

