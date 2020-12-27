using Microsoft.AspNetCore.Mvc;
using ShopsDbEntities;

namespace ShopDeliveryApplication.Controllers
{
	public abstract class CatalogController : Controller
	{
		protected readonly ApplicationDbContext _context;

		protected CatalogController(ApplicationDbContext context) => _context = context;
	}
}