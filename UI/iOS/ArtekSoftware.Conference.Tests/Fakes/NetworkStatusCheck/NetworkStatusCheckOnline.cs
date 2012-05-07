using System;
//using MonoQueue;

namespace ArtekSoftware.Codemash.Tests
{
	public class NetworkStatusCheckOnline : INetworkStatusCheck
	{
		#region INetworkStatusCheck implementation
		public bool IsReachable ()
		{
			return true;
		}

		public bool IsReachable (string host)
		{
			return true;
		}
		#endregion
	}
}

