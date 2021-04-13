using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace CourierService
{
	public class MainCouriersService : MainCouriers.MainCouriersBase
	{
		private readonly ILogger<MainCouriersService> _logger;
		private readonly ConnectionMultiplexer _redisConnection;
		public MainCouriersService(ILogger<MainCouriersService> logger, IConfiguration configuration)
		{
			_logger = logger;
			var cacheConnection = configuration["CacheConnection"];
			_redisConnection = ConnectionMultiplexer.Connect(cacheConnection);
		}

		public override Task<ActiveCourierReply> FindActiveCourier(ActiveCourierRequest request, ServerCallContext context)
		{
			var coords = (request.Longitude, request.Latitude);
			_logger.LogDebug($"Recieved {coords}");

			var database = _redisConnection.GetDatabase();
			database.StringSet("test", "pepe");

			Console.WriteLine(database.StringGet("test"));


			throw new NotImplementedException();
		}
	}
}