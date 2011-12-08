using System;
using MonoTouch.Dialog;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.Foundation;


using System.Threading;

namespace ArtekSoftware.Codemash
{
	public class SessionDialogViewController : DialogViewController
	{
		private IEnumerable<SessionEntity> _sessions;
		private SessionsDialogMapper _sessionsDialogMapper;
		
		public SessionDialogViewController () : base(null)
		{
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) {
				this.Autorotate = false;
			}
			else
			{
				this.Autorotate = true;
			}
			
			LoadData (isRefresh:false);
			
			//this.EnableSearch = true;
			this.AutoHideSearch = true;
			this.SearchPlaceholder = "Search Sessions...";
			this.Style = UITableViewStyle.Plain;
			this.RefreshRequested += HandleHandleRefreshRequested;
			
		}
		
		void LoadData (bool isRefresh)
		{
			
			TestFlightProxy.PassCheckpoint ("SessionDialogViewController.LoadData - 1");
			//Thread.Sleep (500);
			
			_sessionsDialogMapper = new SessionsDialogMapper ();
			TestFlightProxy.PassCheckpoint ("SessionDialogViewController.LoadData - 2");
			//Thread.Sleep (500);
			
			_sessions = _sessionsDialogMapper.GetSessions (isRefresh:isRefresh);
			TestFlightProxy.PassCheckpoint ("SessionDialogViewController.LoadData - 3");
			//Thread.Sleep (1000);
			
			_sessions = _sessions.OrderBy (x => x.StartDate);
			TestFlightProxy.PassCheckpoint ("SessionDialogViewController.LoadData - 4");
			//Thread.Sleep (500);
			
			//this.Root = null;
			this.Root = _sessionsDialogMapper.GetSessionDialog (_sessions);
			TestFlightProxy.PassCheckpoint ("SessionDialogViewController.LoadData - 5");
			//Thread.Sleep (500);
			
			
			this.ReloadData ();
			TestFlightProxy.PassCheckpoint ("SessionDialogViewController.LoadData - 6");
			//Thread.Sleep (500);
			
			this.ReloadComplete ();
			TestFlightProxy.PassCheckpoint ("SessionDialogViewController.LoadData - 7");
			//Thread.Sleep (500);
			
		}
		
//		public override RefreshTableHeaderView MakeRefreshTableHeaderView (RectangleF rect)
//		{
//			List<UIImage> myImages = new List<UIImage> ();
//			for (int i = 0; i < 24; i++) {
//				myImages.Add (UIImage.FromFile ("images/LoadingGif/Loading" + i.ToString () + ".png"));
//			}
//			
//			var table = base.MakeRefreshTableHeaderView (rect, myImages);		
//			
//			return table;
//		}
		
		void HandleHandleRefreshRequested (object sender, EventArgs e)
		{
			TestFlightProxy.PassCheckpoint ("SessionDialogViewController.HandleRefreshRequested - 1");
			LoadData (isRefresh:true);
			TestFlightProxy.PassCheckpoint ("SessionDialogViewController.HandleRefreshRequested - 2");
			TestFlightProxy.PassCheckpoint ("Refreshed Sessions");
			
		}
		
		private int CalculateSelectedRow (NSIndexPath indexPath, UITableView tableView)
		{
			int totalCountOfRows = 0;
			int selectedSectionNumber = indexPath.Section;
			
			for (int currentSectionNumber = 0; currentSectionNumber < selectedSectionNumber; ++ currentSectionNumber) {
				totalCountOfRows += tableView.NumberOfRowsInSection (currentSectionNumber);
			}
			
			int selectedRow = totalCountOfRows + indexPath.Row;
			
			return selectedRow;
		}
		
		public override void Selected (MonoTouch.Foundation.NSIndexPath indexPath)
		{
			int selectedRow = CalculateSelectedRow (indexPath, this.TableView);

			SessionEntity session = _sessions.ToList () [selectedRow];
			
			AppDelegate.CurrentAppDelegate.SetSession (session);
			
			base.Selected (indexPath);
		}
		
		
	}
}