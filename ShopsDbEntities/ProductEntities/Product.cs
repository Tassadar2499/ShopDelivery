namespace ShopsDbEntities
{
	public class Product
	{
		public long Id { get; set; }
		public ShopType ShopType { get; set; }
		public ProductCategory Category { get; set; }
		public ProductSubCategory SubCategory { get; set; }
		public string Name { get; set; }
		public float Price { get; set; }
		public float? Mass { get; set; }
		public string ImageUrl { get; set; }
	}
}