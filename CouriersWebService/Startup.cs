using CouriersWebService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ShopsDbEntities;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;
using System.Linq;
using System.Net.Http;

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
			services.AddSignalR();

			var redisConfiguration = Configuration.GetSection("Redis").Get<RedisConfiguration>();
			services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(redisConfiguration);

			var connection = Configuration.GetConnectionString("DefaultConnection");
			services.AddDbContext<MainDbContext>(options => options.UseSqlServer(connection));

			services.AddScoped<HttpClient>();
			services.AddScoped<OrdersLogic>();
			services.AddScoped<CouriersAuthLogic>();
			services.AddScoped<CouriersNotifyService>();
			services.AddSingleton<CouriersCacheLogic>();

			services.AddResponseCompression
			(
				opts => opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" })
			);

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

			app.UseResponseCompression();
			app.UseStaticFiles();

			app.UseRouting();

			//app.UseCookiePolicy(new CookiePolicyOptions
			//{
			//	MinimumSameSitePolicy = SameSiteMode.Strict,
			//	HttpOnly = HttpOnlyPolicy.Always,
			//	Secure = CookieSecurePolicy.Always,
			//});

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapBlazorHub();
				endpoints.MapControllers();
				endpoints.MapHub<CouriersHub>("/couriershub");
				endpoints.MapFallbackToPage("/_Host");
			});
		}
	}
}