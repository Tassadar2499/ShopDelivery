using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ShopsDbEntities
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<Product> Products { get; set; }

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