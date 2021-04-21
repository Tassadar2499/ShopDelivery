using ShopsDbEntities.Entities;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourierService.Services
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

			await RedisDatabase.SetAddAsync(courierKey, courier);
		}

		public async Task RemoveAsync(Courier courier)
		{
			var courierKey = $"Courier{courier.Id}";
			var isCourierInCache = await RedisDatabase.ExistsAsync(courierKey);
			if (isCourierInCache)
				await UpdateCouriersIdStorageAsync(courierKey, (s, k) => s.Remove(k));

			await RedisDatabase.RemoveAsync($"Courier{courier.Id}");
		}

		public async Task<HashSet<string>> GetCouriersKeysAsync()
		{
			var couriersIdStr = await RedisDatabase.GetAsync<string>(AuthCouriersService.COURIERS_KEY);

			return couriersIdStr?
				.Split(';')
				.Select(long.Parse)
				.Select(i => $"Courier{i}")
				.ToHashSet();
		}

		public async Task<IEnumerable<Courier>> GetCouriersByKeysAsync(IEnumerable<string> keys)
		{
			var couriersDict = await RedisDatabase.GetAllAsync<Courier>(keys);

			return couriersDict.Select(kv => kv.Value);
		}

		private async Task UpdateCouriersIdStorageAsync(string key, Action<HashSet<string>, string> setAction)
		{
			var couriersIdStr = await RedisDatabase.GetAsync<string>(COURIERS_KEY) ?? "";
			var couriersIdSet = couriersIdStr.Split(';').ToHashSet();
			setAction.Invoke(couriersIdSet, key);
			couriersIdSet.Remove(key);
			var newCouriersIdStr = string.Join(';', couriersIdSet);
			await RedisDatabase.SetAddAsync(COURIERS_KEY, newCouriersIdStr);
		}
	}
}