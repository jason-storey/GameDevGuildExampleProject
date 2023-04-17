using System.Collections.Generic;

namespace JCore.Search
{
    public class AutocompleteTrie {
        private class TrieNode {
            public Dictionary<char, TrieNode> Children = new Dictionary<char, TrieNode>();
            public bool IsWordEnd;
        }

        TrieNode root = new TrieNode();

        public void Insert(string word) {
            var node = root;
            foreach (var c in word) {
                if (!node.Children.ContainsKey(c)) node.Children[c] = new TrieNode();
                node = node.Children[c];
            }
            node.IsWordEnd = true;
        }

        public List<string> Search(string prefix) {
            var node = root;
            foreach (var c in prefix) {
                if (!node.Children.ContainsKey(c)) return new List<string>();
                node = node.Children[c];
            }
            return DFS(node, prefix);
        }

        List<string> DFS(TrieNode node, string prefix) {
            var result = new List<string>();
            if (node.IsWordEnd) result.Add(prefix);
            foreach (var pair in node.Children) result.AddRange(DFS(pair.Value, prefix + pair.Key));
            return result;
        }
    }
}