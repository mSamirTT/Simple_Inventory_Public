using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace StarterApp.Core.Common.PageSort
{
    public static class PageSortHelper
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> query, string propertyName, bool desc)
            where T : class
        {
            if (string.IsNullOrEmpty(propertyName)) return query;

            propertyName = propertyName.First().ToString().ToUpper() + propertyName.Substring(1);

            PropertyInfo prop = typeof(T).GetProperty(propertyName);

            if (prop == null)
                throw new Exception("No Such Property " + propertyName + " In Class " + typeof(T).Name);

            if (prop.PropertyType == typeof(string))
            {
                var exp = Expressions.PropertyExpression<T, string>(propertyName);
                query = query.SortWith(exp, desc);
            }
            else if (prop.PropertyType == typeof(double))
            {
                var exp = Expressions.PropertyExpression<T, double>(propertyName);
                query = query.SortWith(exp, desc);
            }
            else if (prop.PropertyType == typeof(double?))
            {
                var exp = Expressions.PropertyExpression<T, double?>(propertyName);
                query = query.SortWith(exp, desc);
            }
            else if (prop.PropertyType == typeof(decimal))
            {
                var exp = Expressions.PropertyExpression<T, decimal>(propertyName);
                query = query.SortWith(exp, desc);
            }
            else if (prop.PropertyType == typeof(decimal?))
            {
                var exp = Expressions.PropertyExpression<T, decimal?>(propertyName);
                query = query.SortWith(exp, desc);
            }
            else if (prop.PropertyType == typeof(float))
            {
                var exp = Expressions.PropertyExpression<T, float>(propertyName);
                query = query.SortWith(exp, desc);
            }
            else if (prop.PropertyType == typeof(float?))
            {
                var exp = Expressions.PropertyExpression<T, float?>(propertyName);
                query = query.SortWith(exp, desc);
            }
            else if (prop.PropertyType == typeof(long))
            {
                var exp = Expressions.PropertyExpression<T, long>(propertyName);
                query = query.SortWith(exp, desc);
            }
            else if (prop.PropertyType == typeof(long?))
            {
                var exp = Expressions.PropertyExpression<T, long?>(propertyName);
                query = query.SortWith(exp, desc);
            }
            else if (prop.PropertyType == typeof(int))
            {
                var exp = Expressions.PropertyExpression<T, int>(propertyName);
                query = query.SortWith(exp, desc);
            }
            else if (prop.PropertyType == typeof(int?))
            {
                var exp = Expressions.PropertyExpression<T, int?>(propertyName);
                query = query.SortWith(exp, desc);
            }
            else if (prop.PropertyType == typeof(DateTime))
            {
                var exp = Expressions.PropertyExpression<T, DateTime>(propertyName);
                query = query.SortWith(exp, desc);
            }
            else if (prop.PropertyType == typeof(DateTime?))
            {
                var exp = Expressions.PropertyExpression<T, DateTime?>(propertyName);
                query = query.SortWith(exp, desc);
            }
            else
            {
                var exp = Expressions.Property<T>(propertyName);
                query = query.SortWith(exp, desc);
            }

            return query;
        }

        public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query,
            int pageNum,
            int pageSize,
            out int count)
        {
            count = query.Count();
            query = query
                .Skip(pageNum * pageSize)
                .Take(pageSize);
            return query;
        }
    }

    public class Expressions
    {
        public static MemberExpression GetMemberExpression(ParameterExpression pExp, string propertyName)
        {
            MemberExpression mExp = null;
            if (propertyName.Contains("."))
            {
                string[] parts = propertyName.Split('.');
                mExp = Expression.Property(pExp, parts[0]);
                for (int i = 1; i < parts.Length; i++)
                {
                    mExp = Expression.Property(mExp, parts[i]);
                }
            }
            else
            {
                mExp = Expression.Property(pExp, propertyName);
            }
            return mExp;
        }
        public static Expression<Func<T, TVal>> PropertyExpression<T, TVal>(string property) where T : class
        {
            var par = Expression.Parameter(typeof(T));
            var prop = GetMemberExpression(par, property);
            var propcon = Expression.Convert(prop, typeof(TVal));
            return Expression.Lambda<Func<T, TVal>>(propcon, par);
        }
        public static Expression<Func<T, object>> Property<T>(string property) where T : class
        {
            var par = Expression.Parameter(typeof(T));
            var prop = Expression.Property(par, property);

            return Expression.Lambda<Func<T, object>>(prop, par);
        }
    }

    public static class LinqExtenstions
    {
        public static IQueryable<T> SortWith<T, TVal>(this IQueryable<T> q, Expression<Func<T, TVal>> exp, bool desc) where T : class
        {
            return desc ? q.OrderByDescending(exp) : q.OrderBy(exp);
        }
    }
}
