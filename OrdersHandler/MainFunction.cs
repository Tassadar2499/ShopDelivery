using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace OrdersHandler
{
    public static class MainFunction
    {
        [FunctionName("MainFunction")]
        public static void Run([ServiceBusTrigger("created_orders")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
    }
}
