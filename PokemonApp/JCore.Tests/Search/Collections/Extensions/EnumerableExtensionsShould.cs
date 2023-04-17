using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
namespace JCore.Tests
{
    [TestFixture]
    public class EnumerableExtensionsShould
    {
        public class GetPageCountTests
        {
            IList<int> _items;

            [SetUp]
            public void Setup() => _items = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            [Test]
            public void When_Amount_Per_Page_Is_Larger_Than_Item_Count_Returns_One()
                => _items.GetPageCount(20).Should().Be(1);

            [Test]
            public void When_Amount_Per_Page_Is_Equal_To_Item_Count_Returns_One()
                => _items.GetPageCount(10).Should().Be(1);

            [Test]
            public void When_Amount_Per_Page_Is_Smaller_Than_Item_Count_Returns_Page_Count()
                => _items.GetPageCount(3).Should().Be(4);
        }
    }
}