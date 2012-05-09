using System.Collections.Generic;
using Vici.CoolStorage;

namespace ArtekSoftware.Conference.LocalData
{
  public class LocalSpeakerRepository : IRepository<SpeakerEntity>
  {
    public void Save(SpeakerEntity entity)
    {
      if (!string.IsNullOrWhiteSpace(entity.Slug))
      {
        entity.Description = entity.Description.Replace("’", "''");
        entity.Slug = entity.Slug.Replace("’", "''");
        entity.Name = entity.Name.Replace("’", "''");
        entity.LastName = entity.LastName.Replace("’", "''");
        
        entity.Save();
      }
    }

    public SpeakerEntity Get(string slug)
    {
      var speaker = SpeakerEntity.ReadFirst("Slug=@Slug", "@Slug", slug);

      return speaker;
    }

    public IList<SpeakerEntity> GetAll()
    {
      return SpeakerEntity.List();
    }

    public void Delete(SpeakerEntity entity)
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
      CSDatabase.ExecuteNonQuery("DELETE FROM " + SpeakerEntity.TableName);
    }
  }
}