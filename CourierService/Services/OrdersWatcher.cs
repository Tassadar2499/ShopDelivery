using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CourierService.Services
{
	public class OrdersWatcher : IHostedService
	{
		private const string QUEUE_NAME = "created_orders";
		private readonly ILogger<OrdersWatcher> _logger;
		private readonly IConfiguration _configuration;
		private readonly OrdersExecutor _ordersExecutor;

		public OrdersWatcher(ILogger<OrdersWatcher> logger, IConfiguration configuration, OrdersExecutor ordersExecutor)
		{
			_logger = logger;
			_configuration = configuration;
			_ordersExecutor = ordersExecutor;
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

			processor.ProcessMessageAsync += _ordersExecutor.MessageHandlerAsync;
			processor.ProcessErrorAsync += _ordersExecutor.ErrorHandlerAsync;

			await processor.StartProcessingAsync(cancellationToken);
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogDebug("Service stopped");
			Console.WriteLine("Service stopped");

			return Task.CompletedTask;
		}
	}
}
