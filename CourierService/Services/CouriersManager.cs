using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ShopsDbEntities;
using ShopsDbEntities.Entities;
using ShopsDbEntities.Entities.ProductEntities;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CourierService.Services
{
	public class CouriersManager
	{
		private readonly IServiceProvider _scopeFactory;
		private readonly CouriersCacheLogic _couriersCacheLogic;
		private IRedisDatabase RedisDatabase => _couriersCacheLogic.RedisDatabase;

		public CouriersManager(IServiceProvider scopeFactory, CouriersCacheLogic couriersCacheLogic)
		{
			_scopeFactory = scopeFactory;
			_couriersCacheLogic = couriersCacheLogic;
		}

		public async Task HandleOrderAsync(string message)
		{
			//{ "Id": 1, "BucketProducts": "[2, 4, 4, 2, 5]", "UserAddressId": 1 }

			var order = JsonConvert.DeserializeObject<Order>(message);

			using var scope = _scopeFactory.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<MainDbContext>();

			var usersAddress = await context.UsersAddresses.FindAsync(order.UserAddressId);
			var address = await context.Addresses.FindAsync(usersAddress.AddressId);

			var coords = (address.Longitude, address.Latitude);
			var correctCourier = await GetCorrectCourierAsync(coords);
			//TODO: fix it
			if (correctCourier == null)
				return;

			correctCourier.Status = CourierStatus.Work;
			await _couriersCacheLogic.UpdateAsync(correctCourier);

			var login = correctCourier?.Login;
			Console.WriteLine(login);
		}

		private async Task<Courier> GetCorrectCourierAsync((double Longitude, double Latitude) coords)
		{
			var couriersKeys = await _couriersCacheLogic.GetCouriersKeysAsync();
			if (couriersKeys.Count == 0)
				return null;

			var allCouriers = await _couriersCacheLogic.GetCouriersByKeysAsync(couriersKeys);
			var activeCouriers = allCouriers?
				.Where(c => c.Status == CourierStatus.Active)
				.Select(c => (Courier: c, Value: GetDeltaCoords(coords, (c.Longitude, c.Latitude))))
				.ToArray();

			var minValue = activeCouriers.Min(c => c.Value);

			return activeCouriers.FirstOrDefault(c => c.Value == minValue).Courier;
		}

		private static double GetDeltaCoords((double Longitude, double Latitude) first, (double Longitude, double Latitude) second)
			=> Math.Abs(first.Latitude - second.Latitude) + Math.Abs(first.Longitude - second.Longitude);
	}
}