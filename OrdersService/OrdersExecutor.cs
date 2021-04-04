using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ShopsDbEntities;
using ShopsDbEntities.Entities.ProductEntities;
using ShopsDbEntities.Logic;
using System;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OrdersService
{
	public class OrdersExecutor
	{
		private readonly CourierServiceSender _sender;
		private readonly IServiceProvider _scopeFactory;
		public OrdersExecutor(IServiceProvider scopeFactory, IConfiguration configuration)
		{
			_scopeFactory = scopeFactory;
			_sender = new CourierServiceSender(configuration);
		}

		public async Task ErrorHandlerAsync(ProcessErrorEventArgs arg)
		{
			Console.WriteLine("Error in Asure service bus");
		}

		public async Task MessageHandlerAsync(ProcessMessageEventArgs arg)
		{
			var message = arg.Message;
			var body = message.Body.ToString();
			Console.WriteLine($"Received: {body}");

			await HandleOrderAsync(body);
			await arg.CompleteMessageAsync(message);
		}

		private async Task HandleOrderAsync(string message)
		{
			//{ "Id": 1, "BucketProducts": "[2, 4, 4, 2, 5]", "UserAddressId": 1 }

			using var scope = _scopeFactory.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<MainDbContext>();

			var order = JsonConvert.DeserializeObject<Order>(message);

			var usersAddress = await context.UsersAddresses.FindAsync(order.UserAddressId);
			var address = await context.Addresses.FindAsync(usersAddress.AddressId);

			await _sender.FindActiveCourier(address.Longitude, address.Latitude);
		}
	}
}