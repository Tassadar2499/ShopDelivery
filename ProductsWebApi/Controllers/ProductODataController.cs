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
	public class ProductsODataController : ODataController
	{
		private readonly ApplicationDbContext _context;

		public ProductsODataController(ApplicationDbContext context)
		{
			_context = context;
		}

		[EnableQuery]
		public List<Product> Get()
			=> _context.Products.ToList();

		[EnableQuery]
		public List<Product> Get([FromODataUri] int id) 
			=> _context.Products.Where(p => p.Id == id).ToList();
	}
}
