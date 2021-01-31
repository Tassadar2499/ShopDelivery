using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

namespace ShopDeliveryApplication.Models.Logic
{
	public class MessageHandler
	{
		private const string QUEUE_NAME = "product-orders-queue";
		public QueueClient QueueClient { get; }
		public MessageHandler(IConfiguration configuration)
		{
			var connection = configuration.GetConnectionString("MessageQueueConnection");
			QueueClient = new QueueClient(connection, QUEUE_NAME);
		}
	}
}
