using System;
using System.Collections.Generic;

namespace Core.Extensions
{
    public static class ListExtensions
    {
        public static IEnumerable<List<T>> Chunk<T>(this List<T> input, int chunkSize)
        {
            for (int i = 0; i < input.Count; i += chunkSize)
            {
                yield return input.GetRange(i, Math.Min(chunkSize, input.Count - i));
            }
        }
    }
}
