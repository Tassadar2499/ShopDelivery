using Newtonsoft.Json;

namespace ShopsDbEntities.Entities.Geography
{
	[JsonObject]
	public class City
	{
		[JsonProperty]
		public long Id { get; set; }

		[JsonProperty]
		public string Name { get; set; }
	}
}