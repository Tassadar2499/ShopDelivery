using AutoMapper.Configuration;
using ShopsDbEntities.Entities.Comparers;
using ShopsDbEntities.Entities.ProductEntities;
using ShopsDbEntities.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ShopsDbEntities.Logic
{
	public class ProductsLogic
	{
		public MainDbContext Context { get; }
		public IQueryable<Product> Products => Context.Products;

		private readonly ProductImageLogic _imageLogic;
		public ProductsLogic(MainDbContext context, ProductImageLogic imageLogic)
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

			var toUpdateProducts = GetProductsToUpdate(filteredProducts);

			bool isProductToUpdate(ParsedProduct u)
				=> toUpdateProducts.Any(p => ProductDataComparer.IsParsedProductEqualToProduct(u, p));

			var (toUpdateParsedProducts, toCreateParsedProducts) = filteredProducts.SplitCollection(isProductToUpdate);
			UpdateProducts(toUpdateProducts, toUpdateParsedProducts.ToImmutableHashSet());
			Context.UpdateRange(toUpdateProducts);

			var toCreateProducts = toCreateParsedProducts.ConvertParsedProducts();
			Context.AddRange(toCreateProducts.Select(t => t.Product));
			Context.SaveChanges();

			var imagesUpdatedProducts = _imageLogic.LoadProductImagesToBlob(toCreateProducts);
			Context.UpdateRange(imagesUpdatedProducts);
			Context.SaveChanges();
		}

		public IQueryable<Product> GetProductsByIdSet(HashSet<long> idSet)
			=> Products.WhereByExpression(p => idSet.Contains(p.Id));

		public Product[] GetCatalogProducts(byte shopId, byte categoryId, byte subCategoryId)
			=> Products
				.Where(p => (byte)p.ShopType == shopId)
				.Where(p => (byte)p.Category == categoryId)
				.Where(p => (byte)p.SubCategory == subCategoryId)
				.ToArray();

		private ImmutableHashSet<Product> GetProductsToUpdate(ParsedProduct[] products)
		{
			var namesSet = products.Select(p => p.Name).ToImmutableHashSet();
			Expression<Func<Product, bool>> namesExpression = p => namesSet.Contains(p.Name);

			var subCategoriesSet = products.Select(p => p.SubCategory).ToImmutableHashSet();
			Expression<Func<Product, bool>> subCategoriesExpression = p => subCategoriesSet.Contains(p.SubCategory);

			var categoriesSet = products.Select(p => p.Category).ToImmutableHashSet();
			Expression<Func<Product, bool>> categoriesExpression = p => categoriesSet.Contains(p.Category);

			var shopTypesSet = products.Select(p => p.ShopType).ToImmutableHashSet();
			Expression<Func<Product, bool>> shopTypesExpression = p => shopTypesSet.Contains(p.ShopType);

			return Products
				.Where(namesExpression)
				.Where(subCategoriesExpression)
				.Where(categoriesExpression)
				.Where(shopTypesExpression)
				.ToImmutableHashSet();
		}

		private void UpdateProducts(IEnumerable<Product> toUpdateProducts, ImmutableHashSet<ParsedProduct> toUpdateParsedProducts)
			=> Parallel.ForEach(toUpdateProducts, p => UpdateProduct(toUpdateParsedProducts, p));

		private void UpdateProduct(ImmutableHashSet<ParsedProduct> toUpdateParsedProducts, Product productToUpdate)
		{
			var parsedProduct = toUpdateParsedProducts.First(u => ProductDataComparer.IsParsedProductEqualToProduct(u, productToUpdate));
			productToUpdate.Price = parsedProduct.Price;
			productToUpdate.Mass = parsedProduct.Mass;
		}
	}
}