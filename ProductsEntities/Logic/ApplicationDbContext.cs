using Microsoft.EntityFrameworkCore;
using ShopsDbEntities.Entities.ProductEntities;
using ShopsDbEntities.ShopsEntities;
using System.Threading.Tasks;

namespace ShopsDbEntities
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<Shop> Shops { get; set; }
		public DbSet<ShopLocation> ShopsLocations { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<ProductDbCategory> ProductCategories { get; set; }
		public DbSet<ProductDbSubCategory> ProductSubCategories { get; set; }
		public DbSet<Order> Orders { get; set; }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options) => Database.EnsureCreated();

		public async Task CreateAndSaveAsync<T>(T entity)
			=> await Task.Run(() => CreateAndSave(entity));

		public void CreateAndSave<T>(T entity)
		{
			Add(entity);
			SaveChanges();
		}
	}
}