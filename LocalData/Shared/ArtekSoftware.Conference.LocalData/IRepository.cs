using System.Collections.Generic;

namespace ArtekSoftware.Conference.LocalData
{
  public interface IRepository<T>
  {
    void Save(T entity);
    T Get(string slug);
    IList<T> GetAll();
    void Delete(T entity);
    void Delete(string slug);
  }
}