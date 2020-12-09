using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopDeliveryApplication.Models
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<Shop> Shops { get; set; }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options) => Database.EnsureCreated();
	}
}
