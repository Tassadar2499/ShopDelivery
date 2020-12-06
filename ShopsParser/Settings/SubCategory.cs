using Newtonsoft.Json;

namespace ShopsParser.Settings
{
	[JsonObject]
	public class SubCategory
	{
		[JsonProperty]
		public string Name { get; set; }

		[JsonProperty]
		public string Url { get; set; }
	}
}