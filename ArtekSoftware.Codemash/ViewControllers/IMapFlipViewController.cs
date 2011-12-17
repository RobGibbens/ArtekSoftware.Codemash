using System;
using MonoTouch.CoreLocation;
using MonoTouch.UIKit;

namespace ArtekSoftware.Codemash
{
	public interface IMapFlipPadViewController : IMapFlipViewController
	{
		UIToolbar Toolbar {get;}
	}
	
	public interface IMapFlipViewController
	{
		void Flip(CLLocationCoordinate2D toLocation);
		void Flip();
	}
}