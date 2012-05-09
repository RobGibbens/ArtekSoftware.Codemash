using System;
using System.Collections.Generic;
using ArtekSoftware.Conference.RemoteData;

namespace ArtekSoftware.Conference
{
  public class Session
  {

    public Guid id { get; set; }
    public string conferenceSlug { get; set; }
    public string @abstract { get; set; }
    public string difficulty { get; set; }
    public List<string> subjects { get; set; }
    public string twitterHashTag { get; set; }
    public string slug { get; set; }
    public string title { get; set; }
    public List<Speaker> speakers { get; set; }
    public DateTime start { get; set; }
    public DateTime end { get; set; }
    public string room { get; set; }
  }
}

