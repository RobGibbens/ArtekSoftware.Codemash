using System;
using MonoTouch.Dialog;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.UIKit;
using MonoTouch.Foundation;


namespace ArtekSoftware.Codemash
{
	public class SpeakerDialogViewController : DialogViewController
	{
		private IEnumerable<SpeakerEntity> _speakers;
		private SpeakersDialogMapper _speakersDialogMapper;
		private List<string> sectionTitles;
		
		public SpeakerDialogViewController () : base (null)
		{
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
			_speakersDialogMapper = new SpeakersDialogMapper ();
			_speakers = _speakersDialogMapper.GetSpeakers (isRefresh:isRefresh);
			sectionTitles = _speakers.Select (x => x.Name.Substring (0, 1)).Distinct ().OrderBy (x => x).ToList ();
			this.Root = _speakersDialogMapper.GetSpeakerDialog (_speakers, sectionTitles);
			
			this.ReloadData ();
			this.ReloadComplete ();
		}

		void HandleHandleRefreshRequested (object sender, EventArgs e)
		{
			LoadData (isRefresh:true);
			TestFlightProxy.PassCheckpoint ("Refreshed Speakers");
			
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
			AppDelegate.CurrentAppDelegate.SetSpeaker (speaker);
			
			TestFlightProxy.PassCheckpoint("Selected Speaker");
			
			base.Selected (indexPath);
		}
			
	}
}