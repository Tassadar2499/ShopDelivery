using CouriersWebService.Data;
using ShopsDbEntities;
using ShopsDbEntities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CouriersWebService.Services
{
	public class CouriersAuth
	{
		public const string COURIERS_KEY = "couriers_id";

		private readonly CouriersCacheLogic _couriersCacheLogic;
		private readonly MainDbContext _context;

		private IQueryable<Courier> Couriers => _context.Couriers;

		public CouriersAuth(CouriersCacheLogic couriersCacheLogic, MainDbContext context)
		{
			_couriersCacheLogic = couriersCacheLogic;
			_context = context;
		}

		public async Task Update(string login)
		{
			var random = new Random();
			var courier = Couriers.FirstOrDefault(c => c.Login == login);
			courier.Status = CourierStatus.Active;
			courier.Longitude = random.Next(1000) + random.NextDouble();
			courier.Latitude = random.Next(1000) + random.NextDouble();

			await _couriersCacheLogic.UpdateAsync(courier);
		}

		public async Task LoginAsync()
		{
			throw new NotImplementedException();
		}

		public async Task CheckPasswordAsync()
		{
			throw new NotImplementedException();
		}

		public async Task RegisterAsync(RegisterData registerData)
		{
			var random = new Random();
			var courier = new Courier()
			{
				Status = CourierStatus.Sleep,
				Login = registerData.Login,
				Password = registerData.Password,
				Longitude = random.Next(1000) + random.NextDouble(),
				Latitude = random.Next(1000) + random.NextDouble()
			};

			await _context.CreateAndSaveAsync(courier);
		}
	}
}
