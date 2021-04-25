using CouriersWebService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ShopsDbEntities;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;

namespace CouriersWebService
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration) => Configuration = configuration;

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddRazorPages();
			services.AddServerSideBlazor();
			services.AddControllers();

			var redisConfiguration = Configuration.GetSection("Redis").Get<RedisConfiguration>();
			services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(redisConfiguration);

			var connection = Configuration.GetConnectionString("DefaultConnection");
			services.AddDbContext<MainDbContext>(options => options.UseSqlServer(connection));

			services.AddSingleton<CouriersManager>();
			services.AddSingleton<CouriersCacheLogic>();

			services.AddSwaggerGen((config) =>
			{
				config.SwaggerDoc("v1", new OpenApiInfo()
				{
					Title = "Swagger Demo Api",
					Description = "Swagger Demo",
					Version = "v1"
				});
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CouriersWebService v1"));
			}
			else
			{
				app.UseExceptionHandler("/Error");
			}

			app.UseStaticFiles();

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapBlazorHub();
				endpoints.MapControllers();
				endpoints.MapFallbackToPage("/_Host");
			});
		}
	}
}