using CouriersWebService.Data;
using Isopoh.Cryptography.Argon2;
using Microsoft.EntityFrameworkCore;
using ShopsDbEntities;
using ShopsDbEntities.Entities;
using System;
using System.Diagnostics.CodeAnalysis;
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

		public async Task UpdateAsync([DisallowNull] UpdateCourierData courierData)
		{
			var login = courierData.Login;
			var courier = await _couriersCacheLogic.GetCourierByLoginAsync(login)
				?? await _context.Couriers.FirstOrDefaultAsync(c => c.Login == login);

			if (courier == null)
				return;

			courier.Status = CourierStatus.Active;
			courier.Longitude = courierData.Longitude;
			courier.Latitude = courier.Latitude;
			courier.SignalRConnectionId = courierData.SignalRConnectionId;

			await _couriersCacheLogic.UpdateAsync(courier);
			Console.WriteLine("Courier updated");
		}

		public async Task<bool> LoginAsync([DisallowNull] AuthData loginData)
		{
			var courier = await Couriers.FirstOrDefaultAsync(l => loginData.Login == l.Login);
			if (courier == null)
				return false;

			return Argon2.Verify(courier.Password, loginData.Password);
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

		public async Task RemoveAsync(string login)
		{
			var courier = await _couriersCacheLogic.GetCourierByLoginAsync(login);
			if (courier == null)
			{
				//TODO: Log
				return;
			}

			await _couriersCacheLogic.RemoveAsync(courier);
		}
	}
}