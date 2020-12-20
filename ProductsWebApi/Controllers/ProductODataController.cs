using Microsoft.AspNet.OData;
using ShopsDbEntities;
using ProductsWebApi.Models;
using System.Collections.Generic;
using System.Linq;

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