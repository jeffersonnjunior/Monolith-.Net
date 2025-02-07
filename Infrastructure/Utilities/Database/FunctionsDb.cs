using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Utilities.Db;

public static class IncludesFunction
{
    public static IQueryable<T> Includes<T>(this IQueryable<T> query, params string[] includes) where T : class
    {
        var aux = query;

        if (includes == null) return aux;
        foreach (string include in includes)
        {
            aux = aux.Include(include);
        }

        return aux;
    }
}