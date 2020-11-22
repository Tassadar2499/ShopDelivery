using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsWebApi.Models.Entities
{
	[JsonObject]
	public class ProductData
	{
		[JsonProperty]
		public Product[] Products { get; set; }
	}
}
