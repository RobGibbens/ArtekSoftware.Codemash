using System;
using System.Collections.Generic;

namespace ArtekSoftware.Conference.RemoteData
{
  public class Speaker
  {
    public string ConferenceSlug  { get; set; }
    public string TwitterName  { get; set; }
    public string BlogUrl  { get; set; }
    public string CompanyName  { get; set; }
    public string LinkedInUrl  { get; set; }
    public string FacebookUrl  { get; set; }
    public IList<Guid> SessionsIds  { get; set; }
    public string Name { get; protected internal set; }
    public string FirstName  { get; set; }
    public string LastName  { get; set; }
  }
}
