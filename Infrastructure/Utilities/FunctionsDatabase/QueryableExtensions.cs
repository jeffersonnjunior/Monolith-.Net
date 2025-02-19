using System.Linq.Expressions;

namespace Infrastructure.Utilities.FunctionsDatabase;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyDynamicFilters<T>(this IQueryable<T> query, Dictionary<string, string> filters, int pageSize = 10, int pageNumber = 1)
    {
        if (filters == null || !filters.Any())
            return ApplyPagination(query, pageSize, pageNumber);

        var parameter = Expression.Parameter(typeof(T), "x");
        Expression combined = null;

        foreach (var filter in filters)
        {
            if (string.IsNullOrEmpty(filter.Value))
                continue;

            var propertyName = filter.Key.EndsWith("Contains") ? filter.Key.Replace("Contains", "") : filter.Key;
            var member = Expression.Property(parameter, propertyName);
            var constant = Expression.Constant(filter.Value);

            Expression body = member.Type switch
            {
                Type t when t == typeof(string) => BuildStringContainsExpression(member, constant),
                Type t when t == typeof(int) || t == typeof(double) || t == typeof(decimal) => BuildNumericEqualsExpression(member, constant),
                _ => throw new NotSupportedException($"The type '{member.Type}' is not supported for dynamic filtering.")
            };

            combined = combined == null ? body : Expression.AndAlso(combined, body);
        }

        if (combined != null)
        {
            var lambda = Expression.Lambda<Func<T, bool>>(combined, parameter);
            query = query.Where(lambda);
        }

        return ApplyPagination(query, pageSize, pageNumber);
    }

    private static Expression BuildStringContainsExpression(MemberExpression member, ConstantExpression constant)
    {
        var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        return Expression.Call(member, containsMethod, constant);
    }

    private static Expression BuildNumericEqualsExpression(MemberExpression member, ConstantExpression constant)
    {
        var numericValue = Convert.ChangeType(constant.Value, member.Type);
        var numericConstant = Expression.Constant(numericValue);
        return Expression.Equal(member, numericConstant);
    }

    private static IQueryable<T> ApplyPagination<T>(IQueryable<T> query, int pageSize, int pageNumber)
    {
        return query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
    }
}