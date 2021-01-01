using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopDeliveryApplication.Models;
using ShopsDbEntities;
using ShopsDbEntities.Utils;
using System.Linq;

namespace ShopDeliveryApplication.Controllers
{
	public class BucketController : CatalogController
	{
		public const string BUCKET = "bucket";

		public BucketController(ApplicationDbContext context) : base(context)
		{
		}

		public IActionResult Index()
		{
			var isSuccess = HttpContext.Session.TryGetString(BUCKET, out var bucketStrId);
			if (!isSuccess || string.IsNullOrEmpty(bucketStrId))
				return View(new Product[] { });

			var productsIdSet = bucketStrId.Split(';').Select(long.Parse).ToHashSet();
			var products = _context.Products
				.WhereByExpression(p => productsIdSet.Contains(p.Id))
				.ToArray();

			return View(products);
		}
	}
}