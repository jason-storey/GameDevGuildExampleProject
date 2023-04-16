using System;
using System.Collections.Generic;
using System.Linq;
using PokemonApp;

public static class InvertedIndexExtensions
{
    public static InvertedIndex<T> ToInvertedIndex<T>(this IEnumerable<T> items, Func<T, string[]> labels)
    {
        var index = new InvertedIndex<T>();
        foreach (var item in items) index.Add(item, labels?.Invoke(item));
        return index;
    }

    public static IEnumerable<T> Query<T>(this InvertedIndex<T> index,string query) => new InvertedIndexQuery<T>(index).ExecuteQuery(query);
}