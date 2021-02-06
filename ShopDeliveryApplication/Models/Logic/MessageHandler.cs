using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;

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