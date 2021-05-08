using Newtonsoft.Json;
using System;

namespace ShopsDbEntities
{
	[JsonObject]
	public class Product
	{
		[JsonProperty]
		public long Id { get; set; }

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

		public override bool Equals(object obj)
			=> obj is Product product &&
				   ShopType == product.ShopType &&
				   Category == product.Category &&
				   SubCategory == product.SubCategory &&
				   Name == product.Name;

		public override int GetHashCode()
			=> HashCode.Combine(ShopType, Category, SubCategory, Name);
	}
}