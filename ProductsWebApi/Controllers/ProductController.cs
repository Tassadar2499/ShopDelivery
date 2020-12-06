using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsEntities;
using ProductsWebApi.Models;
using ProductsWebApi.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsWebApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ProductController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public ProductController(ApplicationDbContext context) => _context = context;

		[HttpPost("createorupdate")]
		public async Task CreateOrUpdateAsync([FromBody] ProductData productData)
			=> await _context.CreateOrUpdateProductsAsync(productData.Products);

		[HttpPost("create")]
		public async Task CreateAsync(Product product)
			=> await _context.CreateAsync(product);

		[HttpGet]
		public async Task<List<Product>> Get()
			=> await _context.Products.ToListAsync();

		[HttpGet("{id}")]
		public async Task<List<Product>> Get(int id)
			=> await Task.Run(() => _context.Products.Where(p => p.Id == id).ToList());
	}
}