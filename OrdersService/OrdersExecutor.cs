using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrdersService
{
	public class OrdersExecutor
	{
		public async Task ErrorHandlerAsync(ProcessErrorEventArgs arg)
		{
			Console.WriteLine("Pepepopo");
		}

		public async Task MessageHandlerAsync(ProcessMessageEventArgs arg)
		{
			var message = arg.Message;
			var body = message.Body.ToString();
			Console.WriteLine($"Received: {body}");

			await arg.CompleteMessageAsync(message);
		}
	}
}
