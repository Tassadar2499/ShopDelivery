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
	public class OrdersWatcher : IHostedService, IDisposable
	{
		private const string QUEUE_NAME = "created_orders";
		private readonly string _connectionString = "";
		private ILogger<OrdersWatcher> Logger { get; }
		private OrdersExecutor OrdersExecutor { get; }

		public OrdersWatcher(ILogger<OrdersWatcher> logger, IConfiguration configuration, OrdersExecutor ordersExecutor)
		{
			Logger = logger;
			OrdersExecutor = ordersExecutor;
			_connectionString = configuration.GetConnectionString("OrdersServiceBus");
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			await using var client = new ServiceBusClient(_connectionString);
			var serviceBusOption = new ServiceBusProcessorOptions();
			var processor = client.CreateProcessor(QUEUE_NAME, serviceBusOption);

			processor.ProcessMessageAsync += OrdersExecutor.MessageHandlerAsync;
			processor.ProcessErrorAsync += OrdersExecutor.ErrorHandlerAsync;

			await processor.StartProcessingAsync();
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
