using HarabaSourceGenerators.Common.Attributes;
using Microsoft.AspNetCore.Http;
using ShopDeliveryApplication.Models.Entities;
using ShopsDbEntities;
using ShopsDbEntities.Logic;
using ShopsDbEntities.OrderData;
using ShopsDbLogic.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShopDeliveryApplication.Models.Logic
{
	[Inject]
	public partial class BucketLogic
	{
		public const string BUCKET = "bucket";

		private readonly ProductsLogic _productsLogic;
		private readonly OrdersLogic _ordersLogic;
		private readonly MessageHandler _messageHandler;
		private readonly OrderProductRepository _orderProductRepo;

		public async Task SendOrderAsync(long[] idArr, string userId)
		{
			//TODO: Заполение информации о заказе

			/*var usersAddress = new UsersAddress()
			{
				UserId = userId,
				AddressId = -1
			};*/

			var order = new Order()
			{
				Created = DateTime.Now
			};

			await _ordersLogic.Context.CreateAndSaveAsync(order);

			var bucketProducts = idArr.Select(i => new OrderProduct() { OrderId = order.Id, ProductId = i }).ToArray();
			await _orderProductRepo.CreateAndSaveAsync(bucketProducts);

			var orderInfo = new OrderInfo()
			{
				OrderProductIds = bucketProducts.Select(b => b.Id).ToArray()
			};

			await _messageHandler.SendActiveOrderMessageAsync(orderInfo);
		}

		public BucketProduct[] GetBucketProductsBySession(ISession session)
		{
			var isSuccess = session.TryGetIdArrByKey(BUCKET, out var productsIdArr);

			return isSuccess
				? GetBucketProductsByIdArr(productsIdArr)
				: Array.Empty<BucketProduct>();
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