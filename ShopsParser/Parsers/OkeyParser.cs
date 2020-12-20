using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Media;
using ShopsDbEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShopsParser.Parsers
{
	public class OkeyParser : IShopParser
	{
		private readonly Dictionary<string, ProductCategory> _categories;
		private readonly Dictionary<string, ProductSubCategory> _subategories;

		public OkeyParser()
		{
			_categories = new()
			{
				["miaso-ptitsa-kolbasy"] = ProductCategory.Meat
			};

			_subategories = new()
			{
				["miaso-i-ptitsa"] = ProductSubCategory.Chicken
			};
		}

		public ParallelQuery<Product> GetProducts(IDocument document)
			=> document.GetElementsByClassName("product").AsParallel().Select(GetProduct);

		private Product GetProduct(IElement element)
		{
			var url = element.BaseUri;

			var productCategoryTask = new Task<ProductCategory>(() => GetProductCategory(url));
			productCategoryTask.Start();

			var productSubCategoryTask = new Task<ProductSubCategory>(() => GetProductSubCategory(url));
			productSubCategoryTask.Start();

			var productNameTask = new Task<string>(() => GetProductName(element));
			productNameTask.Start();

			var priceTask = new Task<float>(() => GetPrice(element));
			priceTask.Start();

			var weightTask = new Task<float?>(() => GetWeight(element));
			weightTask.Start();

			var imageUrlTask = new Task<string>(() => GetImageUrl(element));
			imageUrlTask.Start();

			Task.WaitAll(productNameTask, priceTask, weightTask, imageUrlTask);

			return new Product()
			{
				ShopType = ShopType.Okey,
				Category = productCategoryTask.Result,
				SubCategory = productSubCategoryTask.Result,
				Name = productNameTask.Result,
				Price = priceTask.Result,
				Mass = weightTask.Result,
				ImageUrl = imageUrlTask.Result
			};
		}

		private ProductCategory GetProductCategory(string url)
		{
			var categoriesKey = _categories.Keys.FirstOrDefault(k => url.Contains(k));

			return categoriesKey == null
				? ProductCategory.None
				: _categories[categoriesKey];
		}

		private ProductSubCategory GetProductSubCategory(string url)
		{
			var categoriesKey = _subategories.Keys.FirstOrDefault(k => url.Contains(k));

			return categoriesKey == null
				? ProductSubCategory.None
				: _subategories[categoriesKey];
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

		private string GetImageUrl(IElement element)
		{
			var imageDivElement = element.GetElementsByClassName("image").SingleOrDefault();
			var imageElement = imageDivElement.GetElementsByTagName("img").SingleOrDefault() as IHtmlImageElement;
			var src = imageElement.GetAttribute("data-src");

			return $"{element.BaseUrl.Origin}/{src}";
		}

		private string FilterTextContent(string str)
			=> str.Replace("\t", "").Replace("\n", "").Replace(" ", "");
	}
}