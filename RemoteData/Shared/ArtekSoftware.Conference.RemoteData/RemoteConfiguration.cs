using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using RestSharp;
using ArtekSoftware.Conference.Common.Http;

namespace ArtekSoftware.Conference
{
  
  public class RemoteConfiguration : IRemoteConfiguration
  {
    public string BaseUrl  { get; set; }
  }
  
}