using Microsoft.AspNetCore.Mvc;
using ShopsDbEntities;
using System.Linq;

namespace ShopDeliveryApplication.Controllers
{
	public class ShopsController : Controller
	{
		private readonly ApplicationDbContext _context;

		public ShopsController(ApplicationDbContext context) => _context = context;

		public IActionResult Index()
		{
			var shops = _context.Shops.ToArray();

			return View(shops);
		}
	}
}