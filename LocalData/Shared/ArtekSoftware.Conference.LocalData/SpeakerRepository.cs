using System.Collections.Generic;

namespace ArtekSoftware.Conference.LocalData
{
  public class SpeakerRepository : IRepository<SpeakerEntity>
  {
    public void Save(SpeakerEntity entity)
    {
      entity.Save();
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
  }
}