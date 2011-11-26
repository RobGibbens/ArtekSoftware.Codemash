using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.CoreGraphics;
using System.Collections.Generic;

namespace ArtekSoftware.Codemash
{
	public class SessionEventCell : SessionEventElement
	{
		public SessionEventCell (SessionEntity session)
		{
			this.Title = session.Title;
			this.DateRoom = session.Start.ToShortTimeString () + " | " + session.Room;
			this.Speaker = session.SpeakerName;
			this.Session = session;
			
		}
	}
	
	public abstract class SessionEventElement : Element, IElementSizing
	{
		protected string Title, DateRoom, Speaker, ImageUrl;
		protected SessionEntity Session;
		
		public SessionEventElement () : base(null)
		{
		}
		
		string _cellIdentifier = "SessionViewCell";

		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = tv.DequeueReusableCell (_cellIdentifier) as SessionInfoCell;
			if (cell == null) {
				cell = new SessionInfoCell (_cellIdentifier);
			}

			_loadDataIntoCell (cell);
			return cell;
		}

		private void _loadDataIntoCell (SessionInfoCell cell)
		{
			cell.SetSession (this.Session);
			cell.btnTitle.SetTitle (Title, UIControlState.Normal);
			cell.txtSpeaker.Text = Speaker;
			cell.txtRoom.Text = DateRoom;
			
			SetImageUrl ();
			cell.SetLocalImage (ImageUrl);
			
			
			//cell.SetImage (ImageUrl);
			cell.SetNeedsLayout ();
		}

		void SetImageUrl ()
		{
			if (this.Session.Technology.ToLower () == ".net") {
				ImageUrl = "images/Technologies/DotNetSmall2.png";
			} else if (this.Session.Technology.ToLower () == "ruby") {
				ImageUrl = "images/Technologies/RubySmall.png";
			} else if (this.Session.Technology.ToLower () == "mobile") {
				ImageUrl = "images/Technologies/mobile2Small.png";
			} else if (this.Session.Technology.ToLower () == "javascript") {
				ImageUrl = "images/Technologies/JavaScriptSmall.png";
			} else if (this.Session.Technology.ToLower () == "design/ux") {
				ImageUrl = "images/Technologies/DesignUX2Small.png";
			} else if (this.Session.Technology.ToLower () == "java") {
				ImageUrl = "images/Technologies/JavaSmall.png";
			} else if (this.Session.Technology.ToLower () == "windows 8") {
				ImageUrl = "images/Technologies/WindowsSmall.png";
			} else if (this.Session.Technology.ToLower () == "other languages") {
				ImageUrl = "images/Technologies/OtherLanguages2Small.png";
			} else if (this.Session.Technology.ToLower () == "software process") {
				ImageUrl = "images/Technologies/SoftwareProcess4Small.png";
			} else {
				ImageUrl = "images/Technologies/Other2.png";
			}
		}
		
