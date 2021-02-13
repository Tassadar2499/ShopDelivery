using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopsDbEntities.Entities
{
	[JsonObject]
	public class Address
	{
		[JsonProperty]
		public long Id { get; set; }
		[JsonProperty]
		public long CityId { get; set; }
		[JsonProperty]
		public string Street { get; set; }
		[JsonProperty]
		public string House { get; set; }
		[JsonProperty]
		public string Coords { get; set; }
	}
}
