using System;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Should;

namespace ArtekSoftware.Conference.LocalData.WP7.Tests.Int
{
  [TestFixture]
  public class SessionTests : TestBase
  {
    [TestFixtureSetUp]
    public void Setup()
    {
      base.Setup();
    }

    [Test]
    public void should_be_able_to_create_new_entity()
    {
      var session = CreateEntity();
      session.ShouldNotBeNull();
      Assert.True(true);
    }

    [Test]
    public void should_be_able_to_get_existing_entity()
    {
      var entity = CreateEntity();
      var repository = new SessionRepository();
      var session = repository.Get(entity.Slug);
      session.ShouldNotBeNull();
    }

    [Test]
    public void should_be_able_to_get_list_of_entities()
    {
      var repository = new SessionRepository();
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
    
    private SessionEntity CreateEntity()
    {
      var repository = new SessionRepository();
      var entity = Fixture.CreateAnonymous<SessionEntity>();
      repository.Save(entity);

      return entity;
    }
  }
}