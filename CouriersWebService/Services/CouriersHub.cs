using Microsoft.AspNetCore.SignalR;

namespace CouriersWebService.Services
{
	public class CouriersHub : Hub
	{
		//public async Task SendOrderInfoAsync(string connectionId, string orderInfo)
		//{
		//	var client = Clients.Client(connectionId);

		//	await client.SendAsync("RecieveOrderInfo", orderInfo);
		//}
	}
}