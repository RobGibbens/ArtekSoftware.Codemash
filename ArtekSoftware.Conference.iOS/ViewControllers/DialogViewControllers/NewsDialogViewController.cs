using System;
using MonoTouch.Dialog;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.UIKit;

namespace ArtekSoftware.CodeMash
{
	public class NewsDialogViewController : DialogViewController
	{
		//private IEnumerable<ScheduledSessionEntity> _sessions;
		private NewsDialogMapper _newsDialogMapper;
		
		public NewsDialogViewController () : base(null)
		{
			this.Autorotate = true;
			
			_newsDialogMapper = new NewsDialogMapper();
			//_sessions = _newsDialogMapper.GetScheduledSessions(isRefresh:false);
			this.Root = _newsDialogMapper.GetNewsDialog(isRefresh:false);
			
			this.EnableSearch = true;
			this.AutoHideSearch = true;
			this.SearchPlaceholder = "Search News...";
			this.Style = UITableViewStyle.Plain;
			this.RefreshRequested += HandleHandleRefreshRequested;
		}

		void HandleHandleRefreshRequested (object sender, EventArgs e)
		{
			this.ReloadData();
			this.ReloadComplete ();
		}
		
		public override void Selected (MonoTouch.Foundation.NSIndexPath indexPath)
		{
			base.Selected (indexPath);
		}
	}
}

