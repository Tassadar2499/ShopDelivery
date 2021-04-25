using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CouriersWebService.Services
{
	public class CouriersHub : Hub
	{
		public async Task SendOrderInfoAsync(string login)
		{
			await Clients.All.SendAsync("Send", login);
		}
	}
}
