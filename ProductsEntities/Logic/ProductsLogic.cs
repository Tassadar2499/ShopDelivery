using ShopsDbEntities.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopsDbEntities.Logic
{
	public class ProductsLogic
	{
		public ApplicationDbContext Context { get; }

		public ProductsLogic(ApplicationDbContext context) => Context = context;

		public async Task CreateOrUpdateProductsAsync(IEnumerable<Product> products)
			=> await Task.Run(() => CreateOrUpdateProducts(products));

		public void CreateOrUpdateProducts(IEnumerable<Product> products)
		{
			var (toUpdate, toCreate) = products.SplitCollection(p => Context.Products.Any(o => o.Id == p.Id));

			Parallel.Invoke
			(
				() => Context.UpdateRange(toUpdate),
				() => Context.AddRange(toCreate)
			);

			Context.SaveChanges();
		}
	}
}