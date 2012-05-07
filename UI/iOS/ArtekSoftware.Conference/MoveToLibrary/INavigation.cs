using System;
using ArtekSoftware.Conference.LocalData;

namespace ArtekSoftware.Codemash
{
	public interface INavigation
	{
		void SetSession(SessionEntity session);
		void SetSpeaker (SpeakerEntity speaker);
		void SetLocationMap ();
		void SetMap ();
	}
}