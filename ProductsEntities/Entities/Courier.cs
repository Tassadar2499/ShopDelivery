using System;
using System.Collections.Generic;
using System.Text;

namespace ShopsDbEntities.Entities
{
	public enum CourierStatus : byte
	{ 
		Active = 1,
		Sleep = 2
	}

	public class Courier
	{
		public long Id { get; set; }
		public CourierStatus Status { get; set; }
		public string Host { get; set; }
		public string Login { get; set; }
		public string Password { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
	}
}
