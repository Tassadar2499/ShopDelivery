using Azure.Messaging.ServiceBus;
using System;
using System.Threading.Tasks;

namespace OrdersService
{
	public static class OrdersExecutor
	{
		public static async Task ErrorHandlerAsync(ProcessErrorEventArgs arg)
		{
			Console.WriteLine("Error in Asure service bus");
		}

		public static async Task MessageHandlerAsync(ProcessMessageEventArgs arg)
		{
			var message = arg.Message;
			var body = message.Body.ToString();
			Console.WriteLine($"Received: {body}");

			await arg.CompleteMessageAsync(message);
		}
	}
}