using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourierService.OrdersHandler
{
	[JsonObject]
	public class ConnectionStrings
	{
		[JsonProperty]
		public string DefaultConnection { get; set; }
		[JsonProperty]
		public string OrdersServiceBus { get; set; }
	}
}
