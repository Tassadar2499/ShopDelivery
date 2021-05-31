using Newtonsoft.Json;
using System;

namespace ShopsDbEntities
{
	public class Order
	{
		public long Id { get; set; }
		public long UserAddressId { get; set; }
		public DateTime? Created { get; set; }
	}
}