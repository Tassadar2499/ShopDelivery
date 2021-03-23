using Microsoft.AspNetCore.Mvc;
using ShopsDbEntities.Entities.ProductEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourierService.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class OrderController : ControllerBase
	{
		[HttpPost("process")]
		public void Process([FromBody] Order order)
		{
			
		}
	}
}
