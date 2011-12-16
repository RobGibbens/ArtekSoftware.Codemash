using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.CoreGraphics;
using System.Collections.Generic;
using MonoTouch.Dialog.Utilities;
using System.IO;
using System.Threading;
using MonoTouch.Foundation;
using MonoTouch.Dialog.Extensions;

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
				cell = new SessionInfoCell (_cellIdentifier, tv);
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
			
			
			//TODO: cell.SetImage (ImageUrl);
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
				ImageUrl = "images/Technologies/Other2Small.png";
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
	
	class SessionInfoCell : UITableViewCell, MonoTouch.Dialog.Extensions.IImageUpdated
	{

		//static CGGradient bottomGradient, topGradient;
		public UIButton btnTitle, btnAvatar;
		public UILabel txtRoom, txtSpeaker;
		public static UIFont SmallFont = UIFont.SystemFontOfSize (13f);
		//public UIImageView imageView;
		string imgurl;
		private SessionEntity _session;
		private UITableView _tableView;
		
		//TODO:
//		private static UIImage _cellBackground;
//		public static UIImage CellBackground {
//			get {
//				if (_cellBackground == null) {
//					var url = "images/CellBackground2.png";
//					var imageBackground = new Uri ("file://" + Path.GetFullPath (url));
//					_cellBackground = ImageLoader.DefaultRequestImage (imageBackground, null);
//					//_cellBackground = UIImage.FromFile (url);
//				}
//				
//				return _cellBackground;
//			}
//		}

		public SessionInfoCell (string cellId, UITableView tableView):base(UITableViewCellStyle.Default, cellId)
		{
			//TODO: this.ContentView.BackgroundColor = UIColor.FromPatternImage (SessionInfoCell.CellBackground);
			this.ContentView.BackgroundColor = UIColor.Black;
			_tableView = tableView;
			
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
			
			ImageView.Frame = new Rectangle (0, 0, 48, 48);
			
			//imageView = new UIImageView (new Rectangle (0, 0, 48, 48));
			
			//btnAvatar.AddSubview (imageView);
			btnAvatar.TouchUpInside += HandleUserSelected;
			
			this.AddSubview (txtSpeaker);
			this.AddSubview (btnTitle);
			this.AddSubview (txtRoom);
			this.AddSubview (btnAvatar);

			this.SelectionStyle = UITableViewCellSelectionStyle.None;
		}

		public void SetSession (SessionEntity session)
		{
			_session = session;
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

		public void UpdatedImage (string url, UIImage image)
		{
			if (imgurl != url)
				return;

			this.SetNeedsDisplay ();
		}

		public override void PrepareForReuse ()
		{
			base.PrepareForReuse ();
			this.imgurl = null;
			this.ImageView.Image = null;
			//imageView.Image = null;
		}
		
		UIImage imgBG;
		//ImageLoader _imageLoader = new ImageLoader(20, 4*1024*1024);
		public void SetLocalImage (string url)
		{
			if (!string.IsNullOrEmpty (url)) {
				this.imgurl = url;
				int imageId = 0;
				//var imageBackground = new Uri ("file://" + Path.GetFullPath (url));
				
				imgBG = ImageStore.RequestImage(imageId, "file://" + Path.GetFullPath (url), this);
				//ThreadPool.QueueUserWorkItem (RequestImage, this);

				//var imageLoader = new ImageLoader();
				//_imageLoader.RequestImage(imageBackground, this);
				//var image = ImageLoader.DefaultRequestImage (imageBackground, null);
				//imageView.Image = ImageLoader.DefaultRequestImage (imageBackground, null);	
				//using (imageView.Image) {
				//	imageView.Image = image;
				//}
				
			}
		}
		
		void MonoTouch.Dialog.Extensions.IImageUpdated.UpdatedImage (long onId)
		{
			// Discard notifications that might have been queued for an old cell
			//if(this.id != onId)
				//return;

			imgBG = ImageStore.GetImage (onId);
			this.ImageView.Image = imgBG;
//			InvokeOnMainThread (delegate {
//
//						RefreshImage (sessionInfoCell); 
//						//_tableView.ReloadData ();
//					});
			//ShowMe(root, imgBG);
			
		}
		
		
		private void RequestImage (object state)
		{
			
			
			using (NSAutoreleasePool pool = new NSAutoreleasePool()) { 
				SessionInfoCell sessionInfoCell = state as SessionInfoCell;
				if (sessionInfoCell != null) {
					//NSUrl imageUrl = NSUrl.FromString (controller.ImageUri);
					//NSData imageData = NSData.FromUrl (imageUrl);
					//controller.ImageThumbnail.Image = UIImage.LoadFromData (imageData);
					//var imageBackground = new Uri ("file://" + Path.GetFullPath (sessionInfoCell.imgurl));
					//var image = ImageLoader.DefaultRequestImage (imageBackground, null);
				
					//sessionInfoCell.imageView = new UIImageView (new Rectangle (0, 0, 48, 48));
					//sessionInfoCell.btnAvatar.AddSubview(sessionInfoCell.imageView);
					//btnAvatar.AddSubview (imageView);
				
					//sessionInfoCell.btnAvatar.BackgroundColor = UIColor.FromPatternImage(image);
					UIImage image = null;
					if (!string.IsNullOrEmpty (sessionInfoCell.imgurl)) {
						
						image = UIImage.FromFile (sessionInfoCell.imgurl);
												using (sessionInfoCell.ImageView.Image) {
							sessionInfoCell.ImageView.Image = image;
						}
					}
					//images.Add (controller.ImageUri, controller.ImageThumbnail.Image);

					InvokeOnMainThread (delegate {

						RefreshImage (sessionInfoCell); 
						//_tableView.ReloadData ();
					});
				}
			}
		}

		private void RefreshImage (SessionInfoCell sessionInfoCell)
		{
			sessionInfoCell.SetNeedsDisplay ();
			UIView.BeginAnimations ("imageThumbnailTransitionIn");
			UIView.SetAnimationDuration (0.5f);
			
			sessionInfoCell.ImageView.Alpha = 1.0f;
			UIView.CommitAnimations ();
		}		
		
	}	
}