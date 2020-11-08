using Newtonsoft.Json;
using ShopsParser.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopsParser
{
	[JsonObject]
	public class Setting
	{
		[JsonProperty]
		public Shop[] Shops { get; set; }
	}
}
