using System;
using System.Collections.Generic;
using System.Linq;
using PokemonApp;

public static class InvertedIndexExtensions
{
    public static InvertedIndex<T> Filter<T>(this IEnumerable<T> items, Func<T, string[]> labels)
    {
        var index = new InvertedIndex<T>();
        foreach (var item in items) index.Add(item, labels?.Invoke(item));
        return index;
    }
    
    public static InvertedIndex<T> Filter<T>(this IEnumerable<T> items, Func<T, string> labels)
    {
        var index = new InvertedIndex<T>();
        foreach (var item in items) index.Add(item, labels?.Invoke(item));
        return index;
    }

    public static InvertedIndex<string> ReverseIndex(this InvertedIndex<string> index)
    {
        var dict = index.ToDictionary();
        var newIndex = new InvertedIndex<string>();
        foreach (var kvp in dict)
        {
            foreach (var innerValue in kvp.Value) 
                newIndex.Add(innerValue, kvp.Key);
        }
        return newIndex;
    }

    public static InvertedIndex<T> ToIndex<T>(this Dictionary<T, List<string>> dictionary)
    {
        var index = new InvertedIndex<T>();
        foreach (var kvp in dictionary) 
            index.Add(kvp.Key, kvp.Value.ToArray());
        return index;
    }

    public static IEnumerable<T> Query<T>(this InvertedIndex<T> index,string query,InvertedIndex<string> aliases) => 
        string.IsNullOrWhiteSpace(query) ? index : new InvertedIndexQuery<T>(index).ExecuteQuery(query,(token)=> aliases.ContainsKey(token) ? aliases[token].First() : token);

    public static IEnumerable<T> Query<T>(this InvertedIndex<T> index,string query,Func<string,string> beforeTokenProcess = null) => 
        string.IsNullOrWhiteSpace(query) ? index : new InvertedIndexQuery<T>(index).ExecuteQuery(query,beforeTokenProcess);
}