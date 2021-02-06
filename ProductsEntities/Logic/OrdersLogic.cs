using ShopsDbEntities.Entities.ProductEntities;
using System.Linq;

namespace ShopsDbEntities.Logic
{
	public class OrdersLogic
	{
		public ApplicationDbContext Context { get; }
		public IQueryable<Order> Orders => Context.Orders;

		public OrdersLogic(ApplicationDbContext context) => Context = context;
	}
}