using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace OrdersHandler
{
	public static class MainFunction
	{
		[FunctionName("MainFunction")]
		[FixedDelayRetry(5, "00:00:10")]
		public static async Task RunAsync([ServiceBusTrigger("created_orders")] string queueItem, ILogger log)
		{
			log.LogInformation($"C# ServiceBus queue trigger function processed message: {queueItem}");

			var order = JsonConvert.DeserializeObject<OrderInfo>(queueItem);
			var couriersHost = Environment.GetEnvironmentVariable("CouriersWebService", EnvironmentVariableTarget.Process);
			var url = $"{couriersHost}/Orders/Handle";

			try
			{
				using var client = new HttpClient();
				await client.PostAsJsonAsync(url, order);
			}
			catch (WebException ex)
			{
				log.LogError("Error by sending web request");
				log.LogError(ex.Message, ex);
				throw;
			}
			catch (Exception ex)
			{
				log.LogError(ex.Message, ex);
				throw;
			}
		}
	}

	[JsonObject]
	public class OrderInfo
	{
		[JsonProperty]
		public long OrderId { get; set; }
		[JsonProperty]
		public long[] OrderProductIds { get; set; }
	}
}