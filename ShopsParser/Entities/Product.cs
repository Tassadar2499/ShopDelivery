using System;
using System.Collections.Generic;
using System.Text;

namespace ShopsParser
{
	public enum ShopType : byte
	{
		None = 0,
		Okey = 1
	}

	public enum ProductCategory : byte
	{
		None = 0,
		Meat = 1
	}

	public enum ProductSubCategory : byte
	{
		None = 0,
		Chicken = 1
	}

	public record Product
	{
		public ShopType Shop { get; init; }
		public ProductCategory Category { get; init; }
		public ProductSubCategory SubCategory { get; init; }
		public string Name { get; init; }
		public float Price { get; init; }
		public float? Mass { get; init; }
		public string ImgContent { get; init; }

		public Product(ShopType shop, ProductCategory category, ProductSubCategory subCategory, string name, float price, float? mass, string imgContent)
			=> (Shop, Category, SubCategory, Name, Price, Mass, ImgContent) = (shop, category, subCategory, name, price, mass, imgContent);
	}
}
