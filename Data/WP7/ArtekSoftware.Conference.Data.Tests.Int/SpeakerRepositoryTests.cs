using System;
using System.IO;
using System.Linq;
using ArtekSoftware.Conference.LocalData;
using NUnit.Framework;
using RestSharp;
using Moq;
using Ploeh.AutoFixture;
using Should;
using Vici.CoolStorage;
using ArtekSoftware.Conference.Data;

namespace ArtekSoftware.Conference.Data.Tests.Int
{
  [TestFixture]
  public class SpeakerRepositoryTests
  {
    protected IFixture Fixture;

    [TestFixtureSetUp]
    public virtual void Setup()
    {
      Fixture = new Fixture().Customize(new MultipleCustomization());

      CreateDatabase();
    }

    [Test]
    public void Test()
    {
      var networkStatusCheck = new Mock<INetworkStatusCheck>();
      networkStatusCheck.Setup(x => x.IsReachable()).Returns(true);
      networkStatusCheck.Setup(x => x.IsReachable("codemash.org")).Returns(true);

      var testFlightProxy = new TestFlightProxy();
      var localRepository = new LocalSpeakerRepository();
      var restClient = new RestClient();
      var remoteConfiguration = new RemoteConfiguration()
                                  {
                                    BaseUrl = "http://conference.apphb.com/api"
                                  };
      var remoteRepository = new RemoteSpeakersRepository(testFlightProxy, restClient, remoteConfiguration);
      var conferenceSlug = "MobiDevDay-2012";

      var repository = new SpeakerRepository(remoteRepository, localRepository, networkStatusCheck.Object, testFlightProxy, restClient, remoteConfiguration, conferenceSlug);
      
      var entities = repository.GetEntities(isRefresh: true);
      entities.ShouldNotBeNull();
      entities.Count().ShouldBeInRange(1, int.MaxValue);
    }


    private void CreateDatabase()
    {
      var personalFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
      string dbName = Path.Combine(personalFolder, "conference.db3");
      CSConfig.SetDB(new CSDataProviderSQLite("Data Source=" + dbName));

      CSDatabase.ExecuteNonQuery(SpeakerEntity.CreateTableSql);
      CSDatabase.ExecuteNonQuery(SpeakerEntity.CreateTableSql);
    }

  }
}
