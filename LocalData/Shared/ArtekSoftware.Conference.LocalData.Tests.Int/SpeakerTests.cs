using System;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Should;

namespace ArtekSoftware.Conference.LocalData.WP7.Tests.Int
{
  [TestFixture]
  public class SpeakerTests : TestBase
  {
    [TestFixtureSetUp]
    public void Setup()
    {
      base.Setup();
    }

    [Test]
    public void should_be_able_to_create_new_entity()
    {
      var speaker = CreateEntity();
      speaker.ShouldNotBeNull();
      Assert.True(true);
    }

    [Test]
    public void should_be_able_to_get_existing_entity()
    {
      var speakerEntity = CreateEntity();
      var repository = new LocalSpeakerRepository();
      var speaker = repository.Get(speakerEntity.Slug);
      speaker.ShouldNotBeNull();
    }

    [Test]
    public void should_be_able_to_get_list_of_entities()
    {
      var repository = new LocalSpeakerRepository();
      var entities = repository.GetAll();
      entities.ShouldNotBeNull();
      entities.Count.ShouldBeInRange(1, int.MaxValue);
    }

    [Test]
    public void should_be_able_to_update_entity()
    {
      throw new NotImplementedException();
    }

    [Test]
    public void should_be_able_to_delete_entity()
    {
      throw new NotImplementedException();
    }


    [Test]
    public void should_be_able_to_query_by_properties()
    {
      throw new NotImplementedException();
    }
    
    private SpeakerEntity CreateEntity()
    {
      var repository = new LocalSpeakerRepository();
      var speakerEntity = Fixture.CreateAnonymous<SpeakerEntity>();
      repository.Save(speakerEntity);

      return speakerEntity;
    }  
  }
}