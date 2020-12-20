namespace ShopsDbEntities
{
	public enum ProductSubCategory : byte
	{
		None = 0,
		Chicken = 1
	}

	public class ProductDbSubCategory
	{
		public byte Id { get; set; }
		public string Name { get; set; }
		public string ImgUrl { get; set; }
	}
}