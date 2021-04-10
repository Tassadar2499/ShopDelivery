using CourierService;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrdersService
{
	public class CourierServiceSender
	{
		private readonly string _courierServiceAddress;

		public CourierServiceSender(IConfiguration configuration)
		{
			_courierServiceAddress = configuration.GetValue<string>("CourierServiceHost");
		}

		public async Task FindActiveCourier(double longitude, double latitude)
		{
			using var channel = GrpcChannel.ForAddress(_courierServiceAddress);
			var client = new MainCouriers.MainCouriersClient(channel);

			var request = new ActiveCourierRequest
			{
				Longitude = longitude,
				Latitude = latitude
			};

			var reply = await client.FindActiveCourierAsync(request);
			Console.WriteLine("Ответ сервера: " + reply.Message);
		}
	}
}
