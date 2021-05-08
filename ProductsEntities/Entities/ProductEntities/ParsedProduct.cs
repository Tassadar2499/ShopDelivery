using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopsDbEntities.Entities.ProductEntities
{
	[JsonObject]
	public class ParsedProduct
	{
		[JsonProperty]
		public ShopType ShopType { get; set; }

		[JsonProperty]
		public ProductCategory Category { get; set; }

		[JsonProperty]
		public ProductSubCategory SubCategory { get; set; }

		[JsonProperty]
		public string Name { get; set; }

		[JsonProperty]
		public float Price { get; set; }

		[JsonProperty]
		public float? Mass { get; set; }

		[JsonProperty]
		public string ImageUrl { get; set; }
	}
}
