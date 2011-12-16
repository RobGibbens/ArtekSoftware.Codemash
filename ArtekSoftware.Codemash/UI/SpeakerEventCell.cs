using System;
using System.Drawing;
using System.IO;
using MonoTouch.CoreGraphics;
using MonoTouch.Dialog;
using MonoTouch.Dialog.Utilities;
using MonoTouch.UIKit;

namespace ArtekSoftware.Codemash
{
	public class SpeakerEventCell : SpeakerEventElement
	{
		public SpeakerEventCell (SpeakerEntity speaker)
		{
			this.Name = speaker.Name;
			this.DateRoom = "";
			this.TwitterName = speaker.TwitterHandle;
			this.Speaker = speaker;
		}
		
		public float GetHeight (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			return 76;
		}
	}
	
	public abstract class SpeakerEventElement : Element, IElementSizing, IImageUpdated
	{
		protected string Name, DateRoom, TwitterName, ImageUrl;
		protected SpeakerEntity Speaker;
		
		public SpeakerEventElement () : base(null)
		{
		}
		
		string _cellIdentifier = "SpeakerViewCell";
		
		SpeakerInfoCell _cell;
		public override UITableViewCell GetCell (UITableView tv)
		{
			_cell = tv.DequeueReusableCell (_cellIdentifier) as SpeakerInfoCell;
			if (_cell == null) {
				_cell = new SpeakerInfoCell (_cellIdentifier);
			}

			_loadDataIntoCell ();
			return _cell;
		}

		private void _loadDataIntoCell ()
		{
			_cell.SetSpeaker (this.Speaker);
			_cell.btnTitle.SetTitle (Name, UIControlState.Normal);
			_cell.txtDate.Text = TwitterName;
			_cell.txtRoom.Text = DateRoom;
			
			SetImageUrl ();
			SetLocalImage (ImageUrl);
			
			_cell.SetNeedsLayout ();
		}
		
		public void SetImageUrl ()
		{
			if (!string.IsNullOrWhiteSpace(this.Speaker.TwitterHandle))
			{
				ImageUrl = "images/Profiles/" + this.Speaker.TwitterHandle.Replace ("@", "") + ".png";
			}	
			
			if (ImageUrl != string.Empty && File.Exists (ImageUrl)) {
			} else if (File.Exists ("images/Profiles/" + this.Speaker.Name.Replace (" ", "") + ".png")) {
				ImageUrl = "images/Profiles/" + this.Speaker.Name.Replace (" ", "") + ".png";
			} else {
				ImageUrl = "images/Profiles/DefaultUser.png";
			}

		}		
		
		public void SetLocalImage (string url)
		{
			if (!string.IsNullOrEmpty (url)) {
				var imageBackground = new Uri ("file://" + Path.GetFullPath (url));
				var image = ImageLoader.DefaultRequestImage (imageBackground, this);
				if (image != null)
				{
					//_cell.btnAvatar.BackgroundColor = UIColor.FromPatternImage(image);
					_cell.imageView.Image = image;
				}
					
			}
		}	
		
		public void UpdatedImage (Uri uri)
		{
			var image = ImageLoader.DefaultRequestImage (uri, this);			

			_cell.btnAvatar.BackgroundColor = UIColor.FromPatternImage (image);
			_cell.ImageView.Image = image;

			_cell.SetNeedsLayout ();
			_cell.SetNeedsDisplay ();
		}		
		
		#region IElementSizing implementation
		public float GetHeight (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			if (string.IsNullOrEmpty (DateRoom))
				return 50;
			
			return 76;
		}
		#endregion		

	}
	
	class SpeakerInfoCell : UITableViewCell
	{

		static CGGradient bottomGradient, topGradient;
		public UIButton btnTitle, btnAvatar;
		public UILabel txtRoom, txtDate;
		public static UIFont SmallFont = UIFont.SystemFontOfSize (13f);
		public UIImageView imageView;
		string imgurl;
		
		private SpeakerEntity _speaker;

		public void SetSpeaker (SpeakerEntity speaker)
		{
			_speaker = speaker;
		}
		
		public SpeakerInfoCell (string cellId):base(UITableViewCellStyle.Default, cellId)
		{
			this.ContentView.BackgroundColor = UIColor.FromPatternImage (SessionInfoCell.CellBackground);
			btnTitle = UIButton.FromType (UIButtonType.Custom);
			btnTitle.Font = UIFont.FromName ("STHeitiTC-Medium", 14);
			btnTitle.SetTitleColor (UIColor.White, UIControlState.Normal);
			btnTitle.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
			btnTitle.Frame = new RectangleF (60, 4, 240, 25);
			btnTitle.TouchUpInside += HandleUserSelected;
			
			txtDate = new UILabel (new RectangleF (60, 24, 200, 20)){ Font = UIFont.FromName ("STHeitiTC-Medium", 12), TextColor = UIColor.DarkGray};
			txtDate.BackgroundColor = UIColor.Clear;
			txtDate.TextColor = UIColor.White;
			
			txtRoom = new UILabel (new RectangleF (60, 50, 240, 20)){Font = UIFont.FromName ("STHeitiTC-Medium", 12) , Lines=0};
			txtRoom.BackgroundColor = UIColor.Clear;
			txtRoom.TextColor = UIColor.White;
			
			btnAvatar = UIButton.FromType (UIButtonType.Custom);
			btnAvatar.Frame = new Rectangle (10, 10, 40, 40);
			
			imageView = new UIImageView (new Rectangle (0, 0, 48, 48));
			imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
			imageView.Image = UIImage.FromBundle ("img/gravatar");
			
			btnAvatar.AddSubview (imageView);
			btnAvatar.TouchUpInside += HandleUserSelected;
			
			this.AddSubview (txtDate);
			this.AddSubview (btnTitle);
			this.AddSubview (txtRoom);
			this.AddSubview (btnAvatar);

			this.SelectionStyle = UITableViewCellSelectionStyle.None;
		}		
		
		void HandleUserSelected (object sender, EventArgs e)
		{
			var user = btnTitle.Title (UIControlState.Normal);
			AppDelegate.CurrentAppDelegate.SetSpeaker (_speaker);
		}

		public override void SetNeedsLayout ()
		{
			base.SetNeedsLayout ();
			float opinionHeight = txtRoom.Text == null ? 50 : this.StringSize (txtRoom.Text, SpeakerInfoCell.SmallFont, new SizeF (240, float.MaxValue), UILineBreakMode.WordWrap).Height;
			txtRoom.Frame = new RectangleF (60, 50, 240, opinionHeight);
		}

		public override void Draw (RectangleF rect)
		{
			base.Draw (rect);

			var context = UIGraphics.GetCurrentContext ();

			var bounds = Bounds;
			var midx = bounds.Width / 2;

			UIColor.White.SetColor ();
			context.FillRect (bounds);
		}

		public override void PrepareForReuse ()
		{
			base.PrepareForReuse ();
			this.imgurl = null;
			imageView.Image = null;
		}

	}	
	
}