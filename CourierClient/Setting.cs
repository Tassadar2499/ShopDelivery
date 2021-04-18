using Newtonsoft.Json;

namespace CourierClient
{
	[JsonObject]
	public class Setting
	{
		[JsonProperty]
		public string CourierServiceHost { get; set; }
	}
}