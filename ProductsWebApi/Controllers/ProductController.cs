using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopsDbEntities;
using ShopsDbEntities.Logic;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsWebApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ProductController : ControllerBase
	{
		private readonly ProductsLogic _logic;
		private ApplicationDbContext Context => _logic.Context;
		public ProductController(ProductsLogic context) => _logic = context;

		[HttpPost("createorupdate")]
		public async Task CreateOrUpdateAsync([FromBody] ProductData productData)
			=> await _logic.CreateOrUpdateProductsAsync(productData.Products);

		[HttpPost("create")]
		public async Task CreateAsync(Product product)
			=> await Context.CreateAndSaveAsync(product);

		[HttpGet]
		public async Task<List<Product>> Get()
			=> await Context.Products.ToListAsync();

		[HttpGet("{id}")]
		public async Task<List<Product>> Get(int id)
			=> await Task.Run(() => Context.Products.Where(p => p.Id == id).ToList());
	}
}