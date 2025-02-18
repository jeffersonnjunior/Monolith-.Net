using System.Linq.Expressions;

namespace Infrastructure.Utilities.FunctionsDatabase;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyDynamicFilters<T>(this IQueryable<T> query, Dictionary<string, string> filters)
    {
        if (filters == null || !filters.Any())
            return query;

        var parameter = Expression.Parameter(typeof(T), "x");
        Expression combined = null;

        foreach (var filter in filters)
        {
            if (!string.IsNullOrEmpty(filter.Value))
            {
                var propertyName = filter.Key.EndsWith("Contains") ? filter.Key.Replace("Contains", "") : filter.Key;
                var member = Expression.Property(parameter, propertyName);
                var constant = Expression.Constant(filter.Value);

                if (member.Type == typeof(string))
                {
                    var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    var body = Expression.Call(member, containsMethod, constant);

                    combined = combined == null ? body : Expression.AndAlso(combined, body);
                }
                else if (member.Type == typeof(int) || member.Type == typeof(double) || member.Type == typeof(decimal))
                {
                    var numericValue = Convert.ChangeType(filter.Value, member.Type);
                    var numericConstant = Expression.Constant(numericValue);
                    var body = Expression.Equal(member, numericConstant);

                    combined = combined == null ? body : Expression.AndAlso(combined, body);
                }
            }
        }

        if (combined == null)
            return query;

        var lambda = Expression.Lambda<Func<T, bool>>(combined, parameter);
        return query.Where(lambda);
    }
}
