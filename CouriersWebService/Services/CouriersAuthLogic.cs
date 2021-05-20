using CouriersWebService.Data;
using HarabaSourceGenerators.Common.Attributes;
using Isopoh.Cryptography.Argon2;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShopsDbEntities;
using ShopsDbEntities.Entities;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace CouriersWebService.Services
{
	[Inject]
	public partial class CouriersAuthLogic
	{
		public const string COURIERS_KEY = "couriers_id";

		private readonly CouriersCacheLogic _couriersCacheLogic;
		private readonly MainDbContext _context;
		private readonly ILogger<CouriersAuthLogic> _logger;

		private IQueryable<Courier> Couriers => _context.Couriers;

		public async Task UpdateAsync([DisallowNull] UpdateCourierData courierData)
		{
			var login = courierData.Login;
			var courier = await _couriersCacheLogic.GetCourierByLoginAsync(login)
				?? await _context.Couriers.FirstOrDefaultAsync(c => c.Login == login);

			if (courier == null)
			{
				_logger.LogError($"Cannot find courier by login = {login}");
				return;
			}

			courier.Status = CourierStatus.Active;
			courier.Longitude = courierData.Longitude;
			courier.Latitude = courier.Latitude;
			courier.SignalRConnectionId = courierData.SignalRConnectionId;

			await _couriersCacheLogic.UpdateAsync(courier);
			_logger.LogInformation("Courier updated");
		}

		public async Task<bool> LoginAsync([DisallowNull] AuthData loginData)
		{
			var login = loginData.Login;
			var courier = await Couriers.FirstOrDefaultAsync(l => l.Login == login);
			if (courier == null)
			{
				_logger.LogError($"Cannot find courier by login = {login}");
				return false;
			}

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
			_logger.LogInformation("Courier registered");
		}

		public async Task RemoveAsync(string login)
		{
			var courier = await _couriersCacheLogic.GetCourierByLoginAsync(login);
			if (courier == null)
			{
				_logger.LogError($"Cannot find courier by login = {login}");
				return;
			}

			await _couriersCacheLogic.RemoveAsync(courier);
			_logger.LogInformation("Courier removed");
		}
	}
}