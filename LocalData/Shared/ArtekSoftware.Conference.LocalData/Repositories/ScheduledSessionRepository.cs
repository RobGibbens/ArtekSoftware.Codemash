using System;
using System.Collections.Generic;

namespace ArtekSoftware.Conference.LocalData
{
public class LocalScheduledSessionRepository : IRepository<ScheduledSessionEntity>
  {
    public void Save(ScheduledSessionEntity entity)
    {
      entity.Save();
    }
    
    public ScheduledSessionEntity Get(string slug)
    {
      var session = ScheduledSessionEntity.ReadFirst("Slug=@Slug", "@Slug", slug);

      return session;
    }

    public IList<ScheduledSessionEntity> GetAll()
    {
      return ScheduledSessionEntity.List();
    }

    public void Delete(ScheduledSessionEntity entity)
    {
      entity.Delete();
    }

    public void Delete(string slug)
    {
      var entity = Get(slug);
      entity.Delete();
    }
  }
}

