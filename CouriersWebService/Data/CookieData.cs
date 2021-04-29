using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CouriersWebService.Data
{
	[JsonObject]
	public class CookieData
	{
		[JsonProperty]
		public string Login { get; set; }
	}
}
