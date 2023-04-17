using System.Collections.Generic;

namespace JCore.Search
{
    public class BKTree
    {
        class Node
        {
            public string Word;
            public Dictionary<int, Node> Children = new Dictionary<int, Node>();

            public Node(string word) => Word = word;
        }

        Node root;

        public void Insert(string word)
        {
            if (root == null)
            {
                root = new Node(word);
                return;
            }

            var node = root;
            while (true)
            {
                int d = Levenshtein.Calculate(node.Word, word);
                if (d == 0) return;
                if (!node.Children.TryGetValue(d, out var child))
                {
                    node.Children[d] = new Node(word);
                    break;
                }

                node = child;
            }
        }

        public List<string> Query(string word, int maxDistance)
        {
            var results = new List<string>();
            var candidates = new Stack<Node>();
            if (root != null) candidates.Push(root);

            while (candidates.Count > 0)
            {
                var node = candidates.Pop();
                int d = Levenshtein.Calculate(node.Word, word);
                if (d <= maxDistance) results.Add(node.Word);
                int minD = d - maxDistance, maxD = d + maxDistance;
                foreach (var kv in node.Children)
                {
                    if (kv.Key >= minD && kv.Key <= maxD) candidates.Push(kv.Value);
                }
            }

            return results;
        }
    }
}