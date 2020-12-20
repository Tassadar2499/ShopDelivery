using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopsDbEntities
{
	[JsonObject]
	public class ProductData
	{
		[JsonProperty]
		public Product[] Products { get; set; }
	}
}
