using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace CouriersWebService.Services
{
	public class CouriersNotifyService
	{
		private readonly IHubContext<CouriersHub> _hub;

		public CouriersNotifyService(IHubContext<CouriersHub> hub) => _hub = hub;

		public async Task SendNotificationAsync(string connectionId, string orderInfo)
			=> await _hub.Clients.Client(connectionId).SendAsync("RecieveOrderInfo", orderInfo);
	}
}