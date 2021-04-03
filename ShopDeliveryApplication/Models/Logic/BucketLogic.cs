using Microsoft.AspNetCore.Http;
using ShopDeliveryApplication.Models.Entities;
using ShopsDbEntities.Entities.ProductEntities;
using ShopsDbEntities.Logic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopDeliveryApplication.Models.Logic
{
	public class BucketLogic
	{
		public const string BUCKET = "bucket";

		private readonly ProductsLogic _productsLogic;
		private readonly OrdersLogic _ordersLogic;
		private readonly MessageHandler _messageHandler;

		public BucketLogic(ProductsLogic productLogic, OrdersLogic ordersLogic, MessageHandler messageHandler)
		{
			_productsLogic = productLogic;
			_ordersLogic = ordersLogic;
			_messageHandler = messageHandler;
		}

		public async Task SendOrderAsync(string idArrStr, string userId)
		{
			//TODO: Заполение информации о заказе

			/*var usersAddress = new UsersAddress()
			{
				UserId = userId,
				AddressId = -1
			};*/

			var order = new Order()
			{
				BucketProducts = idArrStr
			};

			await _ordersLogic.Context.CreateAndSaveAsync(order);
			await _messageHandler.SendActiveOrderMessageAsync(order);
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

			return _productsLogic.GetProductsByIdSet(productsIdSet)
					.AsEnumerable()
					.Select(p => (Product: p, Count: productsIdArr.Count(id => p.Id == id)))
					.Select(t => new BucketProduct(t.Product, t.Count))
					.ToArray();
		}
	}
}