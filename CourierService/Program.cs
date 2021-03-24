using Azure.Messaging.ServiceBus;
using CourierService.OrdersHandler;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CourierService
{
	public class Program
	{
		private const string QUEUE_NAME = "created_orders";
		public const string SETTING = "appsettings.json";
		public static async Task Main(string[] args)
		{
			var connectionString = GetServiceBusConnectionString();

			await using var client = new ServiceBusClient(connectionString);
			var serviceBusOption = new ServiceBusProcessorOptions();
			var processor = client.CreateProcessor(QUEUE_NAME, serviceBusOption);

			processor.ProcessMessageAsync += OrdersExecutor.MessageHandlerAsync;
			processor.ProcessErrorAsync += OrdersExecutor.ErrorHandlerAsync;

			await processor.StartProcessingAsync();

			CreateHostBuilder(args).Build().Run();
		}

		// Additional configuration is required to successfully run gRPC on macOS.
		// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>	webBuilder.UseStartup<Startup>());


		private static string GetServiceBusConnectionString()
		{
			var text = File.ReadAllText(SETTING);
			var setting = JsonConvert.DeserializeObject<Setting>(text);

			return setting.ConnectionStrings.OrdersServiceBus;
		}
	}
}