		public override bool Matches (string text)
		{
			bool matches = false;
			if (!string.IsNullOrWhiteSpace (text)) {
				var searchValue = text.ToLower ();
				
				matches = (this.Session.Technology != null && this.Session.Technology.ToLower ().Contains (searchValue));
				if (!matches) {
					matches = (this.Session.SpeakerName != null && this.Session.SpeakerName.ToLower ().Contains (searchValue));	
				}				
				if (!matches) {
					matches = (this.Session.Title != null && this.Session.Title.ToLower ().Contains (searchValue));	
				}					

			
			}
			
			return matches;
		}
		#region IElementSizing implementation
		public float GetHeight (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			if (string.IsNullOrEmpty (DateRoom))
				return 50;
			return tableView.StringSize (DateRoom, SessionInfoCell.SmallFont, new SizeF (240, float.MaxValue), UILineBreakMode.WordWrap).Height + 60;
		}
		#endregion		

	}
	
	class SessionInfoCell : UITableViewCell , IImageUpdated
	{

		static CGGradient bottomGradient, topGradient;
		public UIButton btnTitle, btnAvatar;
		public UILabel txtRoom, txtSpeaker;
		public static UIFont SmallFont = UIFont.SystemFontOfSize (13f);
		public UIImageView imageView;
		string imgurl;

		static SessionInfoCell ()
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
		
		public static UIImage GetSmallImage(string imageUrl)
		{
			var smallImages = SmallImages;
			UIImage image;
			if (smallImages.ContainsKey(imageUrl))
			{
				image = smallImages[imageUrl];
			}
			else
			{
				smallImages[imageUrl] = UIImage.FromFile(imageUrl);
				image = smallImages[imageUrl];
			}
			
			return image;
		}
		
		private static Dictionary<string, UIImage> _smallImages;
		public static Dictionary<string, UIImage> SmallImages {
			get {
				if (_smallImages == null) {
					_smallImages = new Dictionary<string, UIImage>();
				}
				
				return _smallImages;
			}
		}
		
		public void SetLocalImage (string url)
		{
			if (!string.IsNullOrEmpty (url)) {
				this.imgurl = url;
				
				UIImage image = GetSmallImage (url);
					
				using (imageView.Image) {
					image = Extensions.RemoveSharpEdges (image, Convert.ToInt32 (image.Size.Width), 4);
					imageView.Image = image;
				}
				
			}
		}
		
		public void SetImage (string url)
		{
			this.imgurl = url;
			if (!string.IsNullOrEmpty (url)) {
				if (!SimpleImageStore.Current.RequestImage (url, this)) {
					imageView.Image = UIImage.FromBundle ("img/gravatar");
				}
			} else {
				imageView.Image = UIImage.FromBundle ("img/gravatar");
			}
		}
		
		private SessionEntity _session;

		public void SetSession (SessionEntity session)
		{
			_session = session;
		}
		
		private static UIImage _cellBackground;
		public static UIImage CellBackground {
			get {
				if (_cellBackground == null) {
					_cellBackground = UIImage.FromFile ("images/CellBackground2.png");
				}
				
				return _cellBackground;
			}
		}

		public SessionInfoCell (string cellId):base(UITableViewCellStyle.Default, cellId)
		{
			this.ContentView.BackgroundColor = UIColor.FromPatternImage (SessionInfoCell.CellBackground);
			
			btnTitle = UIButton.FromType (UIButtonType.Custom);
			btnTitle.Frame = new RectangleF (60, 4, 240, 25);
			btnTitle.TouchUpInside += HandleUserSelected;
			btnTitle.Font = UIFont.FromName ("STHeitiTC-Medium", 14);
			btnTitle.SetTitleColor (UIColor.White, UIControlState.Normal);
			btnTitle.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
			btnTitle.BackgroundColor = UIColor.Clear;
			
			txtSpeaker = new UILabel (new RectangleF (60, 24, 200, 20)){ Font = UIFont.FromName ("STHeitiTC-Medium", 12), TextColor = UIColor.DarkGray};
			txtSpeaker.BackgroundColor = UIColor.Clear;
			txtSpeaker.TextColor = UIColor.White;
			
			txtRoom = new UILabel (new RectangleF (60, 50, 240, 20)){Font = UIFont.FromName ("STHeitiTC-Medium", 12) , Lines=0};
			txtRoom.BackgroundColor = UIColor.Clear;
			txtRoom.TextColor = UIColor.White;
			
			btnAvatar = UIButton.FromType (UIButtonType.Custom);
			btnAvatar.Frame = new Rectangle (10, 10, 40, 40);
			
			imageView = new UIImageView (new Rectangle (0, 0, 48, 48));
			
			btnAvatar.AddSubview (imageView);
			btnAvatar.TouchUpInside += HandleUserSelected;
			
			this.AddSubview (txtSpeaker);
			this.AddSubview (btnTitle);
			this.AddSubview (txtRoom);
			this.AddSubview (btnAvatar);

			this.SelectionStyle = UITableViewCellSelectionStyle.None;
		}
		
		void HandleUserSelected (object sender, EventArgs e)
		{
			var user = btnTitle.Title (UIControlState.Normal);
			AppDelegate.CurrentAppDelegate.SetSession (_session);
		}

		public override void SetNeedsLayout ()
		{
			base.SetNeedsLayout ();
			float opinionHeight = txtRoom.Text == null ? 50 : this.StringSize (txtRoom.Text, SessionInfoCell.SmallFont, new SizeF (240, float.MaxValue), UILineBreakMode.WordWrap).Height;
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

			imageView.Image = Graphics.RemoveSharpEdges (image);
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

