﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsWebApi.Models.Logic;
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
		private IQueryable<Product> Products => _logic.Products;
		public ProductController(ProductsLogic logic) => _logic = logic;

		[HttpPost("createorupdate")]
		public async Task CreateOrUpdateAsync([FromBody] ProductData productData)
			=> await _logic.CreateOrUpdateProductsAsync(productData.Products);

		[HttpGet]
		public async Task<List<Product>> Get()
			=> await Products.ToListAsync();

		[HttpGet("{id}")]
		public async Task<List<Product>> Get(int id)
			=> await Task.Run(() => Products.Where(p => p.Id == id).ToList());
	}
}