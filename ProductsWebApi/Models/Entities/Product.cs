using Newtonsoft.Json;
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
	
	[JsonObject]
	public class Product
	{
		[JsonProperty]
		public long Id { get; set; }
		[JsonProperty]
		public Shop Shop { get; set; }
		[JsonProperty]
		public ProductCategory Category { get; set; }
		[JsonProperty]
		public ProductSubCategory SubCategory { get; set; }
		[JsonProperty]
		public string Name { get; set; }
		[JsonProperty]
		public float Price { get; set; }
		[JsonProperty]
		public float? Mass { get; set; }
		[JsonProperty]
		public string ImgContent { get; set; }
	}
}
