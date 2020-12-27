using Microsoft.AspNetCore.Mvc;
using ShopsDbEntities;
using System.Linq;

namespace ShopDeliveryApplication.Controllers
{
	public class ProductsController : CatalogController
	{
		public ProductsController(ApplicationDbContext context) : base(context)
		{
		}

		public IActionResult Index(byte shopId, byte categoryId, byte subCategoryId)
		{
			var products = _context.Products
				.Where(p => (byte)p.ShopType == shopId)
				.Where(p => (byte)p.Category == categoryId)
				.Where(p => (byte)p.SubCategory == subCategoryId)
				.ToArray();

			return View(products);
		}
	}
}