using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopsDbEntities.OrderData
{
	[JsonObject]
	public class OrderInfo
	{
		[JsonProperty]
		public long OrderId { get; set; }
		[JsonProperty]
		public long[] OrderProductIds { get; set; }
	}
}
