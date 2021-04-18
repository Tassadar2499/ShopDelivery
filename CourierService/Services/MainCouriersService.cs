using Grpc.Core;
using Microsoft.Extensions.Logging;
using ShopsDbEntities.Entities;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CourierService
{
	public class MainCouriersService : MainCouriers.MainCouriersBase
	{
		private readonly ILogger<MainCouriersService> _logger;
		private readonly IRedisCacheClient _redisCacheClient;
		private IRedisDatabase RedisDatabase => _redisCacheClient.Db0;

		public MainCouriersService(ILogger<MainCouriersService> logger, IRedisCacheClient redisCacheClient)
		{
			_redisCacheClient = redisCacheClient;
			_logger = logger;
		}

		public override async Task<ActiveCourierReply> FindActiveCourier(ActiveCourierRequest request, ServerCallContext context)
		{
			var reply = new ActiveCourierReply();

			var coords = (request.Longitude, request.Latitude);
			_logger.LogDebug($"Recieved {coords}");

			var couriersIdStr = await RedisDatabase.GetAsync<string>(AuthCouriersService.COURIERS_KEY);
			if (string.IsNullOrEmpty(couriersIdStr))
				return reply;

			couriersIdStr = couriersIdStr[..^1];
			var couriersKeys = couriersIdStr
				.Split(';')
				.Select(long.Parse)
				.Select(i => $"Courier{i}")
				.ToHashSet();

			var couriersDict = await RedisDatabase.GetAllAsync<Courier>(couriersKeys);
			var activeCouriers = couriersDict
				.Select(kv => kv.Value)
				.Where(c => c.Status == CourierStatus.Active)
				.Select(c => (Courier: c, Value: GetDeltaCoords(coords, (c.Longitude, c.Latitude))))
				.ToArray();

			var minValue = activeCouriers.Min(c => c.Value);
			var correctCourier = activeCouriers.FirstOrDefault(c => c.Value == minValue).Courier;

			reply.Login = correctCourier.Login;

			return reply;
		}

		private static double GetDeltaCoords((double Longitude, double Latitude) first, (double Longitude, double Latitude) second)
			=> Math.Abs(first.Latitude - second.Latitude) + Math.Abs(first.Longitude - second.Longitude);
	}
}