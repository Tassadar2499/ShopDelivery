using Newtonsoft.Json;

namespace ShopsDbEntities.Entities.ProductEntities
{
	[JsonObject]
	public class Order
	{
		[JsonProperty]
		public long Id { get; set; }

		[JsonProperty]
		public long UserAddressId { get; set; }

		[JsonProperty]
		public string BucketProducts { get; set; }

		public override string ToString()
			=> $"Id = {Id}; UserAddressId = {UserAddressId}; Bucket = {BucketProducts}";
	}
}