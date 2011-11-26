using System;
using MonoTouch.UIKit;

namespace ArtekSoftware.Codemash
{
	public class NetworkIndicator : IDisposable
	{
		public NetworkIndicator ()
		{
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
		}

		public void Dispose ()
		{
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
			GC.SuppressFinalize (this);
		}
	}
}

