using CourierService;
using FakeItEasy;
using Grpc.Core;
using Grpc.Net.Client;
using System;
using Xunit;

namespace CourierService_Tests
{
	public class WebRequests
	{
		[Fact]
		public void FindActiveCourier_Test()
		{
			const string defaultAddress = "https://localhost:5001";
			using var channel = GrpcChannel.ForAddress(defaultAddress);

			var client = new MainCouriers.MainCouriersClient(channel);

			var request = new ActiveCourierRequest
			{
				Longitude = 12.0,
				Latitude = 10.0
			};

			var reply = client.FindActiveCourierAsync(request).ResponseAsync.Result;
		}
	}
}
