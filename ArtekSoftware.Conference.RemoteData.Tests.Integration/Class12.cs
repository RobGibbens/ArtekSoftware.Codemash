using System;
using NUnit.Framework;
using ArtekSoftware.Codemash;
using RestSharp;
using Should.Core;
using Should;

namespace ArtekSoftware.Conference.RemoteData.Tests.Integration
{
	[TestFixture]
	public class Tests
	{
		[TestFixtureSetUp]
		public void Setup ()
		{
		}
		
		[Test]
		public void sessions_should_return_valid_data ()
		{
			var conferenceSlug = "MobiDevDay-2012";
			var testFlightProxy = new TestFlightProxy ();
			var restClient = new RestClient ();
			//Assert.True(1 == 1);
			
			var repository = new RemoteSessionsRepository (testFlightProxy, restClient);
			var sessions = repository.GetSessions (conferenceSlug);
			sessions.Count.ShouldEqual(7);
		}
	}
}

