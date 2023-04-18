using System.Collections.Generic;

namespace JCore.Search
{
    public class AutocompleteTrie {
        readonly bool _caseInsensitive;

        class TrieNode {
            public Dictionary<char, TrieNode> Children = new Dictionary<char, TrieNode>();
            public bool IsWordEnd;
        }

        public AutocompleteTrie(bool caseInsensitive = true)
        {
            _caseInsensitive = caseInsensitive;
        }
        TrieNode root = new TrieNode();

        public void Insert(string word)
        {
            if (_caseInsensitive)
            {
                PerformInsert(word.ToUpperInvariant());
                PerformInsert(word.ToLowerInvariant());
            }else
                PerformInsert(word);
        }
        
         void PerformInsert(string word) {
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