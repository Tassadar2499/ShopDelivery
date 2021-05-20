using HarabaSourceGenerators.Common.Attributes;
using Microsoft.AspNet.OData;
using ProductsWebApi.Models.Logic;
using ShopsDbEntities;
using ShopsDbEntities.Logic;
using System.Collections.Generic;
using System.Linq;

namespace ProductsWebApi.Controllers
{
	[Inject]
	public partial class ProductsODataController : ODataController
	{
		private readonly ProductsLogic _logic;
		private IQueryable<Product> Products => _logic.Products;

		[EnableQuery]
		public List<Product> Get()
			=> Products.ToList();

		[EnableQuery]
		public List<Product> Get([FromODataUri] int id)
			=> Products.Where(p => p.Id == id).ToList();
	}
}