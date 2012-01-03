using System;
using NUnit.Framework;
using MonoQueue;
using RestSharp;
using Moq;
using Should;

namespace ArtekSoftware.Codemash.Tests
{
	[TestFixture]
	public class NetworkStatusCheckTests
	{
		[Test]
		public void Should_be_online_given_a_valid_url_without_prefix()
		{
			var host = "codemash.org";
			INetworkStatusCheck networkStatusCheck = new NetworkStatusCheck();
			var isReachable = networkStatusCheck.IsReachable(host);
			isReachable.ShouldBeTrue();
		}

		[Test]
		public void Should_be_online_given_a_valid_url_with_prefix()
		{
			var host = "HTTP://codemash.org";
			INetworkStatusCheck networkStatusCheck = new NetworkStatusCheck();
			var isReachable = networkStatusCheck.IsReachable(host);
			isReachable.ShouldBeTrue();
		}
		
		[Test]
		public void Should_be_offline_given_an_invalid_url()
		{
			var host = "http://asdfasdfasdfs.org";
			INetworkStatusCheck networkStatusCheck = new NetworkStatusCheck();
			var isReachable = networkStatusCheck.IsReachable(host);
			isReachable.ShouldBeFalse();
		}
		
		[Test]
		public void Should_be_offline_given_a_garbage_url()
		{
			var host = "sdfdsfdslkjfds";
			INetworkStatusCheck networkStatusCheck = new NetworkStatusCheck();
			var isReachable = networkStatusCheck.IsReachable(host);
			isReachable.ShouldBeFalse();
		}	
		
		[Test]
		public void Should_be_offline_given_an_empty_url()
		{
			var host = "";
			INetworkStatusCheck networkStatusCheck = new NetworkStatusCheck();
			var isReachable = networkStatusCheck.IsReachable(host);
			isReachable.ShouldBeFalse();
		}
		
		[Test]
		public void Should_be_offline_given_a_null_url()
		{
			string host = null;
			INetworkStatusCheck networkStatusCheck = new NetworkStatusCheck();
			var isReachable = networkStatusCheck.IsReachable(host);
			isReachable.ShouldBeFalse();
		}		
	}
}

