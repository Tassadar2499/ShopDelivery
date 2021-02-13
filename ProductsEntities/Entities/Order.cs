using Newtonsoft.Json;

namespace ShopsDbEntities.Entities.ProductEntities
{
	[JsonObject]
	public class Order
	{
		[JsonProperty]
		public long Id { get; set; }

		[JsonProperty]
		public long ClientId { get; set; }

		[JsonProperty]
		public long AddressId { get; set; }

		[JsonProperty]
		public string BucketProducts { get; set; }
	}
}