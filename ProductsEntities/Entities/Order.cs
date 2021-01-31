using System;
using System.Collections.Generic;
using System.Text;

namespace ShopsDbEntities.Entities.ProductEntities
{
	public class Order
	{
		public long Id { get; set; }
		public long ClientId { get; set; }
		public string BucketProducts { get; set; }
		public string Coords { get; set; }
	}
}
