using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ShopsDbEntities;
using ShopsDbEntities.Entities;
using ShopsDbEntities.Entities.ProductEntities;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CouriersWebService.Services
{
	public class OrdersLogic
	{
		private readonly CouriersCacheLogic _couriersCacheLogic;
		private readonly MainDbContext _context;

		public OrdersLogic(CouriersCacheLogic couriersCacheLogic, MainDbContext context)
		{
			_couriersCacheLogic = couriersCacheLogic;
			_context = context;
		}

		public async Task HandleOrderAsync(string message)
		{
			//{ "Id": 1, "BucketProducts": "[2, 4, 4, 2, 5]", "UserAddressId": 1 }

			var order = JsonConvert.DeserializeObject<Order>(message);

			var usersAddress = await _context.UsersAddresses.FindAsync(order.UserAddressId);
			var address = await _context.Addresses.FindAsync(usersAddress.AddressId);

			var coords = (address.Longitude, address.Latitude);
			var correctCourier = await GetCorrectCourierAsync(coords);
			//TODO: fix it
			if (correctCourier == null)
				return;

			correctCourier.Status = CourierStatus.Work;
			await _couriersCacheLogic.UpdateAsync(correctCourier);

			var orderInfo = "Order To handle";

			var couriersHub = new CouriersHub();

			await couriersHub.SendOrderInfoAsync(correctCourier?.SignalRConnectionId, orderInfo);
		}

		private async Task<Courier> GetCorrectCourierAsync((double Longitude, double Latitude) coords)
		{
			var allCouriers = await _couriersCacheLogic.GetCouriersAsync();
			var activeCouriers = allCouriers?
				.Where(c => c.Status == CourierStatus.Active)
				.Select(c => (Courier: c, Value: GetDeltaCoords(coords, (c.Longitude, c.Latitude))))
				.ToArray();

			if (activeCouriers.Length == 0)
				return null;

			var minValue = activeCouriers.Min(c => c.Value);

			return activeCouriers.FirstOrDefault(c => c.Value == minValue).Courier;
		}

		private static double GetDeltaCoords((double Longitude, double Latitude) first, (double Longitude, double Latitude) second)
			=> Math.Abs(first.Latitude - second.Latitude) + Math.Abs(first.Longitude - second.Longitude);
	}
}