using System;
using System.Collections.Generic;
using System.Linq;

namespace JCore
{
    public class TfIdfRanker
    {
        ITokenizer _tokenizer;
        List<string> _documents;
        Dictionary<string, Dictionary<string, double>> _tfIdfMatrix;

        public TfIdfRanker(List<string> documents, ITokenizer tokenizer)
        {
            _tokenizer = tokenizer;
            _documents = documents;
            _tfIdfMatrix = new Dictionary<string, Dictionary<string, double>>();

            foreach (string document in documents)
            {
                Dictionary<string, double> tfIdfVector = CalculateTfIdfVector(document);
                _tfIdfMatrix[document] = tfIdfVector;
            }
        }

        public List<string> RankDocuments(string query)
        {
            Dictionary<string, double> queryVector = CalculateTfIdfVector(query);
            Dictionary<string, double> documentScores = new Dictionary<string, double>();

            foreach (string document in _documents)
            {
                double score = 0.0;
                foreach (string term in queryVector.Keys)
                {
                    if (_tfIdfMatrix[document].ContainsKey(term))
                    {
                        score += queryVector[term] * _tfIdfMatrix[document][term];
                    }
                }
                documentScores[document] = score;
            }

            List<string> rankedResults = new List<string>(documentScores.Keys);
            rankedResults.Sort((d1, d2) => documentScores[d2].CompareTo(documentScores[d1]));

            return rankedResults;
        }

        Dictionary<string, double> CalculateTfIdfVector(string text)
        {
            IList<string> tokens = _tokenizer.Tokenize(text).ToList();
            Dictionary<string, double> tfIdfVector = new Dictionary<string, double>();

            foreach (string term in tokens)
            {
                double tf = TermFrequency(term, tokens);
                double idf = InverseDocumentFrequency(term, _documents);
                tfIdfVector[term] = tf * idf;
            }

            return tfIdfVector;
        }

        double TermFrequency(string term, IList<string> tokens)
        {
            int count = tokens.Count(t => t.Equals(term, StringComparison.OrdinalIgnoreCase));
            return (double)count / tokens.Count;
        }

        double InverseDocumentFrequency(string term, List<string> documents)
        {
            int containingDocs = documents.Count(doc => _tokenizer.Tokenize(doc).Contains(term, StringComparer.OrdinalIgnoreCase));
            return containingDocs == 0 ? 0 : Math.Log((double)documents.Count / containingDocs);
        }
    }
}