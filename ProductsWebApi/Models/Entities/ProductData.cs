using Newtonsoft.Json;
using ProductsEntities;

namespace ProductsWebApi.Models.Entities
{
	[JsonObject]
	public class ProductData
	{
		[JsonProperty]
		public Product[] Products { get; set; }
	}
}