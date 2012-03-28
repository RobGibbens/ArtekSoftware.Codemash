using System;
using MonoTouch.Foundation;
////using MonoQueue;

namespace ArtekSoftware.Codemash
{
	[Serializable]
	public enum NetworkStatus
	{
		NotReachable,
		ReachableViaCarrierDataNetwork,
		ReachableViaWiFiNetwork
	}
	
	public interface INetworkStatusCheck
	{
		bool IsReachable();
		bool IsReachable(string host);
	}
	[Serializable]
	public class NetworkStatusCheck : INetworkStatusCheck
	{
		public bool IsReachable ()
		{
			return IsReachable ("codemash.org");
		}
	
		public bool IsReachable (string host)
		{ 
			if (host != null && host.ToLower ().StartsWith("http://"))
			{
				host = host.ToLower ().Replace ("http://", "");
			}
			return Reachability.InternetConnectionStatus () != NetworkStatus.NotReachable && Reachability.IsHostReachable (host); 
		}

		private void ThreadedIsOnline (object state)
		{
			using (NSAutoreleasePool pool = new NSAutoreleasePool()) {
				((ReachabilityStatus)state).IsOnline = Reachability.RemoteHostStatus () != NetworkStatus.NotReachable;
			}
		}

		private class ReachabilityStatus
		{
			public bool IsOnline;
		}	
	}
}

