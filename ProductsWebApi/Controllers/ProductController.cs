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
	public class ProductsController : ODataController
	{
		private readonly ApplicationDbContext _context;

		public ProductsController(ApplicationDbContext context)
		{
			_context = context;
		}

		[EnableQuery]
		public void Write()
		{
			_context.Products.Add(new Product());
			_context.SaveChanges();
		}

		[EnableQuery]
		public IQueryable<Product> Get()
		{
			return _context.Products;
		}

		[EnableQuery]
		public SingleResult<Product> Get([FromODataUri] int key)
		{
			IQueryable<Product> result = _context.Products.Where(p => p.Id == key);
			return SingleResult.Create(result);
		}
	}
}
