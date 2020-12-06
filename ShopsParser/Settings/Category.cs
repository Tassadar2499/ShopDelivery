using Newtonsoft.Json;

namespace ShopsParser.Settings
{
	[JsonObject]
	public class Category
	{
		[JsonProperty]
		public string Name { get; set; }

		[JsonProperty]
		public string Url { get; set; }

		[JsonProperty]
		public SubCategory[] SubCategories { get; set; }
	}
}