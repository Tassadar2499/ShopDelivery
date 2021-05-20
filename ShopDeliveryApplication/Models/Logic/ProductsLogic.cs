using ShopsDbEntities;
using ShopsDbEntities.Utils;
using ShopsDbLogic.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopDeliveryApplication.Models.Logic
{
	public class ProductsLogic : ProductsLogicBase
	{
		public ProductsLogic(MainDbContext context) : base(context)
		{
		}

		public Product[] GetCatalogProducts(byte shopId, byte categoryId, byte subCategoryId)
			=> Products
				.Where(p => (byte)p.ShopType == shopId)
				.Where(p => (byte)p.Category == categoryId)
				.Where(p => (byte)p.SubCategory == subCategoryId)
				.ToArray();
	}
}
