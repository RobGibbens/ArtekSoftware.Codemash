using System;
using System.Drawing;
using System.IO;
using MonoTouch.CoreGraphics;
using MonoTouch.Dialog;
using MonoTouch.Dialog.Utilities;
using MonoTouch.UIKit;

namespace ArtekSoftware.Codemash
{
	public class SessionEventCell : SessionEventElement
	{
		public SessionEventCell (SessionEntity session)
		{
			this.Title = session.Title;
			this.DateRoom = session.StartDate.ToString ("h:mm tt") + " | " + session.Room;
			this.Speaker = session.SpeakerName;
			this.Session = session;
			
		}
	}
	
	public abstract class SessionEventElement : Element, IElementSizing, IImageUpdated
	{
		protected string Title, DateRoom, Speaker, ImageUrl;
		protected SessionEntity Session;
		
		public SessionEventElement () : base(null)
		{
		}
		
		string _cellIdentifier = "SessionViewCell";
		SessionInfoCell _cell;

		public override UITableViewCell GetCell (UITableView tv)
		{
			_cell = tv.DequeueReusableCell (_cellIdentifier) as SessionInfoCell;
			if (_cell == null) {
				_cell = new SessionInfoCell (_cellIdentifier);
			}

			_loadDataIntoCell ();
			return _cell;
		}

		private void _loadDataIntoCell ()
		{
			_cell.SetSession (this.Session);
			_cell.btnTitle.SetTitle (Title, UIControlState.Normal);
			_cell.txtSpeaker.Text = Speaker;
			_cell.txtRoom.Text = DateRoom;
			
			SetImageUrl ();
			SetLocalImage (ImageUrl);

			_cell.SetNeedsLayout ();
		}
		
		public void SetLocalImage (string url)
		{
			if (!string.IsNullOrEmpty (url)) {
				var imageBackground = new Uri ("file://" + Path.GetFullPath (url));
				var image = ImageLoader.DefaultRequestImage (imageBackground, this);
				if (image != null)
				{
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

		#region IElementSizing implementation
		public float GetHeight (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			if (string.IsNullOrEmpty (DateRoom))
				return 50;
			return tableView.StringSize (DateRoom, SessionInfoCell.SmallFont, new SizeF (240, float.MaxValue), UILineBreakMode.WordWrap).Height + 60;
		}
		#endregion		

	}
	
	class SessionInfoCell : UITableViewCell
	{

		static CGGradient bottomGradient, topGradient;
		public UIButton btnTitle, btnAvatar;
		public UILabel txtRoom, txtSpeaker;
		public static UIFont SmallFont = UIFont.SystemFontOfSize (13f);
		public UIImageView imageView;
		string imgurl;

		
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
			AppDelegate.CurrentAppDelegate.Navigation.SetSession (_session);
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

			//context.DrawLinearGradient (bottomGradient, new PointF (midx, bounds.Height - 17), new PointF (midx, bounds.Height), 0);
			//context.DrawLinearGradient (topGradient, new PointF (midx, 1), new PointF (midx, 3), 0);
		}

		public override void PrepareForReuse ()
		{
			base.PrepareForReuse ();
			this.imgurl = null;
			imageView.Image = null;
		}
	}	
	
}

