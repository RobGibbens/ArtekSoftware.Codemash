using System;
using ArtekSoftware.Conference.LocalData;
using RestSharp;
using System.Collections.Generic;
using Vici.CoolStorage;
using System.Linq;

namespace ArtekSoftware.Conference.Data
{
	public class ScheduleRepository
	{
		private IRemoteRepository<Schedule> _remoteRepository;
		private INetworkStatusCheck _networkStatusCheck;
		private ITestFlightProxy _testFlightProxy;
		private IRestClient _restClient;
		private IRemoteConfiguration _remoteConfiguration;
		private string _conferenceSlug;
		
		public ScheduleRepository (IRemoteRepository<Schedule> remoteRepository, 
		                   INetworkStatusCheck networkStatusCheck, ITestFlightProxy testFlightProxy, 
		                   IRestClient restClient, IRemoteConfiguration remoteConfiguration, string conferenceSlug)
		{
			_remoteRepository = remoteRepository;
			_networkStatusCheck = networkStatusCheck;
			_testFlightProxy = testFlightProxy;
			_restClient = restClient;
			_remoteConfiguration = remoteConfiguration;
			_conferenceSlug = conferenceSlug;
		}
		
		public IEnumerable<ScheduleEntity> GetEntities(bool isRefresh)
		{
			var count = ScheduleEntity.List().CountFast;

			if (count == 0 || isRefresh) {
				if (_networkStatusCheck.IsReachable ()) {

					var dtos = _remoteRepository.Get (_conferenceSlug);
					
					if (dtos != null && dtos.Count > 0) {
						//Map dtos to entities
						//Save to _repository
					}
				} else {
					//TODO : Raise event?
					//ModalDialog.Alert ("Network offline", "Cannot connect to the network");
				}
			} else {
				//TODO: OrderBy
				var entities = ScheduleEntity.List().ToList();
				return entities;
			}
			
			return new List<ScheduleEntity>();
		}

	}
}

