using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopDeliveryApplication.Models;
using ShopsDbEntities;
using System.Linq;

namespace ShopDeliveryApplication.Controllers
{
	public class ProductsController : CatalogController
	{
		public ProductsController(ApplicationDbContext context) : base(context)
		{
		}

		public IActionResult Index(byte shopId, byte categoryId, byte subCategoryId)
		{
			var products = _context.Products
				.Where(p => (byte)p.ShopType == shopId)
				.Where(p => (byte)p.Category == categoryId)
				.Where(p => (byte)p.SubCategory == subCategoryId)
				.ToArray();

			return View(products);
		}

		public void AddToBucket(long productId)
		{
			var session = HttpContext.Session;
			var productIdStr = productId.ToString();

			var isSuccess = session.TryGetString(BucketController.BUCKET, out var bucketStr);
			if (!isSuccess)
			{
				session.SetString(BucketController.BUCKET, productIdStr);
				return;
			}

			bucketStr += $";{productIdStr}";
			session.SetString(BucketController.BUCKET, bucketStr);
		}
	}
}