using System;
using MonoTouch.Dialog;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
//using MonoTouch.TestFlight;

namespace ArtekSoftware.Codemash
{
	public class ScheduledSessionDialogViewController : DialogViewController
	{
		private IEnumerable<ScheduledSessionEntity> _sessions;
		private ScheduledSessionsDialogMapper _sessionsDialogMapper;
		
		public ScheduledSessionDialogViewController () : base(null)
		{
			this.Autorotate = true;
			
			LoadData ();
			
			//this.EnableSearch = true;
			this.AutoHideSearch = true;
			this.SearchPlaceholder = "Search Schedule...";
			this.Style = UITableViewStyle.Plain;
			//this.RefreshRequested += HandleHandleRefreshRequested;
		}

		public void LoadData ()
		{
			_sessionsDialogMapper = new ScheduledSessionsDialogMapper ();
			_sessions = _sessionsDialogMapper.GetScheduledSessions (isRefresh:false);
			this.Root = _sessionsDialogMapper.GetScheduledSessionDialog (_sessions, isRefresh:false);
			
			this.ReloadData ();
			this.ReloadComplete ();
		}
		
		void HandleHandleRefreshRequested (object sender, EventArgs e)
		{
			
			//var queueSync = new QueueSync();
			//queueSync.Sync();
			//var sessions = _sessionsDialogMapper.GetScheduledSessions(isRefresh:true);
			LoadData ();
			//TestFlight.PassCheckpoint ("Refreshed Scheduled Sessions");
			
		}
		
		public override void LoadView ()
		{
			base.LoadView ();
			TableView.BackgroundColor = UIColor.FromPatternImage(SessionInfoCell.CellBackground);
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
			ScheduledSessionEntity session = _sessions.ToList () [selectedRow];
			AppDelegate.CurrentAppDelegate.SetSession (session);
			
			base.Selected (indexPath);
		}
		
		
	}
}

