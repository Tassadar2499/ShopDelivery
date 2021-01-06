using ShopDeliveryApplication.Models.Entities;
using ShopsDbEntities.Logic;
using System.Linq;

namespace ShopDeliveryApplication.Models.Logic
{
	public class BucketLogic
	{
		private readonly ProductsLogic _logic;

		public BucketLogic(ProductsLogic logic) => _logic = logic;

		public BucketProduct[] GetBucketProductsByIdArr(long[] productsIdArr)
		{
			var productsIdSet = productsIdArr.ToHashSet();

			return _logic.GetProductsByIdSet(productsIdSet)
					.AsEnumerable()
					.Select(p => (Product: p, Count: productsIdArr.Count(id => p.Id == id)))
					.Select(t => new BucketProduct(t.Product, t.Count))
					.ToArray();
		}
	}
}