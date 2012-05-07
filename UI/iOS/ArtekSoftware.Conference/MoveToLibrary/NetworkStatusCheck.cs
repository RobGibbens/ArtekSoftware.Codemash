using System;
using ArtekSoftware.Conference;
using MonoTouch.Foundation;

namespace ArtekSoftware.Codemash
{
    public class ReachabilityStatus
    {
        public bool IsOnline;
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
	}
}