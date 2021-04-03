using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrdersService
{
	public class CourierServiceSender
	{
		private readonly string _courierServiceAddress;

		public CourierServiceSender(IConfiguration configuration)
		{
			_courierServiceAddress = configuration.GetValue<string>("CourierServiceHost");
		}

		public async Task FindActiveCourier(string coords)
		{

		}
	}
}
