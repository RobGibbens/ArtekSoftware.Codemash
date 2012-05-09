using System.Collections.Generic;
using Vici.CoolStorage;

namespace ArtekSoftware.Conference.LocalData
{
  public class LocalSessionRepository : IRepository<SessionEntity>
  {
    public void Save(SessionEntity entity)
    {
      entity.Save();
    }
    
    public SessionEntity Get(string slug)
    {
      var session = SessionEntity.ReadFirst("Slug=@Slug", "@Slug", slug);

      return session;
    }

    public IList<SessionEntity> GetAll()
    {
      return SessionEntity.List();
    }

    public void Delete(SessionEntity entity)
    {
      entity.Delete();
    }

    public void Delete(string slug)
    {
      var entity = Get(slug);
      entity.Delete();
    }

    public void DeleteAll()
    {
      CSDatabase.ExecuteNonQuery("DELETE FROM " + SessionEntity.TableName);
    }
  }
}