using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace CouriersWebService.Services
{
	public class CouriersHub : Hub
	{
		private readonly CouriersCacheLogic _couriersCacheLogic;

		public CouriersHub(CouriersCacheLogic couriersCacheLogic) => _couriersCacheLogic = couriersCacheLogic;

		public async Task SendOrderInfoAsync(string login, string orderInfo)
		{
			var courier = await _couriersCacheLogic.GetCourierByLogin(login);
			var client = Clients.Client(courier.SignalRConnectionId);

			await client.SendAsync("RecieveOrderInfo", orderInfo);
		}
	}
}