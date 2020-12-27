using Microsoft.AspNetCore.Mvc;
using ShopsDbEntities;
using System.Linq;

namespace ShopDeliveryApplication.Controllers
{
	public class ProductSubCategoriesController : CatalogController
	{
		public ProductSubCategoriesController(ApplicationDbContext context) : base(context)
		{
		}

		public IActionResult Index(byte shopId, byte categoryId)
		{
			ViewData["ShopId"] = shopId;
			ViewData["CategoryId"] = categoryId;
			var subCategories = _context.ProductSubCategories.ToArray();

			return View(subCategories);
		}
	}
}