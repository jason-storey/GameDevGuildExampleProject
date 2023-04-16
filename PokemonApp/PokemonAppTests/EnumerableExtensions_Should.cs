using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using PokemonApp;
namespace PokemonAppTests
{
    [TestFixture]
    public class EnumerableExtensions_Should
    {
        [Test]
        public void When_collection_is_empty_return_empty() => 
            Enumerable.Empty<string>().Paginated(5, 10).Should().BeEmpty();
    }
}