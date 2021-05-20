using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ShopsDbEntities;
using System.Threading.Tasks;

namespace ShopDeliveryApplication.Models.Logic
{
	public class MessageHandler
	{
		private const string QUEUE_NAME = "created_orders";

		private readonly string _connectionString;

		public MessageHandler(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("OrdersServiceBus");
		}

		public async Task SendActiveOrderMessageAsync(Order order)
		{
			await using var client = new ServiceBusClient(_connectionString);
			var sender = client.CreateSender(QUEUE_NAME);
			var text = JsonConvert.SerializeObject(order);
			var message = new ServiceBusMessage(text);
			await sender.SendMessageAsync(message);
		}
	}
}