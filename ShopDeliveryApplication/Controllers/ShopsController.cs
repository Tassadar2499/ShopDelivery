using Microsoft.AspNetCore.Mvc;
using ShopDeliveryApplication.Models;
using ShopsDbEntities;
using System.Diagnostics;
using System.Linq;

namespace ShopDeliveryApplication.Controllers
{
	public class ShopsController : CatalogController
	{
		public ShopsController(MainDbContext context): base(context)
		{
		}

		public IActionResult Index()
		{
			var shops = _context.Shops.ToArray();

			return View(shops);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
			=> View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}