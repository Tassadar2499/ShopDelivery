using System;
using System.Collections.Generic;
using System.Text;

namespace ShopsDbEntities.Entities.ProductEntities
{
	public readonly struct ProductEqualityData
	{
		public readonly ShopType ShopType;
		public readonly ProductCategory Category;
		public readonly ProductSubCategory SubCategory;
		public readonly string Name;

		public ProductEqualityData(ShopType shopType, ProductCategory category, ProductSubCategory subCategory, string name)
		{
			ShopType = shopType;
			Category = category;
			SubCategory = subCategory;
			Name = name;
		}
	}
}
