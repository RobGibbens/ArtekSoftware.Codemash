using ArtekSoftware.Conference.LocalData;
using ArtekSoftware.Conference.RemoteData;

namespace ArtekSoftware.Conference.Data
{
  public class MapSession
  {
    public SessionEntity Map(Session dto)
    {
      var entity = new SessionEntity()
                     {
                       Abstract = dto.@abstract,
                       ConferenceSlug = dto.conferenceSlug,
                       Difficulty = dto.difficulty,
                       End = dto.end,
                       //Id = dto.id.ToString(),
                       //TODO : ImageUrl = dto.imageurl,
                       Room = dto.room,
                       Slug = dto.slug,
                       Start = dto.start,
                       Title = dto.title,
                       TwitterHashTag = dto.twitterHashTag
                     };

      return entity;
    }
    public SessionEntity Copy(SessionEntity entity, Session dto)
    {
      entity.Abstract = dto.@abstract;
      entity.ConferenceSlug = dto.conferenceSlug;
      entity.Difficulty = dto.difficulty;
      entity.End = dto.end;
      //Id = dto.id.ToString();
      //TODO : ImageUrl = dto.imageurl;
      entity.Room = dto.room;
      entity.Slug = dto.slug;
      entity.Start = dto.start;
      entity.Title = dto.title;
      entity.TwitterHashTag = dto.twitterHashTag;

      return entity;
    }
    public Session Map(SessionEntity entity)
    {
      var dto = new Session()
                  {
                    @abstract = entity.Abstract,
                    conferenceSlug = entity.ConferenceSlug,
                    difficulty = entity.Difficulty,
                    end = entity.End,
                    //id = entity.Id,
                    room = entity.Room,
                    slug = entity.Slug,
                    //TODO: speakers = entity.Speakers,
                    start = entity.Start,
                    //TODO:subjects = entity.subjects,
                    title = entity.Title,
                    twitterHashTag = entity.TwitterHashTag
                  };

      return dto;
    }
  }

  public class MapSpeaker
  {
    public SpeakerEntity Map(Speaker dto)
    {
      var entity = new SpeakerEntity()
      {
        Description = dto.Description,
        BlogUrl = dto.BlogUrl,
        CompanyName = dto.CompanyName,
        ConferenceSlug = dto.ConferenceSlug,
        FacebookUrl = dto.FacebookUrl,
        FirstName = dto.FirstName,
        //Id = dto.id,
        LastName = dto.LastName,
        LinkedInUrl = dto.LinkedInUrl,
        Slug = dto.Slug,
        TwitterName = dto.TwitterName
      };

      return entity;
    }

    public SpeakerEntity Copy(SpeakerEntity entity, Speaker dto)
    {
      entity.Description = dto.Description;
      entity.BlogUrl = dto.BlogUrl;
      entity.CompanyName = dto.CompanyName;
      entity.ConferenceSlug = dto.ConferenceSlug;
      //Id = dto.id.ToString();
      //TODO : ImageUrl = dto.imageurl;
      entity.FacebookUrl = dto.FacebookUrl;
      entity.FirstName = dto.FirstName;
      entity.LastName = dto.LastName;
      entity.LinkedInUrl = dto.LinkedInUrl;
      entity.Slug = dto.Slug;
      entity.TwitterName = dto.TwitterName;

      return entity;
    }
    public Speaker Map(SpeakerEntity entity)
    {
      var dto = new Speaker()
      {
        Description = entity.Description,
        BlogUrl = entity.BlogUrl,
        CompanyName = entity.CompanyName,
        ConferenceSlug = entity.ConferenceSlug,
        //id = entity.Id,
        FacebookUrl = entity.FacebookUrl,
        FirstName = entity.FirstName,
        //TODO: speakers = entity.Speakers,
        LastName= entity.LastName,
        //TODO:subjects = entity.subjects,
        //title = entity.Name,
        Slug = entity.Slug,
        TwitterName = entity.TwitterName
      };

      return dto;
    }
  }

}