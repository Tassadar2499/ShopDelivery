using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace OrdersHandlerService
{
    public static class MainFunction
    {
        [FunctionName("StartFunction")]
        public static void Run([QueueTrigger("product-orders-queue", Connection = "DefaultEndpointsProtocol=https;AccountName=productordersacc;AccountKey=ubmfUgb8XcXYWZNIWHbqJHPnzeZfCWpyjy5Bfc/tO8daCcmpCMlyqxDDrJ5A2wHHkPoYO+Lh+n3i6vaTZuyBsw==;EndpointSuffix=core.windows.net")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
