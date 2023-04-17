#region

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace JCore
{
    public class BM25
    {
        const double k1 = 1.2;
        const double b = 0.75;
        readonly List<string> _documents;
        readonly Dictionary<string, int> _documentLengths;
        readonly double _averageDocumentLength;
        readonly ITokenizer _tokenizer;

        public BM25(List<string> documents, ITokenizer tokenizer)
        {
            _documents = documents;
            _documentLengths = new Dictionary<string, int>();
            _averageDocumentLength = 0;
            _tokenizer = tokenizer;

            foreach (var document in documents)
            {
                var length = _tokenizer.Tokenize(document).Count;
                _documentLengths[document] = length;
                _averageDocumentLength += length;
            }

            _averageDocumentLength /= documents.Count;
        }

        public List<string> RankDocuments(string query)
        {
            IList<string> queryTerms = _tokenizer.Tokenize(query);
            var documentScores = new Dictionary<string, double>();

            foreach (var document in _documents)
            {
                var score = 0.0;
                foreach (var term in queryTerms)
                {
                    var tf = TermFrequency(term, document);
                    var idf = InverseDocumentFrequency(term, _documents);
                    var termWeight = ((k1 + 1) * tf) /
                                     (k1 * ((1 - b) + b * _documentLengths[document] / _averageDocumentLength) + tf);
                    score += idf * termWeight;
                }

                documentScores[document] = score;
            }

            var rankedResults = new List<string>(documentScores.Keys);
            rankedResults.Sort((d1, d2) => documentScores[d2].CompareTo(documentScores[d1]));

            return rankedResults;
        }

        double TermFrequency(string term, string document)
        {
            // Implementation to calculate the term frequency of 'term' in 'document'
            IList<string> documentTerms = _tokenizer.Tokenize(document);
            var termCount = documentTerms.Count(t => t == term);
            return ((double)termCount) / documentTerms.Count;
        }

        double InverseDocumentFrequency(string term, List<string> documents)
        {
            // Implementation to calculate the inverse document frequency of 'term' in the document collection
            var count = documents.Count(d => _tokenizer.Tokenize(d).Contains(term));
            return Math.Log((documents.Count - count + 0.5) / (count + 0.5));
        }
    }
}