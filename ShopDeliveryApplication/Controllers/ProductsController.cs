using Microsoft.AspNetCore.Mvc;
using ShopDeliveryApplication.Models.Logic;
using ShopsDbEntities.Logic;

namespace ShopDeliveryApplication.Controllers
{
	public class ProductsController : Controller
	{
		private readonly ProductsLogic _logic;

		public ProductsController(ProductsLogic logic) => _logic = logic;

		public IActionResult Index(byte shopId, byte categoryId, byte subCategoryId)
		{
			var products = _logic.GetCatalogProducts(shopId, categoryId, subCategoryId);

			return View(products);
		}
	}
}