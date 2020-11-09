using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsWebApi.Controllers
{
	[ApiController]
	[Route("api/product")]
	public class ProductController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public ProductController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet("list")]
		[EnableQuery]
		public async Task<List<Product>> GetAsync()
		{
			return await _context.Products.ToListAsync();
		}

		[HttpGet]
		public void Write()
		{
			_context.Products.Add(new Product());
			_context.SaveChanges();
		}
	}
}
