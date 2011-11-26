using System;
using MonoTouch.Dialog;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.Foundation;

namespace ArtekSoftware.Codemash
{
	public class SessionDialogViewController : DialogViewController
	{
		private IEnumerable<SessionEntity> _sessions;
		private SessionsDialogMapper _sessionsDialogMapper;
		
		public SessionDialogViewController () : base(null)
		{
			this.Autorotate = true;
			
			LoadData (isRefresh:false);
			
			//this.EnableSearch = true;
			this.AutoHideSearch = true;
			this.SearchPlaceholder = "Search Sessions...";
			this.Style = UITableViewStyle.Plain;
			this.RefreshRequested += HandleHandleRefreshRequested;
			
		}

		void LoadData (bool isRefresh)
		{
			_sessionsDialogMapper = new SessionsDialogMapper ();
			_sessions = _sessionsDialogMapper.GetSessions (isRefresh:isRefresh);
			_sessions = _sessions.OrderBy (x => x.Start);
			//this.Root = null;
			this.Root = _sessionsDialogMapper.GetSessionDialog (_sessions);
			
			this.ReloadData ();
			this.ReloadComplete ();
		}
		
		void HandleHandleRefreshRequested (object sender, EventArgs e)
		{
			LoadData (isRefresh:true);
			//TestFlightSdk.TestFlight.PassCheckpoint ("Refreshed Sessions");
			
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