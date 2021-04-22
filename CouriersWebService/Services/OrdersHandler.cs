using Azure.Messaging.ServiceBus;
using System;
using System.Threading.Tasks;

namespace CouriersWebService.Services
{
	public class OrdersHandler
	{
		private readonly CouriersManager _manager;

		public OrdersHandler(CouriersManager manager) => _manager = manager;

		//TODO:
		public async Task ErrorHandlerAsync(ProcessErrorEventArgs arg)
		{
			Console.WriteLine("Error in Asure service bus");
		}

		public async Task MessageHandlerAsync(ProcessMessageEventArgs arg)
		{
			var message = arg.Message;
			var body = message.Body.ToString();
			Console.WriteLine($"Received: {body}");

			await _manager.HandleOrderAsync(body);
			await arg.CompleteMessageAsync(message);
		}
	}
}