namespace ShopsDbEntities.Entities
{
	public enum CourierStatus : byte
	{
		Active = 1,
		Sleep = 2,
		Work = 3
	}

	public class Courier
	{
		public long Id { get; set; }
		public CourierStatus Status { get; set; }
		public string SignalRConnectionId { get; set; }
		public string Login { get; set; }
		public string Password { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
	}
}