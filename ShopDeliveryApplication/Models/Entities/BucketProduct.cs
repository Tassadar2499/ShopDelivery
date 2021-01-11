using ShopsDbEntities;

namespace ShopDeliveryApplication.Models.Entities
{
	public class BucketProduct
	{
		public Product Product { get; }
		public long Id => Product.Id;
		public string Name => Product.Name;
		public string ImageUrl => Product.ImageUrl;
		public float Price => Product.Price;
		public float? Mass => Product.Mass;
		public int Count { get; set; }

		public BucketProduct(Product product) : this(product, 1)
		{
		}

		public BucketProduct(Product product, int count)
		{
			Product = product;
			Count = count;
		}
	}
}