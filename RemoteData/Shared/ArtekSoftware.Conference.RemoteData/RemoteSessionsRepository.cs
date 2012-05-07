using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using RestSharp;
using ArtekSoftware.Conference.Common.Http;

namespace ArtekSoftware.Conference
{
  public class RemoteSessionsRepository : IRemoteRepository<Session>
  {
    private readonly IHttpWebRequestFactory _httpWebRequestFactory = new HttpWebRequestFactory();
    private readonly IRestClient _restClient;
    private readonly IRemoteConfiguration _remoteConfiguration;
    private readonly ITestFlightProxy _testFlightProxy;

    public RemoteSessionsRepository(ITestFlightProxy testFlightProxy, IRestClient restClient, IRemoteConfiguration remoteConfiguration)
    {
      _testFlightProxy = testFlightProxy;
      _restClient = restClient;
      _remoteConfiguration = remoteConfiguration;
    }

    public RemoteSessionsRepository(ITestFlightProxy testFlightProxy, IRestClient restClient, IRemoteConfiguration remoteConfiguration, IHttpWebRequestFactory httpWebRequestFactory)
      : this(testFlightProxy, restClient, remoteConfiguration)
    {
      _httpWebRequestFactory = httpWebRequestFactory;
    }

    public Session Get(string conferenceSlug, string sessionSlug)
    {
      _restClient.BaseUrl = _remoteConfiguration.BaseUrl;

      var request = new RestRequest
      {
        Resource = conferenceSlug + "/sessions/" + sessionSlug,
        RequestFormat = DataFormat.Json,
      };
      var response = _restClient.Execute<Session>(request);
      Session session = null;
      if (response != null && response.Data != null)
      {
        session = (Session)response.Data;
      }
      return session;
    }

    public IList<Session> Get(string conferenceSlug)
    {
      _testFlightProxy.PassCheckpoint("Started RemoteSessionsRepository.GetSessions");

      _restClient.BaseUrl = _remoteConfiguration.BaseUrl;

      var request = new RestRequest
      {
        Resource = conferenceSlug + "/sessions",
        RequestFormat = DataFormat.Json,
      };
      var response = _restClient.Execute<List<Session>>(request);
      var sessions = new List<Session>();
      if (response != null && response.Data != null)
      {
        sessions = response.Data.OrderBy(x => x.title.Trim()).ToList();

      }

      foreach (var session in sessions)
      {
        session.title = session.title.Trim();
      }

      _testFlightProxy.PassCheckpoint("Finished RemoteSessionsRepository.GetSessions");

      return sessions;

    }

    public void Save(string conferenceSlug, Session item)
    {
      _restClient.BaseUrl = _remoteConfiguration.BaseUrl;

      var request = new RestRequest
      {
        Resource = conferenceSlug + "/sessions",
        RequestFormat = DataFormat.Json,
        Method = Method.PUT
      };

      var response = _restClient.Execute(request);
      if (response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.OK)
      {
        throw new Exception("Could not save session. " + response.StatusDescription + " " + response.Content);
      }
    }


    //private IList<SessionOld> GetRegularSessions ()
    //{
    //  IList<SessionOld> sessions;
    //  _testFlightProxy.PassCheckpoint ("Started RemoteSessionsRepository.GetSessions");


    //  _restClient.BaseUrl = "http://codemash.org";

    //  var request = new RestRequest ();
    //  request.Resource = "rest/sessions";
    //  request.RequestFormat = DataFormat.Json;
    //  //using (new NetworkIndicator()) {
    //    var response = _restClient.Execute<SessionsList> (request);
    //    sessions = new List<SessionOld> ();
    //    if (response != null && response.Data != null && response.Data.Sessions != null) {
    //      sessions = response.Data.Sessions.OrderBy (x => x.Title.Trim ()).ToList ();
    //    }
    //  //}

    //  foreach (var session in sessions) {
    //    session.Title = session.Title.Trim ();
    //  }

    //  _testFlightProxy.PassCheckpoint ("Finished RemoteSessionsRepository.GetSessions");

    //  return sessions;
    //}

    //private IList<SessionOld> GetPrecompilerSessions ()
    //{
    //  IList<SessionOld> sessions;
    //  _testFlightProxy.PassCheckpoint ("Started RemoteSessionsRepository.GetSessions");

    //  _restClient.BaseUrl = "http://codemash.org";

    //  var request = new RestRequest ();
    //  request.Resource = "rest/precompiler";
    //  request.RequestFormat = DataFormat.Json;
    //  //using (new NetworkIndicator()) {
    //    var response = _restClient.Execute<SessionsList> (request);
    //    sessions = new List<SessionOld> ();
    //    if (response != null && response.Data != null && response.Data.Sessions != null) {
    //      sessions = response.Data.Sessions.OrderBy (x => x.Title.Trim ()).ToList ();
    //    }
    //  //}

    //  foreach (var session in sessions) {
    //    session.Title = session.Title.Trim ();
    //  }

    //  _testFlightProxy.PassCheckpoint ("Finished RemoteSessionsRepository.GetSessions");

    //  return sessions;
    //}		

  }
}