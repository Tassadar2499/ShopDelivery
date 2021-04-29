using ShopsDbEntities.Entities;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CouriersWebService.Services
{
	public class CouriersCacheLogic
	{
		public const string COURIERS_KEY = "couriers_id";

		private readonly IRedisCacheClient _redisCacheClient;
		public IRedisDatabase RedisDatabase => _redisCacheClient.Db0;

		public CouriersCacheLogic(IRedisCacheClient redisCacheClient) => _redisCacheClient = redisCacheClient;

		public async Task UpdateAsync(Courier courier)
		{
			var courierKey = $"Courier{courier.Id}";
			var isCourierInCache = await RedisDatabase.ExistsAsync(courierKey);
			if (!isCourierInCache)
				await UpdateCouriersIdStorageAsync(courierKey, (s, k) => s.Add(k));

			await RedisDatabase.AddAsync(courierKey, courier);
		}

		public async Task RemoveAsync(Courier courier)
		{
			var courierKey = $"Courier{courier.Id}";
			var isCourierInCache = await RedisDatabase.ExistsAsync(courierKey);
			if (isCourierInCache)
				await UpdateCouriersIdStorageAsync(courierKey, (s, k) => s.Remove(k));

			await RedisDatabase.RemoveAsync(courierKey);
		}

		public async Task<Courier> GetCourierByLoginAsync(string login)
		{
			var couriers = await GetCouriersAsync();

			return couriers.FirstOrDefault(c => c.Login == login);
		}

		public async Task<IEnumerable<Courier>> GetCouriersAsync()
		{
			var keys = await GetCouriersKeysAsync();
			if (keys == null || keys.Count == 0)
				return Enumerable.Empty<Courier>();

			var couriersDict = await RedisDatabase.GetAllAsync<Courier>(keys);

			return couriersDict.Select(kv => kv.Value);
		}

		private async Task<HashSet<string>> GetCouriersKeysAsync()
		{
			var couriersIdStr = await RedisDatabase.GetAsync<string>(COURIERS_KEY);

			return couriersIdStr?
				.Split(';')
				.Select(long.Parse)
				.Select(i => $"Courier{i}")
				.ToHashSet();
		}

		private async Task UpdateCouriersIdStorageAsync(string key, Action<HashSet<string>, string> setAction)
		{
			var couriersIdStr = await RedisDatabase.GetAsync<string>(COURIERS_KEY) ?? "";
			var couriersIdSet = couriersIdStr.Split(';').ToHashSet();
			setAction.Invoke(couriersIdSet, key);
			var newCouriersIdStr = string.Join(';', couriersIdSet);
			await RedisDatabase.AddAsync(COURIERS_KEY, newCouriersIdStr);
		}
	}
}