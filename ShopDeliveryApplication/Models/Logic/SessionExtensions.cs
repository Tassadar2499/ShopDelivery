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

		public static bool TryGetIdArrByKey(this ISession session, string key, out long[] idArr)
		{
			var isSuccess = session.TryGetString(key, out var idStr);

			idArr = isSuccess
				? idStr.Replace("[", "")
					.Replace("]", "")
					.Split(',')
					.Select(long.Parse)
					.ToArray()
				: null;

			return isSuccess;
		}

		private static bool TryGetString(this ISession session, string key, out string value)
		{
			value = null;
			if (!session.Keys.Contains(key))
				return false;

			value = session.GetString(key);

			return !string.IsNullOrEmpty(value);
		}
	}
}