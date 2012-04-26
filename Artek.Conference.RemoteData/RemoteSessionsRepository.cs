using System.Collections.Generic;
using System.Linq;
using RestSharp;
using System.Net;
using System;
using System.IO;
using System.Text;
using ArtekSoftware.Conference.Common.Http;
using RestSharp.Serializers;

namespace ArtekSoftware.Codemash
{
  public class RemoteSessionsRepository
  {
    private readonly IHttpWebRequestFactory _httpWebRequestFactory = new HttpWebRequestFactory();
    private readonly IRestClient _restClient;
    private readonly ITestFlightProxy _testFlightProxy;

    public RemoteSessionsRepository(ITestFlightProxy testFlightProxy, IRestClient restClient)
    {
      _testFlightProxy = testFlightProxy;
      _restClient = restClient;
    }

    public RemoteSessionsRepository(ITestFlightProxy testFlightProxy, IRestClient restClient, IHttpWebRequestFactory httpWebRequestFactory)
      : this(testFlightProxy, restClient)
    {
      _httpWebRequestFactory = httpWebRequestFactory;
    }


    public IList<Session2> GetSessions(string conferenceSlug)
    {
      _testFlightProxy.PassCheckpoint("Started RemoteSessionsRepository.GetSessions");

      _restClient.BaseUrl = "http://conference.apphb.com/api/";

      var request = new RestRequest
      {
        Resource = "MobiDevDay-2012/sessions?format=json",
        RequestFormat = DataFormat.Json,
      };
      var response = _restClient.Execute<List<Session2>>(request);
      var sessions = new List<Session2>();
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


    //private IList<Session> GetRegularSessions ()
    //{
    //  IList<Session> sessions;
    //  _testFlightProxy.PassCheckpoint ("Started RemoteSessionsRepository.GetSessions");


    //  _restClient.BaseUrl = "http://codemash.org";

    //  var request = new RestRequest ();
    //  request.Resource = "rest/sessions";
    //  request.RequestFormat = DataFormat.Json;
    //  //using (new NetworkIndicator()) {
    //    var response = _restClient.Execute<SessionsList> (request);
    //    sessions = new List<Session> ();
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

    //private IList<Session> GetPrecompilerSessions ()
    //{
    //  IList<Session> sessions;
    //  _testFlightProxy.PassCheckpoint ("Started RemoteSessionsRepository.GetSessions");

    //  _restClient.BaseUrl = "http://codemash.org";

    //  var request = new RestRequest ();
    //  request.Resource = "rest/precompiler";
    //  request.RequestFormat = DataFormat.Json;
    //  //using (new NetworkIndicator()) {
    //    var response = _restClient.Execute<SessionsList> (request);
    //    sessions = new List<Session> ();
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