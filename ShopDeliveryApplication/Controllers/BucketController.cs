using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopDeliveryApplication.Models;
using ShopDeliveryApplication.Models.Logic;
using System.Threading.Tasks;

namespace ShopDeliveryApplication.Controllers
{
	public class BucketController : Controller
	{
		public const string BUCKET = "bucket";

		private ISession Session => HttpContext.Session;
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
			Session.SetString(BUCKET, content ?? "");

			return new EmptyResult();
		}

		[HttpPost]
		public async Task SendOrderAsync()
		{
			var isSuccess = Session.TryGetString(BUCKET, out var productsIdArrStr);
			if (isSuccess)
				await _logic.SendOrderAsync(productsIdArrStr);
		}
	}
}