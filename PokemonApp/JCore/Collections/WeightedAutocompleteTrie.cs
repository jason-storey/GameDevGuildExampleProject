using System.Collections.Generic;
using System.Linq;

namespace JCore.Search
{
    public class WeightedAutocompleteTrie 
    {
        class TrieNode {
            public Dictionary<char, TrieNode> Children = new Dictionary<char, TrieNode>();
            public int? Weight;
        }

        TrieNode root = new TrieNode();

        public void Insert(string word, int weight) {
            var node = root;
            foreach (var c in word) {
                if (!node.Children.ContainsKey(c)) node.Children[c] = new TrieNode();
                node = node.Children[c];
            }
            node.Weight = weight;
        }

        public IList<string> Autocomplete(string prefix,bool byWeight = true)
        {
            var results = DFS(SearchNode(prefix), prefix);
            return byWeight ? results.OrderByDescending(x => x.Weight).Select(x => x.Word).ToList() : results.OrderBy(x => x.Word).Select(x=>x.Word).ToList();
        }

        TrieNode SearchNode(string prefix) {
            var node = root;
            foreach (var c in prefix) {
                if (!node.Children.ContainsKey(c)) return null;
                node = node.Children[c];
            }
            return node;
        }

        List<(string Word, int Weight)> DFS(TrieNode node, string prefix) {
            if (node == null) return new List<(string, int)>();
            var result = new List<(string, int)>();
            if (node.Weight.HasValue) result.Add((prefix, node.Weight.Value));
            foreach (var pair in node.Children) result.AddRange(DFS(pair.Value, prefix + pair.Key));
            return result;
        }
    }
}