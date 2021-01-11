using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopDeliveryApplication.Models;
using ShopDeliveryApplication.Models.Entities;
using ShopDeliveryApplication.Models.Logic;
using ShopsDbEntities;
using System.Linq;

namespace ShopDeliveryApplication.Controllers
{
	public class ProductsController : Controller
	{
		private readonly BucketLogic _logic;
		public ProductsController(BucketLogic logic) => _logic = logic;

		public IActionResult Index(byte shopId, byte categoryId, byte subCategoryId)
		{
			var products = GetProducts(shopId, categoryId, subCategoryId);

			return View(products);
		}

		public void AddToBucket(long productId)
		{
			HttpContext.Session.AddIdToString(BucketController.BUCKET, productId.ToString());
			//HttpContext.Request.Query.TryGetValue("shopId", out var shopId);
			//HttpContext.Request.Query.TryGetValue("sategoryId", out var categoryId);
			//HttpContext.Request.Query.TryGetValue("subCategoryId", out var subCategoryId);
			//
			//var products = GetProducts(byte.Parse(shopId.Single()), byte.Parse(categoryId.Single()), byte.Parse(subCategoryId.Single()));
		}

		public BucketProduct[] GetProducts(byte shopId, byte categoryId, byte subCategoryId)
		{
			var pageProducts = _logic.Context.Products
				.Where(p => (byte)p.ShopType == shopId)
				.Where(p => (byte)p.Category == categoryId)
				.Where(p => (byte)p.SubCategory == subCategoryId)
				.ToArray();

			var isSuccess = HttpContext.Session.TryGetIdArrByKey(BucketLogic.BUCKET, out var idArr);

			return !isSuccess || idArr.Length == 0
				? pageProducts.Select(p => new BucketProduct(p, 0)).ToArray()
				: pageProducts.Select(p => new BucketProduct(p, idArr.Count(i => i == p.Id))).ToArray();
		}
	}
}