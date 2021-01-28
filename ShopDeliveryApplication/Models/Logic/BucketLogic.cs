using Microsoft.AspNetCore.Http;
using ShopDeliveryApplication.Models.Entities;
using ShopsDbEntities;
using ShopsDbEntities.Logic;
using System.Linq;

namespace ShopDeliveryApplication.Models.Logic
{
	public class BucketLogic
	{
		public const string BUCKET = "bucket";

		public ApplicationDbContext Context { get; }
		private readonly ProductsLogic _logic;

		public BucketLogic(ProductsLogic logic)
		{
			_logic = logic;
			Context = _logic.Context;
		}

		public BucketProduct[] GetBucketProductsBySession(ISession session)
		{
			var isSuccess = session.TryGetIdArrByKey(BUCKET, out var productsIdArr);

			return isSuccess
				? GetBucketProductsByIdArr(productsIdArr)
				: new BucketProduct[] { };
		}

		private BucketProduct[] GetBucketProductsByIdArr(long[] productsIdArr)
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