using CouriersWebService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShopsDbEntities.Entities.ProductEntities;
using System;
using System.Threading.Tasks;

namespace CouriersWebService.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class OrdersController : ControllerBase
	{
		private readonly OrdersLogic _manager;
		private readonly ILogger<OrdersController> _logger;

		public OrdersController(OrdersLogic manager, ILogger<OrdersController> logger)
		{
			_manager = manager;
			_logger = logger;
		}

		[HttpPost("Handle")]
		public async Task HandleOrderAsync([FromBody] Order order)
		{
			//{ "Id": 1, "BucketProducts": "[2, 4, 4, 2, 5]", "UserAddressId": 1 }
			_logger.LogInformation($"Recieved order: {order}");
			await _manager.HandleOrderAsync(order);
		}
	}
}