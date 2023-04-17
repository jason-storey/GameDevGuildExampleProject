using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
namespace JCore.Tests
{
    [TestFixture]
    public class EnumerableExtensions_Should
    {
        [Test]
        public void When_collection_is_empty_return_empty() =>
            Enumerable.Empty<string>().Paginated(5, 10).Should().BeEmpty();

        [Test]
        public void When_collection_is_null_return_empty()
        {
            IEnumerable<string> collection = null;
            collection.Paginated(1, 1).Should().BeEmpty();
        }

        [Test]
        public void When_pageSize_is_less_than_zero_use_default()
        {
            var results = Enumerable.Range(0, 100).Paginated(-50);
            results.Count().Should().Be(EnumerableExtensions.DEFAULT_PAGE_SIZE);
        }

        [Test]
        public void When_page_is_negative_return_all()
        {
            var results = Enumerable.Range(0, 100).Paginated(20, -3);
            results.Count().Should().Be(100);
        }

        [Test]
        public void When_pageSize_is_greater_than_collection_return_all()
        {
            var results = Enumerable.Range(0, 100).Paginated(1000, -3);
            results.Count().Should().Be(100);
        }
        
    }
}