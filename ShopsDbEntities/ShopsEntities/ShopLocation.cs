namespace ShopsDbEntities.ShopsEntities
{
	public class ShopLocation
	{
		public long Id { get; set; }
		public byte ShopId { get; set; }
		public long AddressId { get; set; }
	}
}