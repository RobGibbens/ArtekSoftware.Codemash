using System;
using RestSharp;

namespace ArtekSoftware.Codemash.Tests
{
//	public class RestClientServiceUnavailable : IRestClient
//	{
//		public bool ExecuteWasCalled { get; set;}
//		
//		#region IRestClient implementation
//		public RestRequestAsyncHandle ExecuteAsync (IRestRequest request, Action<RestResponse, RestRequestAsyncHandle> callback)
//		{
//			throw new NotImplementedException ();
//		}
//
//		public RestRequestAsyncHandle ExecuteAsync<T> (IRestRequest request, Action<RestResponse<T>, RestRequestAsyncHandle> callback) where T : new ()
//		{
//			throw new NotImplementedException ();
//		}
//
//		public RestResponse Execute (IRestRequest request)
//		{
//			this.ExecuteWasCalled = true;
//			return new RestResponse()
//			{
//				 StatusCode = System.Net.HttpStatusCode.ServiceUnavailable,
//			};
//		}
//
//		RestResponse<T> Execute<T> (IRestRequest request)
//		{
//			return new RestResponse<T>()
//			{
//				 StatusCode = System.Net.HttpStatusCode.ServiceUnavailable,
//			};
//		}
//
//		public Uri BuildUri (IRestRequest request)
//		{
//			throw new NotImplementedException ();
//		}
//
//		public string UserAgent { get; set; }
//
//		public int Timeout { get; set; }
//
//		public IAuthenticator Authenticator { get; set; }
//
//		public string BaseUrl { get; set; }
//
//
//		public System.Collections.Generic.IList<Parameter> DefaultParameters {
//			get {
//				throw new NotImplementedException ();
//			}
//		}
//
//		public System.Security.Cryptography.X509Certificates.X509CertificateCollection ClientCertificates { get; set; }
//
//		public System.Net.IWebProxy Proxy { get; set; }
//		#endregion
//	}
}

