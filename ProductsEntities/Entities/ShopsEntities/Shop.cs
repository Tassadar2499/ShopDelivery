namespace ShopsDbEntities
{
	public enum ShopType : byte
	{
		None = 0,
		Okey = 1
	}

	public class Shop
	{
		public byte Id { get; set; }
		public string Name { get; set; }
		public string ImgUrl { get; set; }
	}
}