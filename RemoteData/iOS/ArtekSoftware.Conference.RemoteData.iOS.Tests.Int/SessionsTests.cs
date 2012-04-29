using NUnit.Framework;
using Ploeh.AutoFixture;
using RestSharp;
using Should;

namespace ArtekSoftware.Conference.RemoteData.Tests.Integration
{
  [TestFixture]
  public class SessionTests
  {
    private IFixture _fixture;
    private string _baseUrl;

    [TestFixtureSetUp]
    public void Setup()
    {
      _fixture = new Fixture().Customize(new MultipleCustomization());
      //_baseUrl = "http://conference.apphb.com/api/";
      _baseUrl = "http://localhost:1498/api/";
    }

    [Test]
    public void should_get_single_session()
    {
      var conferenceSlug = "MobiDevDay-2012";
      string sessionSlug = "the-state-of-enterprise-mobile";
      var testFlightProxy = new TestFlightProxy();
      var restClient = new RestClient();

      IRemoteConfiguration remoteConfiguration = new RemoteConfiguration()
                                                   {
                                                     BaseUrl = _baseUrl
                                                   };
      var repository = new RemoteSessionsRepository(testFlightProxy, restClient, remoteConfiguration);
      var session = repository.Get(conferenceSlug, sessionSlug);
      session.ShouldNotBeNull();
    }


    [Test]
    public void sessions_should_return_valid_data()
    {
      var conferenceSlug = "MobiDevDay-2012";
      var testFlightProxy = new TestFlightProxy();
      var restClient = new RestClient();

      IRemoteConfiguration remoteConfiguration = new RemoteConfiguration()
      {
        BaseUrl = _baseUrl
      };
      var repository = new RemoteSessionsRepository(testFlightProxy, restClient, remoteConfiguration);
      var sessions = repository.Get(conferenceSlug);
      sessions.Count.ShouldEqual(7);
    }

    [Test]
    public void should_create_new_session()
    {
      var session = _fixture.CreateAnonymous<Session>();
      var conferenceSlug = "d7152699-cb20-41a4-965f-c55870da596b";
      var testFlightProxy = new TestFlightProxy();
      var restClient = new RestClient();
      IRemoteConfiguration remoteConfiguration = new RemoteConfiguration()
      {
        BaseUrl = _baseUrl
      };
      var repository = new RemoteSessionsRepository(testFlightProxy, restClient, remoteConfiguration);
      repository.Save(conferenceSlug, session);
      Assert.True(true);
    }

    [Test]
    [Ignore]
    public void should_not_be_able_to_post_to_existing_session()
    {
      Assert.Fail();
    }

    [Test]
    [Ignore]
    public void should_create_new_session_with_put()
    {
      Assert.Fail();
    }

    [Test]
    [Ignore]
    public void should_update_existing_session_with_put()
    {
      Assert.Fail();
    }

    [Test]
    [Ignore]
    public void should_delete_session()
    {
      Assert.Fail();
    }


  }
}

