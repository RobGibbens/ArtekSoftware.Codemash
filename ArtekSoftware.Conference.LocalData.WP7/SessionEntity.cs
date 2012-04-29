using System;
using System.Collections.Generic;
using System.Linq;
using Catnap;

namespace ArtekSoftware.Conference.LocalData
{
  public class SessionRepository
  {
    public SessionEntity Get(string slug)
    {
      SessionEntity session = null;
      using (var uow = UnitOfWork.Start())
      {
        var criteria = Criteria.For<SessionEntity>()
          .Equal(x => x.Slug, slug);
        var sessions = uow.Session.List(criteria);
        if (sessions != null)
        {
          session = sessions.FirstOrDefault();
        }
      }

      return session;
    }
  }

  public class SessionEntity
  {
    public static string TableName = "Sessions";

    public static string CreateTableSql = @"CREATE TABLE " + TableName +
                                          " (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, URI VARCHAR, Title VARCHAR,Abstract VARCHAR, Start VARCHAR, Room VARCHAR, Difficulty VARCHAR, SpeakerName VARCHAR, Technology VARCHAR, SpeakerURI VARCHAR)";

    public string Slug { get; set; }
    public string Id { get; set; }
    public string ConferenceSlug { get; set; }
    public string Abstract { get; set; }
    public string Difficulty { get; set; }
    public List<string> Subjects { get; set; }
    public string TwitterHashTag { get; set; }
    public string Title { get; set; }
    public List<SpeakerEntity> Speakers { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string Room { get; set; }
  }
}