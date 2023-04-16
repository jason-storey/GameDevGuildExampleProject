using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace PokemonApp
{
    public class InvertedIndex<T> : IEnumerable<T>
    {
        readonly Dictionary<string, HashSet<T>> _indexes;
        readonly HashSet<T> _elements;
        public InvertedIndex()
        {
            _elements = new HashSet<T>();
            _indexes = new Dictionary<string, HashSet<T>>(StringComparer.OrdinalIgnoreCase);
        }

        public IEnumerable<T> this[int index]
        {
            get
            {
                var matching = Keys.ElementAtOrDefault(index);
                return string.IsNullOrWhiteSpace(matching) ? Enumerable.Empty<T>() : _indexes[matching];
            }
        }
        public IEnumerable<T> this[string key] => _indexes.TryGetValue(key, out var k) ? k : Enumerable.Empty<T>();
        public int ValueCount => _elements.Count;
        public int KeyCount => _indexes.Count;
        public IEnumerable<string> Keys => _indexes.Keys;
        public IEnumerable<T> Values => _elements;
        public bool IsDirty { get; private set; }

        public IEnumerable<T> WithAllOf(params string[] tags)
        {
            if (tags == null || tags.Length == 0) return Enumerable.Empty<T>();
            var tagSet = new HashSet<string>(tags, StringComparer.OrdinalIgnoreCase);
            string smallestTag = null;
            int smallestTagItemCount = int.MaxValue;
            foreach (string tag in tagSet)
            {
                if (_indexes.TryGetValue(tag, out var itemsWithTag))
                {
                    if (itemsWithTag.Count >= smallestTagItemCount) continue;
                    smallestTag = tag;
                    smallestTagItemCount = itemsWithTag.Count;
                }
                else
                    return Enumerable.Empty<T>();
            }

            tagSet.Remove(smallestTag);
            var results = new List<T>();
            foreach (T item in _indexes[smallestTag])
            {
                bool hasAllTags = true;
                foreach (string tag in tagSet)
                {
                    if (_indexes[tag].Contains(item)) continue;
                    hasAllTags = false;
                    break;
                }

                if (hasAllTags) results.Add(item);
            }
            return results;
        }
        
        public IEnumerable<T> FindByAnyTag(params string[] tags)
        {
            if (tags == null || tags.Length == 0) return Enumerable.Empty<T>();
            string largestTag = null;
            int largestTagItemCount = int.MinValue;
            foreach (string tag in tags)
            {
                if (!_indexes.TryGetValue(tag, out var itemsWithTag)) continue;
                if (itemsWithTag.Count <= largestTagItemCount) continue;
                largestTag = tag;
                largestTagItemCount = itemsWithTag.Count;
            }
            if (largestTag == null) return Enumerable.Empty<T>();
            var results = new HashSet<T>(_indexes[largestTag]);
            foreach (string tag in tags)
            {
                if (tag != largestTag && _indexes.TryGetValue(tag, out var itemsWithTag))
                    results.UnionWith(itemsWithTag);
            }
            return results;
        }
        
        public void Add(T item,bool dontDirty, params string[] labels)
        {
            var existingDirtyFlag = IsDirty;
            Add(item,labels);
            IsDirty = !dontDirty || existingDirtyFlag;
        }
        public void Add(T item,params string[] labels)
        {
            _elements.Add(item);
            foreach (string label in labels)
            {
                if (string.IsNullOrWhiteSpace(label)) continue;
                if (!_indexes.ContainsKey(label))
                    _indexes[label] = new HashSet<T> { item };
                else
                    _indexes[label].Add(item);
            }
            IsDirty = true;
        }

        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_elements).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public class JsonConverter : Newtonsoft.Json.JsonConverter
        {
            public override bool CanConvert(Type objectType) => objectType == typeof(InvertedIndex<T>);

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var invertedIndex = (InvertedIndex<T>)value;
                writer.WriteStartObject();
                foreach (var key in invertedIndex.Keys)
                {
                    writer.WritePropertyName(key);
                    writer.WriteStartArray();
                    foreach (var val in invertedIndex[key]) 
                        writer.WriteValue(val);
                    writer.WriteEndArray();
                }
                writer.WriteEndObject();
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null)
                    return null;

                var invertedIndex = new InvertedIndex<T>();

                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        var label = reader.Value.ToString();
                        reader.Read();

                        if (reader.TokenType != JsonToken.StartArray) continue;
                        while (reader.Read())
                        {
                            if (reader.TokenType == JsonToken.EndArray)
                                break;

                            T item = serializer.Deserialize<T>(reader);
                            invertedIndex.Add(item,true, label);
                        }
                    }
                    else if (reader.TokenType == JsonToken.EndObject)
                    {
                        break;
                    }
                }

                return invertedIndex;
            }
        }
    }
}