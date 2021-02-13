using ShopsDbEntities.Entities.ProductEntities;
using System.Linq;

namespace ShopsDbEntities.Logic
{
	public class OrdersLogic
	{
		public MainDbContext Context { get; }
		public IQueryable<Order> Orders => Context.Orders;

		public OrdersLogic(MainDbContext context) => Context = context;
	}
}