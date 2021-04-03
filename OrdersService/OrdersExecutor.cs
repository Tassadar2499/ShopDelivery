using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
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
		private readonly MainDbContext _mainContext;
		private readonly CourierServiceSender _sender;
		public OrdersExecutor(MainDbContext mainContext, CourierServiceSender sender)
		{
			_mainContext = mainContext;
			_sender = sender;
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
			var order = JsonConvert.DeserializeObject<Order>(message);

			var usersAddress = await _mainContext.UsersAddresses.FindAsync(order.UserAddressId);
			var address = await _mainContext.Addresses.FindAsync(usersAddress.AddressId);

			await _sender.FindActiveCourier(address.Coords);
		}
	}
}