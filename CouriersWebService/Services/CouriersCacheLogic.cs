using HarabaSourceGenerators.Common.Attributes;
using ShopsDbEntities.Entities;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CouriersWebService.Services
{
	//TODO: Добавить обработку исключений
	[Inject]
	public partial class CouriersCacheLogic
	{
		private const string COURIERS_KEY = "couriers_id";
		private const string COURIER_PREFIX = "Courier";

		private readonly IRedisCacheClient _redisCacheClient;
		public IRedisDatabase RedisComplex => _redisCacheClient.Db0;
		public IDatabase RedisSimple => RedisComplex.Database;

		public async Task UpdateAsync(Courier courier)
		{
			var courierKey = $"{COURIER_PREFIX}{courier.Id}";
			var isCourierInCache = await RedisComplex.ExistsAsync(courierKey);
			if (!isCourierInCache)
				await UpdateCouriersIdStorageAsync(courierKey, (s, k) => s.Add(k));

			await RedisComplex.AddAsync(courierKey, courier);
		}

		public async Task RemoveAsync(Courier courier)
		{
			var courierKey = $"{COURIER_PREFIX}{courier.Id}";
			var isCourierInCache = await RedisComplex.ExistsAsync(courierKey);
			if (isCourierInCache)
				await UpdateCouriersIdStorageAsync(courierKey, (s, k) => s.Remove(k));

			await RedisComplex.RemoveAsync(courierKey);
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

			var couriersDict = await RedisComplex.GetAllAsync<Courier>(keys);

			return couriersDict.Select(kv => kv.Value).Where(c => c != null);
		}

		private async Task UpdateCouriersIdStorageAsync(string key, Action<HashSet<string>, string> setAction)
		{
			var couriersIdSet = await GetCouriersKeysAsync();
			setAction.Invoke(couriersIdSet, key);
			var newCouriersIdStr = string.Join(';', couriersIdSet);
			await RedisComplex.AddAsync(COURIERS_KEY, newCouriersIdStr);
		}

		private async Task<HashSet<string>> GetCouriersKeysAsync()
		{
			var redisValue = await RedisSimple.StringGetAsync(COURIERS_KEY);

			if (redisValue.IsNullOrEmpty)
				return new HashSet<string>() { };

			var jj = redisValue.ToString();

			return redisValue
				.ToString()
				.Split(';')
				.Select(s => s.Replace("\"", string.Empty))
				.Where(k => k.StartsWith(COURIER_PREFIX))
				.ToHashSet();
		}
	}
}