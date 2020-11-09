using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsWebApi.Models
{
	public enum Shop : byte
	{ 
		Okey = 1
	}

	public enum ProductCategory : byte
	{ 
		Meat = 1
	}

	public enum ProductSubCategory : byte
	{ 
		Chicken = 1
	}
	
	public class Product
	{
		public long Id { get; set; }
		public Shop Shop { get; set; }
		public ProductCategory Category { get; set; }
		public ProductSubCategory SubCategory { get; set; }
		public string Name { get; set; }
		public float Price { get; set; }
		public float? Mass { get; set; }
		public string ImgContent { get; set; }
	}
}
