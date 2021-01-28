using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopDeliveryApplication.Models.Logic;

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

			return new EmptyResult();
		}
	}
}