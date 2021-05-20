using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopsDbEntities.Entities;
using ShopsDbEntities.Entities.Geography;
using ShopsDbEntities.Entities.UsersInfo;
using ShopsDbEntities.ShopsEntities;
using System.Threading.Tasks;

namespace ShopsDbEntities
{
	public class MainDbContext : IdentityDbContext<User>
	{
		public DbSet<Shop> Shops { get; set; }
		public DbSet<ShopLocation> ShopsLocations { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<ProductDbCategory> ProductCategories { get; set; }
		public DbSet<ProductDbSubCategory> ProductSubCategories { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<UsersAddress> UsersAddresses { get; set; }
		public DbSet<Address> Addresses { get; set; }
		public DbSet<City> Cities { get; set; }
		public DbSet<Courier> Couriers { get; set; }

		public MainDbContext(DbContextOptions<MainDbContext> options)
			: base(options)
		{
			Database.EnsureCreated();
			//Database.Migrate();
		}

		public async Task CreateAndSaveAsync<T>(T entity)
			=> await Task.Run(() => CreateAndSave(entity));

		public void CreateAndSave<T>(T entity)
		{
			Add(entity);
			SaveChanges();
		}
	}
}