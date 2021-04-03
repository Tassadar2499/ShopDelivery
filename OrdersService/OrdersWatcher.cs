using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OrdersService
{
	public class OrdersWatcher : IHostedService
	{
		private const string QUEUE_NAME = "created_orders";
		private readonly ILogger<OrdersWatcher> _logger;
		private readonly IConfiguration _configuration;

		public OrdersWatcher(ILogger<OrdersWatcher> logger, IConfiguration configuration)
		{
			_logger = logger;
			_configuration = configuration;		
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			//TODO: Совместить консоль и логгер

			_logger.LogDebug("Service started");
			Console.WriteLine("Service started");

			var connectionString = _configuration.GetConnectionString("OrdersServiceBus");
			var client = new ServiceBusClient(connectionString);
			var serviceBusOption = new ServiceBusProcessorOptions();
			var processor = client.CreateProcessor(QUEUE_NAME, serviceBusOption);

			processor.ProcessMessageAsync += OrdersExecutor.MessageHandlerAsync;
			processor.ProcessErrorAsync += OrdersExecutor.ErrorHandlerAsync;

			await processor.StartProcessingAsync();
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogDebug("Service stopped");
			Console.WriteLine("Service stopped");

			return Task.CompletedTask;
		}
	}
}
