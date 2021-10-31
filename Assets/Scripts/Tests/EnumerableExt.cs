using System.Collections.Generic;

namespace CatProcessingUnit.Tests
{
    public static class EnumerableExt
    {
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> collection)
        {
            return new HashSet<T>(collection);
        }
    }
}