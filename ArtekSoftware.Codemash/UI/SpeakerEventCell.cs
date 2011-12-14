using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.CoreGraphics;
using System.IO;
using MonoTouch.Dialog.Utilities;

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
	
	public abstract class SpeakerEventElement : Element, IElementSizing
	{
		protected string Name, DateRoom, TwitterName, ImageUrl;
		protected SpeakerEntity Speaker;
		
		public SpeakerEventElement () : base(null)
		{
		}
		
		string _cellIdentifier = "SpeakerViewCell";

		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = tv.DequeueReusableCell (_cellIdentifier) as SpeakerInfoCell;
			if (cell == null) {
				cell = new SpeakerInfoCell (_cellIdentifier);
			}

			_loadDataIntoCell (cell);
			return cell;
		}

		private void _loadDataIntoCell (SpeakerInfoCell cell)
		{
			cell.SetSpeaker (this.Speaker);
			cell.btnTitle.SetTitle (Name, UIControlState.Normal);
			cell.txtDate.Text = TwitterName;
			cell.txtRoom.Text = DateRoom;
			cell.SetImage ();
			cell.SetNeedsLayout ();
		}
		
//		public override bool Matches (string text)
//		{
//			bool matches = false;
//			if (!string.IsNullOrWhiteSpace (text)) {
//				var searchValue = text.ToLower ();
//				
//				matches = (this.Speaker.TwitterHandle != null && this.Speaker.TwitterHandle.ToLower ().Contains (searchValue));
//				if (!matches) {
//					matches = (this.Speaker.Name != null && this.Speaker.Name.ToLower ().Contains (searchValue));	
//				}
//				if (!matches) {
//					matches = (this.Speaker.BlogURL != null && this.Speaker.BlogURL.ToLower ().Contains (searchValue));	
//				}
//				if (!matches) {
//					matches = (this.Speaker.Biography != null && this.Speaker.Biography.ToLower ().Contains (searchValue));	
//				}
//
//			
//			}
//			
//			return matches;
//		}			
		
		#region IElementSizing implementation
		public float GetHeight (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			if (string.IsNullOrEmpty (DateRoom))
				return 50;
			
			return 76;
		}
		#endregion		

	}
	
	class SpeakerInfoCell : UITableViewCell , IImageUpdated
	{

		static CGGradient bottomGradient, topGradient;
		public UIButton btnTitle, btnAvatar;
		public UILabel txtRoom, txtDate;
		public static UIFont SmallFont = UIFont.SystemFontOfSize (13f);
		public UIImageView imageView;
		string imgurl;

		static SpeakerInfoCell ()
		{
			using (var rgb = CGColorSpace.CreateDeviceRGB()) {
				float [] colorsBottom = {
					1, 1, 1, .5f,
					0.93f, 0.93f, 0.93f, .5f
				};
				bottomGradient = new CGGradient (rgb, colorsBottom, null);
				float [] colorsTop = {
					0.93f, 0.93f, 0.93f, .5f,
					1, 1, 1, 0.5f
				};
				topGradient = new CGGradient (rgb, colorsTop, null);
			}
		}
		
		public void SetLocalImage (string url)
		{
			if (!string.IsNullOrEmpty (url)) {
				this.imgurl = url;
				
				var imageBackground = new Uri ("file://" + Path.GetFullPath (url));
				var image = ImageLoader.DefaultRequestImage (imageBackground, null);
				//UIImage image = GetSmallImage (url);
					
				using (imageView.Image) {
					image = Extensions.RemoveSharpEdges (image, Convert.ToInt32 (image.Size.Width), 4);
					imageView.Image = image;
				}				

			}
		}
		
		public void SetImage ()
		{
			UIImage image = null;
			

			string profileImage = string.Empty;
			
			if (!string.IsNullOrWhiteSpace(_speaker.TwitterHandle))
			{
				profileImage = "images/Profiles/" + this._speaker.TwitterHandle.Replace ("@", "") + ".png";
			}	
			
			if (profileImage != string.Empty && File.Exists (profileImage)) {
				var imageBackground = new Uri ("file://" + Path.GetFullPath (profileImage));
				image = ImageLoader.DefaultRequestImage (imageBackground, null);
			} else if (File.Exists ("images/Profiles/" + _speaker.Name.Replace (" ", "") + ".png")) {
				profileImage = "images/Profiles/" + _speaker.Name.Replace (" ", "") + ".png";
				var imageBackground = new Uri ("file://" + Path.GetFullPath (profileImage));
				image = ImageLoader.DefaultRequestImage (imageBackground, null);
			} else {
				profileImage = "images/Profiles/DefaultUser.png";
				var imageBackground = new Uri ("file://" + Path.GetFullPath (profileImage));
				image = ImageLoader.DefaultRequestImage (imageBackground, null);
			}
			
			this.imgurl = profileImage;
			
			
			if (image != null) {
				using (imageView.Image) {
					image = Extensions.RemoveSharpEdges (image, Convert.ToInt32 (image.Size.Width), 4);
					imageView.Image = image;
				}
			}
		}
		
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
		
//		private static UIImage _cellBackground;
//		public static UIImage CellBackground {
//			get {
//				if (_cellBackground == null) {
//				_cellBackground = UIImage.FromFile ("images/CellBackground2.png");
//				}
//				
//				return _cellBackground;
//			}
//		}		
		
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

			context.DrawLinearGradient (bottomGradient, new PointF (midx, bounds.Height - 17), new PointF (midx, bounds.Height), 0);
			context.DrawLinearGradient (topGradient, new PointF (midx, 1), new PointF (midx, 3), 0);
		}


		#region IImageUpdated implementation
		public void UpdatedImage (string url, UIImage image)
		{
			if (imgurl != url)
				return;
			
			if (image != null) {
				imageView.Image = Extensions.RemoveSharpEdges (image, Convert.ToInt32 (image.Size.Width), 4);
			}
			this.SetNeedsDisplay ();
		}
		#endregion

		public override void PrepareForReuse ()
		{
			base.PrepareForReuse ();
			this.imgurl = null;
			imageView.Image = null;
		}
	}	
	
}

