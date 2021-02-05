using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace OrdersHandlerService
{
    public static class MainFunction
    {
        [FunctionName("StartFunction")]
        public static void Run([QueueTrigger("product-orders-queue")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
