using System.Collections.Generic;

namespace ArtekSoftware.Conference
{
  public interface IRemoteRepository<T> where T:class 
  {
    T Get(string conferenceSlug, string slug);
    IList<T> Get(string conferenceSlug);
    void Save(string conferenceSlug, T item);
  }
}