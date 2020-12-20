namespace ShopsDbEntities.ShopsEntities
{
	public class ShopLocation
	{
		public long Id { get; set; }
		public byte ShopId { get; set; }
		public string Street { get; set; }
		public string House { get; set; }
	}
}