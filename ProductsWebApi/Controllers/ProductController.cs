using HarabaSourceGenerators.Common.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductsWebApi.Models.Logic;
using ShopsDbEntities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsWebApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	[Inject]
	public partial class ProductController : ControllerBase
	{
		private readonly ILogger<ProductController> _logger;
		private readonly ProductsLogic _logic;
		private IQueryable<Product> Products => _logic.Products;

		[HttpPost("createorupdate")]
		public async Task CreateOrUpdateAsync([FromBody] ProductData productData)
		{
			_logger.LogInformation("Recieved product data");
			await _logic.CreateOrUpdateProductsAsync(productData.Products);
		}

		[HttpGet]
		public async Task<List<Product>> Get()
			=> await Products.ToListAsync();

		[HttpGet("{id}")]
		public async Task<List<Product>> Get(int id)
			=> await Task.Run(() => Products.Where(p => p.Id == id).ToList());
	}
}