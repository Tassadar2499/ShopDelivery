using AngleSharp;
using ShopsDbEntities;
using ShopsDbEntities.Entities.ProductEntities;
using ShopsParser.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using Shop = ShopsParser.Settings.Shop;

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

		public ParallelQuery<ParsedProduct> GetProductsByShop(Shop shop)
		{
			var urlCollection = shop.Categories.SelectMany(c => c.SubCategories.Select(s => $"{shop.Url}/{c.Url}/{s.Url}"));

			return urlCollection.AsParallel().SelectMany(u => GetProductsByUrl(shop.Name, u));
		}

		private ParallelQuery<ParsedProduct> GetProductsByUrl(string shopName, string url)
		{
			var isSuccess = _shopParsers.TryGetValue(shopName, out var shopParser);
			if (!isSuccess)
				throw new Exception($"Не удалось извлечь парсер по названию магазина - {shopName}");

			var document = _context.OpenAsync(url).Result;

			return shopParser.GetProducts(document);
		}
	}
}