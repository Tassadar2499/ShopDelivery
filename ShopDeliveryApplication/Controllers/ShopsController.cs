using Microsoft.AspNetCore.Mvc;
using ShopsDbEntities;
using System.Linq;

namespace ShopDeliveryApplication.Controllers
{
	public class ShopsController : CatalogController
	{
		public ShopsController(ApplicationDbContext context) : base(context)
		{
		}

		public IActionResult Index()
		{
			var shops = _context.Shops.ToArray();

			return View(shops);
		}
	}
}