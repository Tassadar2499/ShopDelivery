using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ShopsDbEntities;
using ShopsDbEntities.Entities;
using ShopsDbEntities.Entities.ProductEntities;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourierService.Services
{
	public class OrdersExecutor
	{
		private readonly IServiceProvider _scopeFactory;
		private readonly IRedisCacheClient _redisCacheClient;
		private IRedisDatabase RedisDatabase => _redisCacheClient.Db0;
		public OrdersExecutor(IRedisCacheClient redisCacheClient, IServiceProvider scopeFactory)
			=> (_redisCacheClient, _scopeFactory) = (redisCacheClient, scopeFactory);

		//TODO:
		public async Task ErrorHandlerAsync(ProcessErrorEventArgs arg)
		{
			Console.WriteLine("Error in Asure service bus");
		}

		public async Task MessageHandlerAsync(ProcessMessageEventArgs arg)
		{
			var message = arg.Message;
			var body = message.Body.ToString();
			Console.WriteLine($"Received: {body}");

			await HandleOrderAsync(body);
			await arg.CompleteMessageAsync(message);
		}

		//TODO:
		private async Task HandleOrderAsync(string message)
		{
			//{ "Id": 1, "BucketProducts": "[2, 4, 4, 2, 5]", "UserAddressId": 1 }

			var order = JsonConvert.DeserializeObject<Order>(message);

			using var scope = _scopeFactory.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<MainDbContext>();

			var usersAddress = await context.UsersAddresses.FindAsync(order.UserAddressId);
			var address = await context.Addresses.FindAsync(usersAddress.AddressId);

			var coords = (address.Longitude, address.Latitude);

			var couriersIdStr = await RedisDatabase.GetAsync<string>(AuthCouriersService.COURIERS_KEY);
			if (string.IsNullOrEmpty(couriersIdStr))
				return;

			couriersIdStr = couriersIdStr[..^1];
			var couriersKeys = couriersIdStr
				.Split(';')
				.Select(long.Parse)
				.Select(i => $"Courier{i}")
				.ToHashSet();

			var couriersDict = await RedisDatabase.GetAllAsync<Courier>(couriersKeys);
			var activeCouriers = couriersDict
				.Select(kv => kv.Value)
				.Where(c => c.Status == CourierStatus.Active)
				.Select(c => (Courier: c, Value: GetDeltaCoords(coords, (c.Longitude, c.Latitude))))
				.ToArray();

			var minValue = activeCouriers.Min(c => c.Value);
			var correctCourier = activeCouriers.FirstOrDefault(c => c.Value == minValue).Courier;

			var gg = correctCourier?.Login;
		}

		private static double GetDeltaCoords((double Longitude, double Latitude) first, (double Longitude, double Latitude) second)
			=> Math.Abs(first.Latitude - second.Latitude) + Math.Abs(first.Longitude - second.Longitude);
	}
}
