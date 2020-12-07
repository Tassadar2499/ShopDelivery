using Newtonsoft.Json;

namespace ProductsEntities
{
	public enum ShopType : byte
	{
		None = 0,
		Okey = 1
	}

	public enum ProductCategory : byte
	{
		None = 0,
		Meat = 1
	}

	public enum ProductSubCategory : byte
	{
		None = 0,
		Chicken = 1
	}

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
	}
}