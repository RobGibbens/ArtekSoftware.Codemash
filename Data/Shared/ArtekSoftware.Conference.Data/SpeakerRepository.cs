using System;
using ArtekSoftware.Conference.LocalData;
using RestSharp;
using System.Collections.Generic;
using Vici.CoolStorage;
using System.Linq;
using ArtekSoftware.Conference.RemoteData;

namespace ArtekSoftware.Conference.Data
{
	public class SpeakerRepository
	{
		private IRemoteRepository<Speaker> _remoteRepository;
	  private readonly IRepository<SpeakerEntity> _localRepository;
	  private INetworkStatusCheck _networkStatusCheck;
		private ITestFlightProxy _testFlightProxy;
		private IRestClient _restClient;
		private IRemoteConfiguration _remoteConfiguration;
		private string _conferenceSlug;
		
		public SpeakerRepository (IRemoteRepository<Speaker> remoteRepository, IRepository<SpeakerEntity> localRepository,
		                   INetworkStatusCheck networkStatusCheck, ITestFlightProxy testFlightProxy, 
		                   IRestClient restClient, IRemoteConfiguration remoteConfiguration, string conferenceSlug)
		{
			_remoteRepository = remoteRepository;
		  _localRepository = localRepository;
		  _networkStatusCheck = networkStatusCheck;
			_testFlightProxy = testFlightProxy;
			_restClient = restClient;
			_remoteConfiguration = remoteConfiguration;
			_conferenceSlug = conferenceSlug;
		}
		
		public IEnumerable<SpeakerEntity> GetEntities(bool isRefresh)
		{
      var count = SpeakerEntity.List().CountFast;
      var entities = new List<SpeakerEntity>();

      if (count == 0 || isRefresh)
      {
        if (_networkStatusCheck.IsReachable())
        {

          var dtos = _remoteRepository.Get(_conferenceSlug);

          if (dtos != null && dtos.Count > 0)
          {
            _localRepository.DeleteAll();
            //Map dtos to entities
            var map = new MapSpeaker();
            foreach (var dto in dtos)
            {
              var existingEntity = SpeakerEntity.ReadSafe(dto.Slug);

              if (existingEntity == null)
              {
                var entity = map.Map(dto);
                entities.Add(entity);
                _localRepository.Save(entity);
              }
              else
              {
                existingEntity = map.Copy(existingEntity, dto);
                _localRepository.Save(existingEntity);
                entities.Add(existingEntity);
              }
              //Save to _repository
            }
          }
        }
        else
        {
          //TODO : Raise event?
          //ModalDialog.Alert ("Network offline", "Cannot connect to the network");
        }
      }
      else
      {
        entities = SpeakerEntity.List().ToList().OrderBy(x => x.Name).ToList();
        return entities;
      }

      return entities;
		}

	}
}

