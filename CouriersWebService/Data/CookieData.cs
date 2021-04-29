using Newtonsoft.Json;

namespace CouriersWebService.Data
{
	[JsonObject]
	public class CookieData
	{
		[JsonProperty]
		public string Login { get; set; }
	}
}