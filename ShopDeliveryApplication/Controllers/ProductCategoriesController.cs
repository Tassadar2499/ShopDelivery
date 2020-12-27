using Microsoft.AspNetCore.Mvc;
using ShopsDbEntities;
using System.Linq;

namespace ShopDeliveryApplication.Controllers
{
	public class ProductCategoriesController : CatalogController
	{
		public ProductCategoriesController(ApplicationDbContext context) : base(context)
		{
		}

		public IActionResult Index(byte shopId)
		{
			ViewData["ShopId"] = shopId;
			var categories = _context.ProductCategories.ToArray();

			return View(categories);
		}
	}
}