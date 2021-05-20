using ShopsDbEntities;
using ShopsDbEntities.Entities.ProductEntities;
using ShopsDbEntities.Logic.Comparers;
using ShopsDbEntities.Utils;
using ShopsDbLogic.Logic;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsWebApi.Models.Logic
{
	public class ProductsLogic : ProductsLogicBase
	{
		private readonly ProductImageLogic _imageLogic;
		private readonly ProductsConverter _productsConverter;

		private ProductsLogic(MainDbContext context) : base(context)
		{
		}

		public ProductsLogic(MainDbContext context, ProductImageLogic imageLogic, ProductsConverter productsConverter)
			: this(context)
		{
			_imageLogic = imageLogic;
			_productsConverter = productsConverter;
		}

		public async Task CreateOrUpdateProductsAsync(IEnumerable<ParsedProduct> products)
			=> await Task.Run(() => CreateOrUpdateProducts(products));

		public void CreateOrUpdateProducts(IEnumerable<ParsedProduct> products)
		{
			var filteredProducts = products
				.Where(p => p != null)
				.ToArray();

			var toUpdateProducts = GetProductsToUpdate(filteredProducts);

			bool isProductToUpdate(ParsedProduct u)
				=> toUpdateProducts.Any(p => ProductDataComparer.IsParsedProductEqualToProduct(u, p));

			var (toUpdateParsedProducts, toCreateParsedProducts) = filteredProducts.SplitCollection(isProductToUpdate);
			UpdateProducts(toUpdateProducts, toUpdateParsedProducts.ToImmutableHashSet());
			Context.UpdateRange(toUpdateProducts);

			var toCreateProductsInfo = _productsConverter.ConvertParsedProducts(toCreateParsedProducts).ToArray();
			Context.AddRange(toCreateProductsInfo.Select(t => t.Product));
			Context.SaveChanges();

			var imagesDict = ProductsInfoToDict(toCreateProductsInfo);
			UploadImagesToProducts(imagesDict);
		}

		private void UploadImagesToProducts(Dictionary<long, string> images)
		{
			var productsSet = GetProductsByIdSet(images.Keys.ToHashSet()).ToHashSet();
			var productsInfoCollection = images.Select(kv => (Product: productsSet.First(p => p.Id == kv.Key), SiteUrl: kv.Value));
			var imagesUpdatedProducts = _imageLogic.LoadProductImagesToBlob(productsInfoCollection);
			Context.UpdateRange(imagesUpdatedProducts);
			Context.SaveChanges();
		}

		private ImmutableHashSet<Product> GetProductsToUpdate(ParsedProduct[] products)
		{
			var namesSet = products.Select(p => p.Name).ToImmutableHashSet();
			var subCategoriesSet = products.Select(p => p.SubCategory).ToImmutableHashSet();
			var categoriesSet = products.Select(p => p.Category).ToImmutableHashSet();
			var shopTypesSet = products.Select(p => p.ShopType).ToImmutableHashSet();

			return Products
				.Where(p => namesSet.Contains(p.Name))
				.Where(p => subCategoriesSet.Contains(p.SubCategory))
				.Where(p => categoriesSet.Contains(p.Category))
				.Where(p => shopTypesSet.Contains(p.ShopType))
				.ToImmutableHashSet();
		}

		private static void UpdateProducts(IEnumerable<Product> toUpdateProducts, ImmutableHashSet<ParsedProduct> toUpdateParsedProducts)
			=> Parallel.ForEach(toUpdateProducts, p => UpdateProduct(toUpdateParsedProducts, p));

		private static void UpdateProduct(ImmutableHashSet<ParsedProduct> toUpdateParsedProducts, Product productToUpdate)
		{
			var parsedProduct = toUpdateParsedProducts.FirstOrDefault(u => ProductDataComparer.IsParsedProductEqualToProduct(u, productToUpdate))
				?? throw new Exception("Cannot find any parsed product");

			productToUpdate.Price = parsedProduct.Price;
			productToUpdate.Mass = parsedProduct.Mass;
		}

		private static Dictionary<long, string> ProductsInfoToDict(IEnumerable<(Product Product, string SiteUrl)> productsInfo)
			=> productsInfo
				.Select(t => (t.Product.Id, t.SiteUrl))
				.GroupBy(t => t.Id)
				.ToDictionary(g => g.Key, g => g.First().SiteUrl);
	}
}