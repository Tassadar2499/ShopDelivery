using AutoMapper.Configuration;
using ShopsDbEntities.Entities.Comparers;
using ShopsDbEntities.Entities.ProductEntities;
using ShopsDbEntities.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace ShopsDbEntities.Logic
{
	public class ProductsLogic
	{
		public MainDbContext Context { get; }
		public IQueryable<Product> Products => Context.Products;

		private readonly Lazy<ProductImageLogic> _imageLogic;
		public ProductsLogic(MainDbContext context, Lazy<ProductImageLogic> imageLogic)
		{
			_imageLogic = imageLogic;
			Context = context;
		}

		public async Task CreateOrUpdateProductsAsync(IEnumerable<ParsedProduct> products)
			=> await Task.Run(() => CreateOrUpdateProducts(products));

		public void CreateOrUpdateProducts(IEnumerable<ParsedProduct> products)
		{
			var filteredProducts = products
				.Where(p => p != null)
				.ToArray();

			var productsSet = filteredProducts
				.Select(p => new ProductEqualityData(p.ShopType, p.Category, p.SubCategory, p.Name))
				.ToImmutableHashSet();

			var toUpdateProducts = GetProductsByEqualityDataSet(productsSet).ToImmutableHashSet();

			bool isProductToUpdate(ParsedProduct u)
				=> toUpdateProducts.Any(p => ProductDataComparer.IsParsedProductEqualToProduct(u, p));

			var (toUpdateParsedProducts, toCreateParsedProducts) = filteredProducts.SplitCollection(isProductToUpdate);

			foreach (var productToUpdate in toUpdateProducts)
			{
				var parsedProduct = toUpdateParsedProducts.Find(u => ProductDataComparer.IsParsedProductEqualToProduct(u, productToUpdate));
				productToUpdate.Price = parsedProduct.Price;
				productToUpdate.Mass = parsedProduct.Mass;
			}

			Context.UpdateRange(toUpdateProducts);

			var toCreateProducts = toCreateParsedProducts.ConvertParsedProducts();
			Context.AddRange(toCreateProducts.Select(t => t.Product));
			Context.SaveChanges();

			_imageLogic.Value.LoadProductImagesToBlob(toCreateProducts);
			Context.SaveChanges();
		}

		public IQueryable<Product> GetProductsByIdSet(HashSet<long> idSet)
			=> Products.WhereByExpression(p => idSet.Contains(p.Id));

		public IQueryable<Product> GetProductsByEqualityDataSet(ImmutableHashSet<ProductEqualityData> productsSet)
			=> Products
				.Where(p => productsSet.Any(e => e.Name == p.Name))
				.Where(p => productsSet.Any(e => e.SubCategory == p.SubCategory))
				.Where(p => productsSet.Any(e => e.Category == p.Category))
				.Where(p => productsSet.Any(e => e.ShopType == p.ShopType));

		public Product[] GetCatalogProducts(byte shopId, byte categoryId, byte subCategoryId)
			=> Products
				.Where(p => (byte)p.ShopType == shopId)
				.Where(p => (byte)p.Category == categoryId)
				.Where(p => (byte)p.SubCategory == subCategoryId)
				.ToArray();
	}
}