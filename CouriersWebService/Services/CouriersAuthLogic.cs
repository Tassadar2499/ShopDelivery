using CouriersWebService.Data;
using Isopoh.Cryptography.Argon2;
using ShopsDbEntities;
using ShopsDbEntities.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CouriersWebService.Services
{
	public class CouriersAuthLogic
	{
		public const string COURIERS_KEY = "couriers_id";

		private readonly CouriersCacheLogic _couriersCacheLogic;
		private readonly MainDbContext _context;

		private IQueryable<Courier> Couriers => _context.Couriers;

		public CouriersAuthLogic(CouriersCacheLogic couriersCacheLogic, MainDbContext context)
		{
			_couriersCacheLogic = couriersCacheLogic;
			_context = context;
		}

		public async Task UpdateAsync(UpdateCourierData courierData, string login)
		{
			var courier = await _couriersCacheLogic.GetCourierByLogin(login);

			courier.Status = CourierStatus.Active;
			courier.Longitude = courierData.Longitude;
			courier.Latitude = courier.Latitude;

			await _couriersCacheLogic.UpdateAsync(courier);
		}

		public bool Login(AuthData loginData)
		{
			var courier = Couriers.FirstOrDefault(l => loginData.Login == l.Login);

			return courier != null && courier.Password == Argon2.Hash(loginData.Password);
		}

		public async Task RegisterAsync(AuthData registerData)
		{
			var random = new Random();
			var courier = new Courier()
			{
				Status = CourierStatus.Sleep,
				Login = registerData.Login,
				Password = Argon2.Hash(registerData.Password),
				Longitude = random.Next(1000) + random.NextDouble(),
				Latitude = random.Next(1000) + random.NextDouble()
			};

			await _context.CreateAndSaveAsync(courier);
		}
	}
}