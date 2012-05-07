using System;

namespace ArtekSoftware.Codemash.Tests
{
	public class TestFlightProxyConsole : ITestFlightProxy
	{
		#region ITestFlightProxy implementation
		public void TakeOff ()
		{
			Console.WriteLine("TakeOff called");
		}

		public void PassCheckpoint (string checkpointName)
		{
			Console.WriteLine("PassCheckpoint called " + checkpointName);
		}
		#endregion
	}
}

