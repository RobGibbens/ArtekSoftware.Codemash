using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
////using MonoQueue;
using ArtekSoftware.Conference;
using ArtekSoftware.Conference.LocalData;

namespace ArtekSoftware.Codemash
{
	public class SpeakerDialogViewController : DialogViewController
	{
		private IEnumerable<SpeakerEntity> _speakers;
		private SpeakersDialogMapper _speakersDialogMapper;
		private List<string> sectionTitles;
		private INetworkStatusCheck _networkStatusCheck;
		
		public SpeakerDialogViewController () : base (null)
		{
			//_networkStatusCheck = new NetworkStatusCheck();
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) {
				this.Autorotate = false;
			}
			else
			{
				this.Autorotate = true;
			}
			
			this.RefreshRequested += HandleHandleRefreshRequested;
			this.Style = UITableViewStyle.Plain;
			this.TableView.RowHeight = 64;
			LoadData (isRefresh:false);
			
			//this.EnableSearch = true;
			this.AutoHideSearch = true;
			this.SearchPlaceholder = "Search Speakers...";
			
		}
		
		class EntitySource : Source
		{
			SpeakerDialogViewController parent;

			public EntitySource (SpeakerDialogViewController parent) : base (parent)
			{
				this.parent = parent;
			}

			public override string[] SectionIndexTitles (UITableView tableView)
			{
				return parent.sectionTitles.ToArray ();
			}
		}

		class SizingIndexedSource : Source
		{
			SpeakerDialogViewController parent;

			public SizingIndexedSource (SpeakerDialogViewController parent) : base (parent)
			{
				this.parent = parent;
			}

			public override string[] SectionIndexTitles (UITableView tableView)
			{
				return parent.sectionTitles.ToArray ();
			}
		}

		public override Source CreateSizingSource (bool unevenRows)
		{
			if (unevenRows)
				return new SizingIndexedSource (this);
			else
				return new EntitySource (this);
			;
		}
		
		void LoadData (bool isRefresh)
		{
			_speakersDialogMapper = new SpeakersDialogMapper (_networkStatusCheck);
			_speakers = _speakersDialogMapper.GetSpeakers (isRefresh:isRefresh);
			sectionTitles = _speakers.Select (x => x.Name.Substring (0, 1)).Distinct ().OrderBy (x => x).ToList ();
			this.Root = _speakersDialogMapper.GetSpeakerDialog (_speakers, sectionTitles);
			
			this.ReloadData ();
			this.ReloadComplete ();
		}

		void HandleHandleRefreshRequested (object sender, EventArgs e)
		{
			ITestFlightProxy testFlight = new TestFlightProxy();
			
			LoadData (isRefresh:true);
			testFlight.PassCheckpoint ("Refreshed Speakers");
			
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

		public override void Selected (NSIndexPath indexPath)
		{
			int selectedRow = CalculateSelectedRow(indexPath, this.TableView);
			
			SpeakerEntity speaker = _speakers.ToList () [selectedRow];
			AppDelegate.CurrentAppDelegate.Navigation.SetSpeaker (speaker);
			
			ITestFlightProxy testFlight = new TestFlightProxy();
			
			testFlight.PassCheckpoint("Selected Speaker");
			
			base.Selected (indexPath);
		}
			
	}
}