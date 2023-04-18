using System.Net.Http;
using NUnit.Framework;
using PokemonService;

namespace PokemonApp
{
    [TestFixture]
    public class TestApi
    {
        [Test]
        public void GetAllResources()
        {

            var poke = _api.GetPokemon(12).Result;
            int i = 0;
        }

            
        PokemonApi _api;
        [SetUp]
        public void Setup()
        {
            _api = new PokemonApi();
            _api.RequestReceived += OnRequestReceived;
        }

        void OnRequestReceived(string arg1, HttpResponseMessage arg2)
        {
        }
    }
}