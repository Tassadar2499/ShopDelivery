using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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

		public static IQueryable<T> WhereByExpression<T>(this IQueryable<T> collection, Expression<Func<T, bool>> predicate) 
			=> collection.Where(predicate);
	}
}