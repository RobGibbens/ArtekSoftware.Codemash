using System;
using System.Collections.Generic;
using System.Linq;
using Catnap;

namespace ArtekSoftware.Conference.LocalData
{
  public class SpeakerRepository
  {
    public SpeakerEntity Get(string slug)
    {
      SpeakerEntity speaker = null;
      using (var uow = UnitOfWork.Start())
      {
        var criteria = Criteria.For<SpeakerEntity>()
          .Equal(x => x.Slug, slug);
        var speakers = uow.Session.List(criteria);
        if (speakers != null)
        {
          speaker = speakers.FirstOrDefault();
        }
      }

      return speaker;
    }
  }

  public class SpeakerEntity
  {
    public static string TableName = "Speakers";

    public static string CreateTableSql = @"CREATE TABLE " + TableName +
                                          " (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, URI VARCHAR, Title VARCHAR,Abstract VARCHAR, Start VARCHAR, Room VARCHAR, Difficulty VARCHAR, SpeakerName VARCHAR, Technology VARCHAR, SpeakerURI VARCHAR)";

    public string Slug { get; set; }
    public string ConferenceSlug { get; set; }
    public string TwitterName { get; set; }
    public string BlogUrl { get; set; }
    public string CompanyName { get; set; }
    public string LinkedInUrl { get; set; }
    public string FacebookUrl { get; set; }
    public IList<Guid> SessionsIds { get; set; }
    public string Name { get; protected internal set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
  }
}