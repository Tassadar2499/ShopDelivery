using ShopsDbEntities.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopsDbEntities.Logic
{
	public class ProductsLogic
	{
		public MainDbContext Context { get; }
		public IQueryable<Product> Products => Context.Products;

		public ProductsLogic(MainDbContext context) => Context = context;

		public async Task CreateOrUpdateProductsAsync(IEnumerable<Product> products)
			=> await Task.Run(() => CreateOrUpdateProducts(products));

		public void CreateOrUpdateProducts(IEnumerable<Product> products)
		{
			var (toUpdate, toCreate) = products.SplitCollection(p => Products.Any(o => o.Id == p.Id));

			Parallel.Invoke
			(
				() => Context.UpdateRange(toUpdate),
				() => Context.AddRange(toCreate)
			);

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
	}
}