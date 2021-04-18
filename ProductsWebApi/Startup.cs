using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using Microsoft.OpenApi.Models;
using OData.Swagger.Services;
using ShopsDbEntities;
using ShopsDbEntities.Logic;
using System;

namespace ProductsWebApi
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			var connection = Configuration.GetConnectionString("DefaultConnection");
			services.AddDbContext<MainDbContext>(options => options.UseSqlServer(connection));
			services.AddScoped<ProductsLogic>();

			services.AddOData();

			services.AddControllers();

			services.AddSwaggerGen((config) =>
			{
				config.SwaggerDoc("v1", new OpenApiInfo()
				{
					Title = "Swagger Odata Demo Api",
					Description = "Swagger Odata Demo",
					Version = "v1"
				});
			});

			services.AddOdataSwaggerSupport();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductsWebApi v1"));
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapODataRoute("odata", "odata", GetEdmModel(app.ApplicationServices));
				endpoints.MapControllers();
			});
		}

		private static IEdmModel GetEdmModel(IServiceProvider serviceProvider)
		{
			var builder = new ODataConventionModelBuilder(serviceProvider);
			builder.EntitySet<Product>("Products");

			return builder.GetEdmModel();
		}
	}
}