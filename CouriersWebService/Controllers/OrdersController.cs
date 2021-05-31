using CouriersWebService.Services;
using HarabaSourceGenerators.Common.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShopsDbEntities;
using ShopsDbEntities.OrderData;
using System.Threading.Tasks;

namespace CouriersWebService.Controllers
{
	[ApiController]
	[Route("[controller]")]
	[Inject]
	public partial class OrdersController : ControllerBase
	{
		private readonly OrdersLogic _manager;
		private readonly ILogger<OrdersController> _logger;

		[HttpPost("Handle")]
		public async Task HandleOrderAsync([FromBody] OrderInfo order)
		{
			//{ "Id": 1, "BucketProducts": "[2, 4, 4, 2, 5]", "UserAddressId": 1 }
			_logger.LogInformation($"Recieved order: {order}");
			await _manager.HandleOrderAsync(order);
		}
	}
}