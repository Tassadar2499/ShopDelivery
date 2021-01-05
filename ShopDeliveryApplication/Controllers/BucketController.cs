using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopDeliveryApplication.Models;
using ShopsDbEntities;
using ShopsDbEntities.Logic;
using System.Linq;

namespace ShopDeliveryApplication.Controllers
{
	public class BucketController : Controller
	{
		public const string BUCKET = "bucket";
		private readonly ProductsLogic _logic;

		public BucketController(ProductsLogic context) => _logic = context;

		public IActionResult Index()
		{
			var isSuccess = HttpContext.Session.TryGetIdSetByKey(BUCKET, out var productsIdSet);

			var products = isSuccess
				? _logic.GetProductsByIdSet(productsIdSet).ToArray()
				: new Product[] { };

			return View(products);
		}
	}
}