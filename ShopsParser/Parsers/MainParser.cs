using AngleSharp;
using ProductsEntities;
using ShopsParser.Parsers;
using ShopsParser.Settings;
using System.Collections.Generic;
using System.Linq;

namespace ShopsParser
{
	public class MainParser
	{
		private readonly IBrowsingContext _context;
		private readonly Dictionary<string, IShopParser> _shopParsers;

		public MainParser()
		{
			var config = Configuration.Default.WithDefaultLoader();
			_context = BrowsingContext.New(config);

			_shopParsers = new()
			{
				["Okey"] = new OkeyParser()
			};
		}

		public ParallelQuery<Product> GetProductsByShop(Shop shop)
		{
			var urlCollection = shop.Categories.SelectMany(c => c.SubCategories.Select(s => $"{shop.Url}/{c.Url}/{s.Url}"));

			return urlCollection.AsParallel().SelectMany(u => GetProductsByUrl(shop.Name, u));
		}

		private ParallelQuery<Product> GetProductsByUrl(string shopName, string url)
		{
			var document = _context.OpenAsync(url).Result;

			return _shopParsers[shopName].GetProducts(document);
		}
	}
}