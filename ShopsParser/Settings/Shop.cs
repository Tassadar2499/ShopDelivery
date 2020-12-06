using Newtonsoft.Json;

namespace ShopsParser.Settings
{
	[JsonObject]
	public class Shop
	{
		[JsonProperty]
		public string Name { get; set; }

		[JsonProperty]
		public string Url { get; set; }

		[JsonProperty]
		public Category[] Categories { get; set; }
	}
}