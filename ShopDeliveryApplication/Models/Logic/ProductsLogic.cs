using ShopsDbEntities;
using ShopsDbEntities.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopDeliveryApplication.Models.Logic
{
	public class ProductsLogic
	{
		public MainDbContext Context { get; }
		public IQueryable<Product> Products => Context.Products;
		public ProductsLogic(MainDbContext context) => Context = context;

		public IQueryable<Product> GetProductsByIdSet(HashSet<long> idSet)
			=> Products.WhereByExpression(p => idSet.Contains(p.Id));

		public Product[] GetCatalogProducts(byte shopId, byte categoryId, byte subCategoryId)
			=> Products
				.Where(p => (byte)p.ShopType == shopId)
				.Where(p => (byte)p.Category == categoryId)
				.Where(p => (byte)p.SubCategory == subCategoryId)
				.ToArray();
	}
}
