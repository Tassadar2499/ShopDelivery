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
		private readonly ProductsLogic _logic;

		public BucketController(ProductsLogic logic) => _logic = logic;

		public IActionResult Index(string idArr)
		{
			var idSet = idArr
				.Replace("[", "")
				.Replace("]", "")
				.Split(',')
				.Select(int.Parse)
				.ToHashSet();

			//var isSuccess = HttpContext.Session.TryGetIdArrByKey(BUCKET, out var productsIdArr);

			//var bucketProducts = isSuccess
			//	? _logic.GetBucketProductsByIdArr(productsIdArr)
			//	: new BucketProduct[] { };

			//return View(bucketProducts);
			return View();
		}
	}
}