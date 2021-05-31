using Microsoft.EntityFrameworkCore;
using ShopsDbEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopsDbLogic.Repositories
{
	public class OrderProductRepository
	{
		private readonly MainDbContext _mainDbContext;
		private DbSet<OrderProduct> OrderProducts => _mainDbContext.OrderProducts;
		public OrderProductRepository(MainDbContext mainDbContext) => _mainDbContext = mainDbContext;

		public async Task CreateAndSaveAsync(IEnumerable<OrderProduct> orderProducts)
		{
			await OrderProducts.AddRangeAsync(orderProducts);
			await SaveAsync();
		}

		public async Task SaveAsync()
			=> await _mainDbContext.SaveChangesAsync();

		public IQueryable<OrderProduct> GetAll() => OrderProducts;
	}
}
