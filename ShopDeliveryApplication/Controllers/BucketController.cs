using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopDeliveryApplication.Models;
using ShopDeliveryApplication.Models.Entities;
using ShopDeliveryApplication.Models.Logic;
using ShopsDbEntities.Logic;
using System.Linq;

namespace ShopDeliveryApplication.Controllers
{
	public class BucketController : Controller
	{
		public const string BUCKET = "bucket";
		private readonly BucketLogic _logic;

		public BucketController(BucketLogic logic) => _logic = logic;

		public IActionResult Index()
		{
			var products = _logic.GetBucketProductsBySession(HttpContext.Session);

			return View(products);
		}

		[HttpPost]
		public IActionResult SaveBucketToSession(string content)
		{
			HttpContext.Session.SetString(BUCKET, content ?? "");

			//return new RedirectToActionResult("Index", "Bucket", null);
			return new EmptyResult();
		}
	}
}