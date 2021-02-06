using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace OrderQueueHandler
{
    public static class StartFunction
    {
        [FunctionName("StartFunction")]
        public static void Run([QueueTrigger("product-orders-queue")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
