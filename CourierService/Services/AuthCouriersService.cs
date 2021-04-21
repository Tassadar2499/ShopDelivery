using Grpc.Core;
using Microsoft.Extensions.Logging;
using ShopsDbEntities;
using ShopsDbEntities.Entities;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace CourierService
{
	public class AuthCouriersService : CouriersAuth.CouriersAuthBase
	{
		public const string COURIERS_KEY = "couriers_id";

		private readonly IRedisCacheClient _redisCacheClient;
		private readonly MainDbContext _context;
		private IQueryable<Courier> Couriers => _context.Couriers;
		private IRedisDatabase RedisDatabase => _redisCacheClient.Db0;

		public AuthCouriersService(IRedisCacheClient redisCacheClient, MainDbContext context)
		{
			_redisCacheClient = redisCacheClient;
			_context = context;
		}

		public override Task<LoginCourierReply> Login(LoginCourierRequest request, ServerCallContext context)
			=> new(() => LoginCourier(request));

		public override Task<Empty> Register(RegisterCourierRequest request, ServerCallContext context)
			=> new(() => RegisterCourier(request, context));

		public override Task<Empty> Update(UpdateCourierRequest request, ServerCallContext context)
			=> new(() => UpdateCourier(request, context));

		private LoginCourierReply LoginCourier(LoginCourierRequest request)
		{
			var reply = new LoginCourierReply()
			{
				IsContains = false
			};

			var courier = Couriers.FirstOrDefault(c => c.Login == request.Login);
			if (courier == null)
				return reply;

			reply.PasswordHash = courier.Password;
			reply.IsContains = true;

			return reply;
		}

		private Empty RegisterCourier(RegisterCourierRequest request, ServerCallContext context)
		{
			var courier = new Courier()
			{
				Status = CourierStatus.Sleep,
				Host = context.Host,
				Login = request.Login,
				Password = request.PasswordHash,
				Longitude = request.Longitude,
				Latitude = request.Latitude
			};

			_context.CreateAndSave(courier);

			return new Empty();
		}

		private Empty UpdateCourier(UpdateCourierRequest request, ServerCallContext context)
		{
			var courier = Couriers.FirstOrDefault(c => c.Login == request.Login);
			courier.Status = CourierStatus.Active;
			courier.Host = context.Host;
			courier.Longitude = request.Longitude;
			courier.Latitude = request.Latitude;

			var courierKey = $"Courier{courier.Id}";
			var isCourierInCache = RedisDatabase.ExistsAsync(courierKey).Result;
			if (!isCourierInCache)
			{
				var couriersId = RedisDatabase.GetAsync<string>(COURIERS_KEY).Result ?? "";
				RedisDatabase.SetAddAsync(COURIERS_KEY, $"{couriersId}{courierKey};").Wait();
			}

			RedisDatabase.SetAddAsync(courierKey, courier).Wait();

			return new Empty();
		}
	}
}