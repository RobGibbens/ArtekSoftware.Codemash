using System;
using MonoTouch.TestFlight;

namespace ArtekSoftware.Codemash
{
	public static class TestFlightProxy
	{
		public static void TakeOff(string teamToken)
		{
#if !DEBUG && !SIMULATOR
			TestFlight.TakeOff(teamToken);
#endif
		}
		
		public static void PassCheckpoint(string checkpointName)
		{
#if !DEBUG && !SIMULATOR
			TestFlight.PassCheckpoint(checkpointName);
#endif
		}
	}
}

