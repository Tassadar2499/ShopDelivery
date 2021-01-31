using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace ShopDeliveryApplication.Models
{
	public static class SessionExtensions
	{
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