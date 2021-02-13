using Microsoft.AspNetCore.Mvc;
using ShopsDbEntities;

namespace ShopDeliveryApplication.Controllers
{
	public abstract class CatalogController : Controller
	{
		protected readonly MainDbContext _context;

		protected CatalogController(MainDbContext context) => _context = context;
	}
}