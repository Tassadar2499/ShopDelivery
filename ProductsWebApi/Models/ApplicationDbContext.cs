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

		public async Task CreateAsync(Product product)
			=> await Task.Run(() => Create(product));

		public async Task CreateOrUpdateProductsAsync(IEnumerable<Product> products)
			=> await Task.Run(() => CreateOrUpdateProducts(products));

		private void CreateOrUpdateProducts(IEnumerable<Product> products)
		{
			var (toUpdate, toCreate) = products.SplitCollection(p => Products.Any(o => o.Id == p.Id));

			Parallel.Invoke(() => UpdateProducts(toUpdate), () => AddProducts(toCreate));
			SaveChanges();
		}

		public async Task UpdateProductsAsync(IEnumerable<Product> products)
			=> await Task.Run(() => UpdateProducts(products));

		public async Task AddProductsAsync(IEnumerable<Product> products)
			=> await Task.Run(() => AddProducts(products));

		private void UpdateProducts(IEnumerable<Product> products)
			=> Parallel.ForEach(products, (Product p) => Update(p));

		private void AddProducts(IEnumerable<Product> products)
			=> Parallel.ForEach(products, (Product p) => Add(p));

		private void Create(Product product)
		{
			Add(product);
			SaveChanges();
		}
	}
}
