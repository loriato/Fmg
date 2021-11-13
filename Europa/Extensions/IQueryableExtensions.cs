using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Europa.Extensions
{
    public static class QueryableExtensions
    {
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
        {
            return source.OrderBy(ToLambda<T>(propertyName));
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
        {
            return source.OrderByDescending(ToLambda<T>(propertyName));
        }

        private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
        {
            var path = propertyName.Split('.');

            ParameterExpression param = Expression.Parameter(typeof(T));
            var expressions = new List<MemberExpression>();
            for (int idx = 0; idx < path.Length; idx++)
            {
                Expression objectNativate = idx == 0 ? (Expression)param : expressions[idx - 1];
                expressions.Insert(idx, Expression.Property(objectNativate, path[idx]));
            }

            UnaryExpression unaryExpression = Expression.Convert(expressions[expressions.Count - 1], typeof(object));
            return Expression.Lambda<Func<T, object>>(unaryExpression, param);
        }
    }
}