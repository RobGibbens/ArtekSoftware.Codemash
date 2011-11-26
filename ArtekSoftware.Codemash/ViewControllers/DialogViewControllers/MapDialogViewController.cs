using System;
using MonoTouch.Dialog;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.UIKit;

namespace ArtekSoftware.Codemash
{
	public class MapDialogViewController : DialogViewController
	{
		public MapDialogViewController () : base(null)
		{
			this.Autorotate = true;
			
			var root = new RootElement ("Map");
			var conferenceCenter = new StyledStringElement("Conference Center");
			conferenceCenter.BackgroundColor = UIColor.FromPatternImage (SessionInfoCell.CellBackground);
			conferenceCenter.Font = UIFont.FromName ("STHeitiTC-Medium", 14);
			conferenceCenter.TextColor = UIColor.White;
			root.Add (
				new Section() { 
					conferenceCenter
				}
			);
			//this.TableView.RowHeight = 64;
			this.Root = root;
			//this.TableView.BackgroundColor = UIColor.FromPatternImage(SessionInfoCell.CellBackground);
			this.EnableSearch = false;
			this.Style = UITableViewStyle.Plain;
		}
		public override void LoadView ()
		{
			base.LoadView ();
			TableView.BackgroundColor = UIColor.FromPatternImage(SessionInfoCell.CellBackground);
		}
		public override void Selected (MonoTouch.Foundation.NSIndexPath indexPath)
		{
			//TODO AppDelegate.CurrentAppDelegate.ShowMap();
			base.Selected (indexPath);
		}
	}
}