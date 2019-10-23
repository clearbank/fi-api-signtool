using System.Collections.Generic;
using System.Linq;

namespace FI.API.SignTool.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool HasAny<T>(this IEnumerable<T> enumerable)
        {
            return enumerable != null && enumerable.Any();
        }
    }
}
