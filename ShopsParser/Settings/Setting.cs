using Newtonsoft.Json;
using ShopsParser.Settings;

namespace ShopsParser
{
	[JsonObject]
	public class Setting
	{
		[JsonProperty]
		public Shop[] Shops { get; set; }
	}
}