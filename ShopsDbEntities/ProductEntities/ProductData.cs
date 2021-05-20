using Newtonsoft.Json;
using ShopsDbEntities.Entities.ProductEntities;

namespace ShopsDbEntities
{
	[JsonObject]
	public class ProductData
	{
		[JsonProperty]
		public ParsedProduct[] Products { get; set; }
	}
}