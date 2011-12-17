using MonoTouch.Dialog;
using MonoTouch.UIKit;

namespace ArtekSoftware.Codemash
{
	public class MapDialogViewController : DialogViewController
	{
		public MapDialogViewController () : base(null)
		{
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) {
				this.Autorotate = false;
			}
			else
			{
				this.Autorotate = true;
			}
			
			var root = new RootElement ("Maps");
			var conferenceCenter = new StyledStringElement("Conference Center");
			conferenceCenter.Font = UIFont.FromName ("STHeitiTC-Medium", 14);
			conferenceCenter.TextColor = UIColor.White;
			conferenceCenter.BackgroundColor = UIColor.FromPatternImage(SessionInfoCell.CellBackground);
			var location = new StyledStringElement("Location Map");
			location.Font = UIFont.FromName ("STHeitiTC-Medium", 14);
			location.TextColor = UIColor.White;
			location.BackgroundColor = UIColor.FromPatternImage(SessionInfoCell.CellBackground);
			this.Style = UITableViewStyle.Plain;
			
			root.Add (
				new Section() { 
					conferenceCenter,
					location
				}
			);
			//this.TableView.RowHeight = 64;
			this.Root = root;
			this.TableView.BackgroundColor = UIColor.FromPatternImage(SessionInfoCell.CellBackground);
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
			if (indexPath.Row == 0)
			{
				AppDelegate.CurrentAppDelegate.SetMap();
			}
			else if (indexPath.Row == 1)
			{
				AppDelegate.CurrentAppDelegate.SetLocationMap();
			}
			base.Selected (indexPath);
		}
	}
}