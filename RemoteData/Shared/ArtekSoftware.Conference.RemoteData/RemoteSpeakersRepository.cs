using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using RestSharp;
using ArtekSoftware.Conference.Common.Http;
using ArtekSoftware.Conference.RemoteData;

namespace ArtekSoftware.Conference
{
  public class RemoteSpeakersRepository : IRemoteRepository<Speaker>
  {
    private readonly IHttpWebRequestFactory _httpWebRequestFactory = new HttpWebRequestFactory();
    private readonly IRestClient _restClient;
    private readonly IRemoteConfiguration _remoteConfiguration;
    private readonly ITestFlightProxy _testFlightProxy;

    public RemoteSpeakersRepository(ITestFlightProxy testFlightProxy, IRestClient restClient, IRemoteConfiguration remoteConfiguration)
    {
      _testFlightProxy = testFlightProxy;
      _restClient = restClient;
      _remoteConfiguration = remoteConfiguration;
    }

    public RemoteSpeakersRepository(ITestFlightProxy testFlightProxy, IRestClient restClient, IRemoteConfiguration remoteConfiguration, IHttpWebRequestFactory httpWebRequestFactory)
      : this(testFlightProxy, restClient, remoteConfiguration)
    {
      _httpWebRequestFactory = httpWebRequestFactory;
    }

    public Speaker Get(string conferenceSlug, string speakerSlug)
    {
      _restClient.BaseUrl = _remoteConfiguration.BaseUrl;

      var request = new RestRequest
      {
        Resource = conferenceSlug + "/speakers/" + speakerSlug,
        RequestFormat = DataFormat.Json,
      };
      var response = _restClient.Execute<Speaker>(request);
      Speaker speaker = null;
      if (response != null && response.Data != null)
      {
        speaker = (Speaker)response.Data;
      }
      return speaker;
    }

    public IList<Speaker> Get(string conferenceSlug)
    {
      _testFlightProxy.PassCheckpoint("Started RemoteSpeakersRepository.Get");

      _restClient.BaseUrl = _remoteConfiguration.BaseUrl;

      var request = new RestRequest
      {
        Resource = conferenceSlug + "/speakers",
        RequestFormat = DataFormat.Json,
      };
      var response = _restClient.Execute<List<Speaker>>(request);
      var speakers = new List<Speaker>();
      if (response != null && response.Data != null)
      {
        speakers = response.Data.OrderBy(x => x.Name.Trim()).ToList();
      }

      foreach (var speaker in speakers)
      {
        speaker.Name = speaker.Name.Trim();
      }

      _testFlightProxy.PassCheckpoint("Finished RemoteSpeakersRepository.Get");

      return speakers;

    }

    public void Save(string conferenceSlug, Speaker item)
    {
      _restClient.BaseUrl = _remoteConfiguration.BaseUrl;

      var request = new RestRequest
      {
        Resource = conferenceSlug + "/speakers",
        RequestFormat = DataFormat.Json,
        Method = Method.PUT
      };

      var response = _restClient.Execute(request);
      if (response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.OK)
      {
        throw new Exception("Could not save speaker. " + response.StatusDescription + " " + response.Content);
      }
    }

  }

}