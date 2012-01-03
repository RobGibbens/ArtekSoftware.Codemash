using System;

namespace ArtekSoftware.Codemash
{
	public interface IAnalytics
	{
		void Initialize();
		void Log(string message);
		void Close();
		void Resume();
	}
}