using System;
using Catnap.Common.Logging;
using Catnap.Database;
using Catnap.Find;
using Catnap.Maps;
using Catnap.Adapters;
using Catnap.Maps.Impl;
using Catnap;
using Catnap.Migration;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.UIKit;
using System.Drawing;

namespace ArtekSoftware.Codemash
{
	public static class Extensions
	{
		public static void ClearTable<T> (this Repository repository) where T:Entity
		{
			
		}
		
		public static UIImage RemoveSharpEdges ( UIImage image, int width, int radius)
		{
			UIGraphics.BeginImageContext (new SizeF (width, width));
			var c = UIGraphics.GetCurrentContext ();
	
			c.BeginPath ();
			c.MoveTo (width, width / 2);
			c.AddArcToPoint (width, width, width / 2, width, radius);
			c.AddArcToPoint (0, width, 0, width / 2, radius);
			c.AddArcToPoint (0, 0, width / 2, 0, radius);
			c.AddArcToPoint (width, 0, width, width / 2, radius);
			c.ClosePath ();
			c.Clip ();
	
			image.Draw (new PointF (0, 0));
			var converted = UIGraphics.GetImageFromCurrentImageContext ();
			UIGraphics.EndImageContext ();
			return converted;
		}
	}
}

