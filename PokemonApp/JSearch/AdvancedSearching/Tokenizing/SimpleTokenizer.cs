using System;
using System.Collections.Generic;
using System.Linq;
using JCore.AdvancedSearch;

namespace JSearch
{
    public class SimpleTokenizer : ITokenizer
    {
        readonly char[] _separators;

        public SimpleTokenizer(char[] separators = null) => _separators = separators ?? DEFAULT_TOKENS;

        public List<string> Tokenize(string input) =>
            input.Split(_separators, StringSplitOptions.RemoveEmptyEntries)
                .Select(token => token.ToLowerInvariant().Trim())
                .ToList();

        static readonly char[] DEFAULT_TOKENS = { ' ', ',', '.', ';', ':', '!', '?', '(', ')', '[', ']', '{', '}', '<', '>' };
    }
}