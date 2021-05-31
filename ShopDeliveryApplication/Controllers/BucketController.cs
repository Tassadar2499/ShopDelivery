using HarabaSourceGenerators.Common.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopDeliveryApplication.Models;
using ShopDeliveryApplication.Models.Logic;
using ShopsDbEntities.Entities;
using System.Threading.Tasks;

namespace ShopDeliveryApplication.Controllers
{
	[Inject]
	public partial class BucketController : Controller
	{
		public const string BUCKET = "bucket";

		private readonly BucketLogic _bucketLogic;
		private readonly UserManager<User> _userManger;

		private ISession Session => HttpContext.Session;

		public IActionResult Index()
		{
			var products = _bucketLogic.GetBucketProductsBySession(HttpContext.Session);

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
			//TODO: SendUserInfo
			//UserManger.GetUserId(User);
			var isSuccess = Session.TryGetIdArrByKey(BUCKET, out var productsIdArr);
			if (isSuccess)
			{
				//var userId = UserManger.GetUserId(User);
				await _bucketLogic.SendOrderAsync(productsIdArr, "-1");
			}
		}
	}
}