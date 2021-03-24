using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourierService.OrdersHandler
{
	[JsonObject]
	public class Setting
	{
		[JsonProperty]
		public ConnectionStrings ConnectionStrings { get; set; }
	}
}
