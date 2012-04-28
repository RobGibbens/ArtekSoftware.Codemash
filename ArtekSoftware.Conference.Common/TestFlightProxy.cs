using System;
//using MonoTouch.TestFlight;

namespace ArtekSoftware.Conference
{
	public interface ITestFlightProxy
	{
		void TakeOff();
		void PassCheckpoint(string checkpointName);
	}
	
	public class TestFlightProxy : ITestFlightProxy
	{
		
		public void TakeOff()
		{
#if !DEBUG && !SIMULATOR
			string teamToken = "19a8eedfedeed47cf1f6d74fd7ab561c_MTkxNDIwMTEtMDktMjkgMjE6MTc6MTAuNjM0NTAw";
			//TestFlight.TakeOff(teamToken);
#endif
		}
		
		public void PassCheckpoint(string checkpointName)
		{
#if !DEBUG && !SIMULATOR
			//TestFlight.PassCheckpoint(checkpointName);
#endif
		}
	}
}

