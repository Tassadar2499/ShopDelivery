using ShopsDbEntities.Entities.ProductEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopsDbEntities.Logic
{
	public class OrdersLogic
	{
		public ApplicationDbContext Context { get; }
		public IQueryable<Order> Orders => Context.Orders;
		public OrdersLogic(ApplicationDbContext context) => Context = context;
	}
}
