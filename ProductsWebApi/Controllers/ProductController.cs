using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsWebApi.Models;
using ProductsWebApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsWebApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ProductController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public ProductController(ApplicationDbContext context) => _context = context;

		[HttpPost]
		public async void CreateOrUpdateAsync(ProductData productData)
		{
			await _context.CreateOrUpdateProductsAsync(productData.Products);
		}

		[HttpPost]
		public async void CreateAsync(Product product)
		{
			await _context.AddAsync(product);
			await _context.SaveChangesAsync();
		}

		[HttpGet]
		public async Task<List<Product>> Get() 
			=> await _context.Products.ToListAsync();

		[HttpGet("{id}")]
		public async Task<List<Product>> Get(int id)
			=> await Task.Run(() => _context.Products.Where(p => p.Id == id).ToList());
	}
}
