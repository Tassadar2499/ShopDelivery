using AutoMapper;
using ShopsDbEntities;
using ShopsDbEntities.Entities.ProductEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsWebApi.Models.Logic
{
	public class ProductsConverter
	{
		private readonly Mapper _mapper;
		public ProductsConverter()
		{
			var mapperConfig = new MapperConfiguration(ConfigureMapping);
			_mapper = new Mapper(mapperConfig);
		}

		public IEnumerable<(Product Product, string SiteUrl)> ConvertParsedProducts(IEnumerable<ParsedProduct> parsedProducts)
			=> parsedProducts.Select(ConvertParsedProduct);

		private (Product Product, string SiteUrl) ConvertParsedProduct(ParsedProduct parsedProduct)
		{
			var product = new Product();
			_mapper.Map(parsedProduct, product);

			return (product, parsedProduct.ImageUrl);
		}

		private static void ConfigureMapping(IMapperConfigurationExpression expression)
		{
			expression
				.CreateMap<ParsedProduct, Product>()
				.ForMember(e => e.ImageUrl, opt => opt.Ignore());
		}
	}
}
