using System.Linq;

namespace ShopsDbEntities.Logic
{
	public partial class OrdersLogic
	{
		public MainDbContext Context { get; }
		public IQueryable<Order> Orders => Context.Orders;

		public OrdersLogic(MainDbContext context) => Context = context;
	}
}