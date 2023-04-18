using System;
using System.Linq;
using System.Text;
using JCore;
using PokemonApp;
using UnityEngine;
using Random = UnityEngine.Random;

namespace JasonStorey
{
    public class PaginateRandomWords : MonoBehaviour
    {
        [SerializeField]
        string[] _words;

        [SerializeField,Range(50,4000)]
        int _amountOfWordsToGenerate = 2000;

        [SerializeField,Range(5,200)]
        int _amountPerPage = 50;

        [SerializeField,Range(1,20)]
        int _currentPage = 1;

        [SerializeField]
        string[] _pageResults;
        
        void OnValidate()
        {
            if(_words.Length != _amountOfWordsToGenerate)
                GenerateWords();
            if (_amountPerPage > _words.Length) _amountPerPage = _words.Length;
            var maxPages = _words.GetPageCount(_amountPerPage);
            if (_currentPage > maxPages)
                _currentPage = maxPages;
            _pageResults = _words.Paginated(_amountPerPage, _currentPage).ToArray();
        }

        [ContextMenu("Generate Random Words")]
        void GenerateWords() =>
            _words = Enumerable.Range(0, _amountOfWordsToGenerate).Select(x => $"{x} - {SillyWordGenerator.GenerateSillyWord()}")
                .ToArray();
    }
    
    public static class SillyWordGenerator
    {
        static readonly string[] Syllables = {
            "bl", "br", "cl", "cr", "dr", "fl", "fr", "gl", "gr", "pl", "pr", "sk", "sl", "sm", "sn", "sp", "st", "sw", "tr", "tw",
            "a", "e", "i", "o", "u", "y",
            "bble", "bble", "ckle", "ddle", "fle", "ggle", "mble", "mple", "nkle", "pple", "sh", "ttle", "zzle"
        };
        
        public static string GenerateSillyWord(int minLength = 5, int maxLength = 12)
        {
            if (minLength < 1 || maxLength < 1 || minLength > maxLength)
                throw new ArgumentException("Invalid word length range");
            int wordLength = Random.Range(minLength, maxLength + 1);
            var stringBuilder = new StringBuilder(wordLength);

            while (stringBuilder.Length < wordLength)
            {
                string syllable = Syllables[Random.Range(0,Syllables.Length)];
                if (stringBuilder.Length + syllable.Length <= wordLength) stringBuilder.Append(syllable);
            }
            return stringBuilder.ToString();
        }
    }

}
