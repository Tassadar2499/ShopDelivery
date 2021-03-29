using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopDeliveryApplication.Models;
using ShopDeliveryApplication.Models.Logic;
using ShopsDbEntities.Entities;
using System.Threading.Tasks;

namespace ShopDeliveryApplication.Controllers
{
	public class BucketController : Controller
	{
		public const string BUCKET = "bucket";

		private ISession Session => HttpContext.Session;
		private BucketLogic BucketLogic { get; }
		private UserManager<User> UserManger { get; }

		public BucketController(BucketLogic logic)
		{
			BucketLogic = logic;
		}

		public IActionResult Index()
		{
			var products = BucketLogic.GetBucketProductsBySession(HttpContext.Session);

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
			UserManger.GetUserId(User);
			var isSuccess = Session.TryGetString(BUCKET, out var productsIdArrStr);
			if (isSuccess)
			{
				var userId = UserManger.GetUserId(User);
				await BucketLogic.SendOrderAsync(productsIdArrStr, userId);
			}
		}
	}
}