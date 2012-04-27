using System;
using RestSharp;

namespace ArtekSoftware.Codemash.Tests
{
//	public class RestClientOk : IRestClient
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
//				 StatusCode = System.Net.HttpStatusCode.OK,
//			};
//		}
//
//		RestResponse<T> Execute<T> (IRestRequest request)
//		{
//			return new RestResponse<T>()
//			{
//				 StatusCode = System.Net.HttpStatusCode.OK,
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
//
//		#region IRestClient implementation
//		public RestRequestAsyncHandle ExecuteAsync (IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback)
//		{
//			throw new System.NotImplementedException ();
//		}
//
//		public RestRequestAsyncHandle ExecuteAsync<T> (IRestRequest request, Action<IRestResponse<T>, RestRequestAsyncHandle> callback) where T : new ()
//		{
//			throw new System.NotImplementedException ();
//		}
//
//		public IRestResponse Execute (IRestRequest request)
//		{
//			throw new System.NotImplementedException ();
//		}
//
//		public IRestResponse<T> Execute<T> (IRestRequest request) where T : new ()
//		{
//			throw new System.NotImplementedException ();
//		}
//
//		public Uri BuildUri (IRestRequest request)
//		{
//			throw new System.NotImplementedException ();
//		}
//
//		public System.Net.CookieContainer CookieContainer {
//			get {
//				throw new System.NotImplementedException ();
//			}
//			set {
//				throw new System.NotImplementedException ();
//			}
//		}
//
//		string IRestClient.UserAgent {
//			get {
//				throw new System.NotImplementedException ();
//			}
//			set {
//				throw new System.NotImplementedException ();
//			}
//		}
//
//		int IRestClient.Timeout {
//			get {
//				throw new System.NotImplementedException ();
//			}
//			set {
//				throw new System.NotImplementedException ();
//			}
//		}
//
//		public bool UseSynchronizationContext {
//			get {
//				throw new System.NotImplementedException ();
//			}
//			set {
//				throw new System.NotImplementedException ();
//			}
//		}
//
//		IAuthenticator IRestClient.Authenticator {
//			get {
//				throw new System.NotImplementedException ();
//			}
//			set {
//				throw new System.NotImplementedException ();
//			}
//		}
//
//		string IRestClient.BaseUrl {
//			get {
//				throw new System.NotImplementedException ();
//			}
//			set {
//				throw new System.NotImplementedException ();
//			}
//		}
//
//		System.Collections.Generic.IList<Parameter> IRestClient.DefaultParameters {
//			get {
//				throw new System.NotImplementedException ();
//			}
//		}
//
//		System.Security.Cryptography.X509Certificates.X509CertificateCollection IRestClient.ClientCertificates {
//			get {
//				throw new System.NotImplementedException ();
//			}
//			set {
//				throw new System.NotImplementedException ();
//			}
//		}
//
//		System.Net.IWebProxy IRestClient.Proxy {
//			get {
//				throw new System.NotImplementedException ();
//			}
//			set {
//				throw new System.NotImplementedException ();
//			}
//		}
//		#endregion
//	}
}

