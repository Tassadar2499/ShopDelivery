using HarabaSourceGenerators.Common.Attributes;
using Microsoft.AspNetCore.Mvc;
using ShopDeliveryApplication.Models.Logic;

namespace ShopDeliveryApplication.Controllers
{
	[Inject]
	public partial class ProductsController : Controller
	{
		private readonly ProductsLogic _logic;

		public IActionResult Index(byte shopId, byte categoryId, byte subCategoryId)
		{
			var products = _logic.GetCatalogProducts(shopId, categoryId, subCategoryId);

			return View(products);
		}
	}
}