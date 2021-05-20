using HarabaSourceGenerators.Common.Attributes;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CouriersWebService.Services
{
	[Inject]
	public partial class CouriersNotifyService
	{
		private readonly IHubContext<CouriersHub> _hub;
		private readonly ILogger<CouriersNotifyService> _logger;

		public async Task SendNotificationAsync(string connectionId, string orderInfo)
		{
			var client = _hub.Clients.Client(connectionId);
			if (client == null)
			{
				_logger.LogError($"Cannot find client by signalR id = {connectionId}");
				return;
			}

			_logger.LogInformation($"Send order info = {orderInfo} by signalrId = {connectionId}");
			await client.SendAsync("RecieveOrderInfo", orderInfo);
		}
	}
}