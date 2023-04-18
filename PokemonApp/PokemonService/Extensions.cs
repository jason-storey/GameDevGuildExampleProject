using System;
using System.Collections.Generic;

namespace PokemonService
{
    static class Extensions 
    {
        public static Dictionary<string, TResult> ToDictionary<TListItem, TResult>(this IEnumerable<TListItem> items,
            Func<TListItem, string> key, Func<TListItem, TResult> val)
        {
            var dict = new Dictionary<string, TResult>();
            if (key == null || val == null) return dict;
            foreach (var item in items)
            {
                var keyVal = key.Invoke(item);
                if (string.IsNullOrWhiteSpace(keyVal)) continue;
                var actualVal = val.Invoke(item);
                dict.Add(keyVal, actualVal);
            }

            return dict;
        }
    }
}