using Microsoft.EntityFrameworkCore;
using ProductsWebApi.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsWebApi.Models
{
	public class ApplicationDbContext : DbContext
	{
        public DbSet<Product> Products { get; set; }
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options) => Database.EnsureCreated();

		public async Task CreateOrUpdateProductsAsync(IEnumerable<Product> products)
		{
			var (toUpdate, toCreate) = products.SplitCollection(p => Products.Any(o => o.Id == p.Id));

			await UpdateProductsAsync(toUpdate);
			await AddProductsAsync(toCreate);
			await SaveChangesAsync();
		}

		public async Task UpdateProductsAsync(IEnumerable<Product> products)
			=> await InvokeProductsAsync(products, (Product p) => Update(p));

		public async Task AddProductsAsync(IEnumerable<Product> products)
			=> await InvokeProductsAsync(products, (Product p) => Add(p));

		private async Task InvokeProductsAsync(IEnumerable<Product> products, Action<Product> action)
			=> await Task.Run(() => Parallel.ForEach(products, action));
	}
}
