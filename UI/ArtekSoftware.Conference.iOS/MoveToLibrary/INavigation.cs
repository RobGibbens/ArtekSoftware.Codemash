using System;

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