using Newtonsoft.Json;

namespace ShopsDbEntities
{
	[JsonObject]
	public class ProductData
	{
		[JsonProperty]
		public Product[] Products { get; set; }
	}
}