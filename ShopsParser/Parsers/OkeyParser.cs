using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShopsParser.Parsers
{
	public class OkeyParser : IShopParser
	{
		public ParallelQuery<Product> GetProducts(IDocument document) 
			=> document.GetElementsByClassName("product").AsParallel().Select(GetProduct);

		private Product GetProduct(IElement element)
		{
			var productNameTask = new Task<string>(() => GetProductName(element));
			productNameTask.Start();

			var priceTask = new Task<float>(() => GetPrice(element));
			priceTask.Start();

			var weightTask = new Task<float?>(() => GetWeight(element));
			weightTask.Start();

			var imageStrContentTask = new Task<string>(() => GetImageStrContent(element));
			imageStrContentTask.Start();

			Task.WaitAll(productNameTask, priceTask, weightTask, imageStrContentTask);

			return new Product
			(
				productNameTask.Result,
				priceTask.Result,
				weightTask.Result,
				imageStrContentTask.Result
			);
		}

		private string GetProductName(IElement element)
		{
			var productNameElement = element.GetElementsByClassName("product-name").SingleOrDefault();
			var anchorElement = productNameElement.GetElementsByTagName("a").SingleOrDefault();

			return anchorElement.GetAttribute("title");
		}

		private float? GetWeight(IElement element)
		{
			var weightElement = element.GetElementsByClassName("product-weight").SingleOrDefault();
			var weight = FilterTextContent(weightElement.TextContent).Replace("кг", "");

			return !string.IsNullOrEmpty(weight)
				? float.Parse(weight)
				: null;
		}

		private float GetPrice(IElement element)
		{
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
