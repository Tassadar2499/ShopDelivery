using Newtonsoft.Json;
using ShopsParser.Settings;

namespace ShopsParser
{
	[JsonObject]
	public class Setting
	{
		[JsonProperty]
		public string WebApiUrl { get; set; }

		[JsonProperty]
		public Shop[] Shops { get; set; }
	}
}