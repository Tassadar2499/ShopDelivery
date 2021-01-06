using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopDeliveryApplication.Models;
using ShopDeliveryApplication.Models.Entities;
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
			var isSuccess = HttpContext.Session.TryGetIdArrByKey(BUCKET, out var productsIdArr);

			var bucketProducts = isSuccess
				? _logic.GetBucketProductsByIdArr(productsIdArr)
				: new BucketProduct[] { };

			return View(bucketProducts);
		}
	}
}