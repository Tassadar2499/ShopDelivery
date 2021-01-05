using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShopDeliveryApplication.Models
{
	public static class SessionExtensions
	{
		public static void AddIdToString(this ISession session, string key, string id)
		{
			var isSuccess = session.TryGetString(key, out var idStr);
			var strValue = !isSuccess ? id : idStr + $";{id}";
			session.SetString(key, strValue);
		}

		public static bool TryGetIdSetByKey(this ISession session, string key, out HashSet<long> idSet)
		{
			var isSuccess = session.TryGetString(key, out var idStr);

			idSet = isSuccess
				? idStr.Split(';').Select(long.Parse).ToHashSet()
				: null;

			return isSuccess;
		}

		public static bool TryGetString(this ISession session, string key, out string value)
		{
			value = null;
			if (!session.Keys.Contains(key))
				return false;

			value = session.GetString(key);

			return !string.IsNullOrEmpty(value);
		}
	}
}