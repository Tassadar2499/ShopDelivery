using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CourierService
{
	public class MainCouriersService : MainCouriers.MainCouriersBase
	{
		private readonly ILogger<MainCouriersService> _logger;
		public MainCouriersService(ILogger<MainCouriersService> logger) => _logger = logger;

		public override Task<ActiveCourierReply> FindActiveCourier(ActiveCourierRequest request, ServerCallContext context)
		{
			var coords = (request.Longitude, request.Latitude);
			_logger.LogDebug($"Recieved {coords}");

			throw new NotImplementedException();
		}
	}
}