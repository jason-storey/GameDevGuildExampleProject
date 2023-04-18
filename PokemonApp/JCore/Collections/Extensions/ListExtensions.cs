using System;
using System.Collections.Generic;

namespace JCore.Extensions
{
    public static class ListExtensions
    {
        public static void RemoveDuplicates<T>(this List<T> list)
        {
            var set = new HashSet<T>();
            for (int i = list.Count - 1; i >= 0; i--)
                if (!set.Add(list[i]))
                    list.RemoveAt(i);
        }


    }
}