using System.Collections.Generic;
using FluentAssertions;
using JCore.Collections;
using JCore.Search;
using NSubstitute;
using NUnit.Framework;

namespace JCore.Tests
{
    [TestFixture]
    public class Index_Should
    {
        Index<string> _index;
        Dictionary<string, string> _dictionary;
        ISearchAlgorithm _search;
        [SetUp]
        public void SetUp()
        {
            _search = Substitute.For<ISearchAlgorithm>();
            _dictionary = new Dictionary<string, string>
            {
                { "key1", "value1" },
                { "key2", "value2" },
                { "key3", "value3" }
            };

            _index = Index<string>.From(_dictionary);
        }

        [TestCaseSource(nameof(AddTestCaseData))]
        public void AddTests(Dictionary<string, string> dictionary, string newItem, string expectedKey)
        {
            var index = new Index<string>(dictionary) { { newItem, x=> expectedKey } };
            index[newItem].Should().Be(expectedKey);
        }

        public static IEnumerable<TestCaseData> AddTestCaseData
        {
            get
            {
                yield return new TestCaseData(new Dictionary<string, string> { { "key1", "value1" } }, "value2", "value2");
                yield return new TestCaseData(new Dictionary<string, string> { { "key1", "value1" } }, "value1", "value1");
            }
        }

        [TestCaseSource(nameof(SearchTestCaseData))]
        public void SearchTests(string searchTerm, bool caseSensitive, IEnumerable<string> expectedResults)
        {
            _search.IsMatch(Arg.Any<string>(), Arg.Any<string>())
                .ReturnsForAnyArgs(x => true);
            var index = Index<string>.From(_dictionary.Values, x => x, caseSensitive);
           
            var results = index.Search(searchTerm, _search);
            results.Should().BeEquivalentTo(expectedResults);
        }

        public static IEnumerable<TestCaseData> SearchTestCaseData
        {
            get
            {
                yield return new TestCaseData("value1", false, new List<string> { "value1","value2","value3" });
                yield return new TestCaseData("VALUE1", false, new List<string> { "value1","value2","value3" });
                yield return new TestCaseData("VALUE1", true, new List<string> { "value1","value2","value3" });
            }
        }

        [Test]
        public void AddAllTests()
        {
            var index = new Index<string>(new Dictionary<string, string>());
            index.AddAll(_dictionary);
            index.GetAll().Should().BeEquivalentTo(_dictionary.Values);
        }

        [TestCase("key1", true)]
        [TestCase("key4", false)]
        public void HasTests(string key, bool expected)
        {
            _index.Has(key).Should().Be(expected);
        }

        [Test]
        public void KeysTest()
        {
            _index.Keys.Should().BeEquivalentTo(_dictionary.Keys);
        }

        [TestCase("key1", "value1")]
        [TestCase("key4", null)]
        public void IndexerTests(string key, string expectedValue)
        {
            _index[key].Should().Be(expectedValue);
        }

        [TestCase("key1", "value1")]
        [TestCase("key4", null)]
        public void GetByIdTests(string key, string expectedValue)
        {
            _index.GetById(key).Should().Be(expectedValue);
        }

        [Test]
        public void GetAllTests()
        {
            _index.GetAll().Should().BeEquivalentTo(_dictionary.Values);
        }

        [Test]
        public void UpdateTests()
        {
            _index.Update("key1", "newValue");
            _index["key1"].Should().Be("newValue");
        }
    }
}