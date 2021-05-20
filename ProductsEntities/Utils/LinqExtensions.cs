using System;
using System.Collections.Generic;

namespace ShopsDbEntities.Utils
{
	public static class LinqExtensions
	{
		public static (List<T> First, List<T> Second) SplitCollection<T>(this IEnumerable<T> collection, Predicate<T> predicate)
		{
			var first = new List<T>();
			var second = new List<T>();

			foreach (var item in collection)
			{
				if (predicate(item))
					first.Add(item);
				else
					second.Add(item);
			}

			return (first, second);
		}
	}
}