using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShopDeliveryApplication.Models.Logic;
using ShopsDbEntities;
using ShopsDbEntities.Entities;
using ShopsDbEntities.Logic;

namespace ShopDeliveryApplication
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDistributedMemoryCache();
			services.AddSession();

			services.AddScoped<ProductsLogic>();
			services.AddScoped<BucketLogic>();
			services.AddScoped<OrdersLogic>();

			services.AddSingleton<MessageHandler>();

			var connection = Configuration.GetConnectionString("DefaultConnection");
			services.AddDbContext<MainDbContext>(opt => opt.UseSqlServer(connection));

			services.AddIdentity<User, IdentityRole>(SetIdentityOptions)
				.AddEntityFrameworkStores<MainDbContext>()
				.AddDefaultTokenProviders();

			services.AddControllersWithViews();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
				app.UseDeveloperExceptionPage();
			else
				app.UseExceptionHandler("/Shops/Error");

			app.UseSession();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Shops}/{action=Index}/{id?}");
			});
		}

		private static void SetIdentityOptions(IdentityOptions opts)
		{
			var password = opts.Password;

			password.RequiredLength = 0;
			password.RequireNonAlphanumeric = false;
			password.RequireLowercase = false;
			password.RequireUppercase = false;
			password.RequireDigit = false;
		}
	}
}