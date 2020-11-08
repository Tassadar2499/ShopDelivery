using AngleSharp;
using AngleSharp.Dom;
using ShopsParser.Settings;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShopsParser
{
	public class Executor
	{
		private readonly IBrowsingContext _context;
		
		public Executor()
		{
			var config = Configuration.Default.WithDefaultLoader();
			_context = BrowsingContext.New(config);
		}

		public ParallelQuery<Product> GetProductsByShop(Shop shop)
		{
			var urlCollection = shop.Categories.SelectMany(c => c.SubCategories.Select(s => $"{shop.Url}/{c.Url}/{s.Url}"));
			return urlCollection.AsParallel().SelectMany(GetProductsByUrl);
		}

		private ParallelQuery<Product> GetProductsByUrl(string url)
		{
			var document = _context.OpenAsync(url).Result;
			return document.GetElementsByClassName("product").AsParallel().Select(GetProduct);
		}

		private Product GetProduct(IElement element)
		{
			return new Product
			(
				GetProductName(element),
				GetPrice(element),
				GetWeight(element),
				GetImageStrContent(element)
			);
		}

		private string GetProductName(IElement element)
		{
			var productNameElement = element.GetElementsByClassName("product-name").SingleOrDefault();
			var anchorElement = productNameElement.GetElementsByTagName("a").SingleOrDefault();

			return anchorElement.GetAttribute("title");
		}

		private float GetWeight(IElement element)
		{
			var weightElement = element.GetElementsByClassName("product-weight").SingleOrDefault();
			var weight = FilterTextContent(weightElement.TextContent).Replace("кг", "");

			return float.Parse(weight);
		}

		private float GetPrice(IElement element)
		{
			var priceElements = element.GetElementsByClassName("product-price").ToArray();
			var priceElement = element.GetElementsByClassName("product-price").FirstOrDefault();

			var regExp = new Regex(@"\d+,\d+");
			var priceText = regExp.Match(FilterTextContent(priceElement.TextContent)).Value;

			return float.Parse(priceText);
		}

		private string GetImageStrContent(IElement element)
		{
			var imageDivElement = element.GetElementsByClassName("image").SingleOrDefault();
			var imageElement = imageDivElement.GetElementsByTagName("img").SingleOrDefault();
			var src = imageElement.GetAttribute("src");

			using var webClient = new WebClient();
			var imageBytes = webClient.DownloadData($"{element.BaseUri}/{src}");

			return Convert.ToBase64String(imageBytes);
		}

		private string FilterTextContent(string str)
			=> str.Replace("\t", "").Replace("\n", "").Replace(" ", "");
	}
}