using System;
using NUnit.Framework;
//using MonoQueue;
using RestSharp;
using Moq;
using Should;


namespace ArtekSoftware.Codemash.Tests.AddSessionToRemoteScheduleCommandTests
{
	[TestFixture]
	public class ExecuteMethod
	{
		[Test]
		public void stuff()
		{
			var mock = new Mock<IRestClient>();
			var x = mock.Object;
		}
	}
}




//namespace ArtekSoftware.Codemash.Tests.AddSessionToRemoteScheduleCommandTests
//{
//	[TestFixture]
//	public class ExecuteMethod
//	{
//		[Test]
//		public void Should_call_remote_service_given_network_is_online ()
//		{
//			INetworkStatusCheck networkStatusCheck = new NetworkStatusCheckOnline();
//			ITestFlightProxy testFlight = new TestFlightProxyConsole();
//			IRestClient restClient = new RestClientOk();
//			
//			var command = new AddSessionToRemoteScheduleCommand(networkStatusCheck, 
//			                                                    testFlight, 
//			                                                    restClient);
//			command.Execute();
//			
//			((RestClientOk)restClient).ExecuteWasCalled.ShouldBeTrue();
//		}
//	
//		[Test]
//		public void Should_not_call_remote_service_given_network_is_offline ()
//		{
//			INetworkStatusCheck networkStatusCheck = new NetworkStatusCheckOffline();
//			ITestFlightProxy testFlight = new TestFlightProxyConsole();
//			IRestClient restClient = new RestClientOk();
//			
//			var command = new AddSessionToRemoteScheduleCommand(networkStatusCheck, 
//			                                                    testFlight, 
//			                                                    restClient);
//			command.Execute();
//			
//			((RestClientOk)restClient).ExecuteWasCalled.ShouldBeFalse();
//		}	
//	
//		[Test]
//		public void Should_throw_exception_given_bad_status_code_returned()
//		{
//			INetworkStatusCheck networkStatusCheck = new NetworkStatusCheckOnline();
//			ITestFlightProxy testFlight = new TestFlightProxyConsole();
//			IRestClient restClient = new RestClientServiceUnavailable();
//			
//			var command = new AddSessionToRemoteScheduleCommand(networkStatusCheck, 
//			                                                    testFlight, 
//			                                                    restClient);
//			
//		    /* Test */
//		    TestDelegate throwingCode = () => command.Execute();
//		 
//		    /* Assert */
//		    Assert.Throws<ApplicationException>(throwingCode);			
//			
//			((RestClientServiceUnavailable)restClient).ExecuteWasCalled.ShouldBeTrue();
//		}
//	}
//}