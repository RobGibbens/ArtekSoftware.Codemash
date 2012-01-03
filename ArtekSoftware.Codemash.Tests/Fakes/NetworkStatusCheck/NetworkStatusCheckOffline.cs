using System;
using MonoQueue;

namespace ArtekSoftware.Codemash.Tests
{
	public class NetworkStatusCheckOffline : INetworkStatusCheck
	{
		#region INetworkStatusCheck implementation
		public bool IsReachable ()
		{
			return false;
		}

		public bool IsReachable (string host)
		{
			return false;
		}
		#endregion
	}
}

