using Grpc.Core;
using Microsoft.Extensions.Logging;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Threading.Tasks;

namespace CourierService
{
	public class MainCouriersService : MainCouriers.MainCouriersBase
	{
		private readonly ILogger<MainCouriersService> _logger;
		private readonly IRedisCacheClient _redisCacheClient;

		public MainCouriersService(ILogger<MainCouriersService> logger, IRedisCacheClient redisCacheClient)
		{
			_redisCacheClient = redisCacheClient;
			_logger = logger;
		}

		public override Task<ActiveCourierReply> FindActiveCourier(ActiveCourierRequest request, ServerCallContext context)
		{
			var coords = (request.Longitude, request.Latitude);
			_logger.LogDebug($"Recieved {coords}");

			var task = _redisCacheClient.Db0.AddAsync("test", "pepe");
			task.Wait();

			var res = GetValueAsync().Result;

			Console.WriteLine(res);

			throw new NotImplementedException();
		}

		private async Task<string> GetValueAsync()
		{
			return await _redisCacheClient.Db0.GetAsync<string>("test");
		}
	}
}