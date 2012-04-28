using System;

namespace ArtekSoftware.Conference
{
	public interface IAnalytics
	{
		void Initialize();
		void Log(string message);
		void Close();
		void Resume();
	}
}