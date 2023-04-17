using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using JCore.Collections;
using Newtonsoft.Json;
using NUnit.Framework;
namespace JCore.Tests
{
    [TestFixture]
    public class InvertedIndex_Should
    {
        [SetUp]
        public void Setup()
        {
            _index = new InvertedIndex<string>();
            _settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
            _settings.Converters.Add(new InvertedIndex<string>.JsonConverter());
        }

        [Test]
        public void When_created_has_no_elements()
        {
            _index.ValueCount.Should().Be(0);
            _index.KeyCount.Should().Be(0);
            _index.Keys.Should().BeEmpty();
            _index.Values.Should().BeEmpty();
            _index.IsDirty.Should().BeFalse();
        }

        [Test]
        public void When_adding_element_mark_as_dirty()
        {
            _index.Add("Jason", "Awesome","Cool","Dude");
            _index.ValueCount.Should().Be(1);
            _index.KeyCount.Should().Be(3);
            _index.Keys.Count().Should().Be(3);
            _index.Values.Count().Should().Be(1);
            _index.IsDirty.Should().BeTrue();
        }

        [Test]
        public void When_indexing_empty_return_empty() => _index["Awesome"].Should().BeEmpty();

        [Test]
        public void When_indexing_with_matching_entry_return_matches()
        {
            _index.Add("Jason","programmer","dude","brunette");
            _index.Add("Sarah","artist","dude","blonde");

            _index["programmer"].Should().HaveCount(1);
            _index["dude"].Should().HaveCount(2);
            _index["artist"].Should().HaveCount(1);
        }

        [Test]
        public void FindByAnyTag_ReturnsCorrectResults()
        {
            SetupTestData();
            var result = _index.FindByAnyTag("tag2", "tag4");
            result.Should().BeEquivalentTo(new List<string> { "Item1", "Item2", "Item3", "Item4" });
        }
        
        
        
        
        [Test]
        public void When_withAllOf_returns_correct_items()
        {
            _index.Add("Jason","programmer","dude","brunette");
            _index.Add("Sarah","artist","dude","blonde");
            _index.Add("Alien","space","monoclops");

            _index.WithAllOf("Programmer", "DUDe").Count().Should().Be(1);
            _index.WithAllOf("dude").Count().Should().Be(2);
            _index.WithAllOf("alien", "artist").Should().BeEmpty();
        }

        [Test]
        public void When_querying_include_returns_correctly()
        {
            SetupTestData();
            
            var result = _index.Query("tag2 OR tag4");
            result.Should().BeEquivalentTo(new List<string> { "Item1", "Item2", "Item3", "Item4" });
        }

        [Test]
        public void When_querying_with_empty_returns_empty()
        {
            SetupTestData();
            var results = _index.Query("");
            results.Count().Should().Be(0);
        }

        void SetupTestData()
        {
            _index.Add("Item1", "tag1", "tag2");
            _index.Add("Item2", "tag2");
            _index.Add("Item3", "tag3", "tag4");
            _index.Add("Item4", "tag4");
        }

        [Test]
        public void When_querying_with_exclusion_and_include_returns()
        {
            SetupTestData();
            var result = _index.Query("tag2 AND NOT tag1");
            result.Should().BeEquivalentTo(new List<string> { "Item2" });
        }
        
        [Test]
        public void When_serializing_should_serialize_correctly()
        {
            _index.Add("Apple","Fruit","Red");
            _index.Add("Banana","Fruit","Yellow");
            _index.Add("Blueberry","Fruit","Blue");

            var json = JsonConvert.SerializeObject(_index,_settings);
            var restored = JsonConvert.DeserializeObject<InvertedIndex<string>>(json,_settings);
            restored.ValueCount.Should().Be(_index.ValueCount);
            restored.Keys.Count().Should().Be(_index.Keys.Count());
            restored.Values.Count().Should().Be(_index.Values.Count());
            restored.IsDirty.Should().BeFalse();
        }

        [Test]
        public void When_adding_a_blank_label_do_not_add()
        {
            _index.Add("Apple","Fruit",string.Empty);
            _index.ValueCount.Should().Be(1);
        }
        
        [Test]
        public void When_adding_with_dontDirty_flag_does_not_mark_dirty()
        {
            _index.Add("Apple",true,"Fruit","Nuts");
            _index.IsDirty.Should().BeFalse();
        }

        
        [Test]
        public void When_foreaching_returns_values()
        {
            _index.Add("Apple","Fruit","Red");
            _index.Add("Banana","Fruit","Yellow");
            _index.Add("Blueberry","Fruit","Blue");

            var expected = new List<string> { "Apple", "Banana", "Blueberry" };
            var elements = _index.ToList();

            elements.Should().BeEquivalentTo(expected);
            var loopedElements = new List<string>();
            foreach (var element in _index) 
                loopedElements.Add(element);
            loopedElements.Should().BeEquivalentTo(expected);
        }
        
        JsonSerializerSettings _settings;
        InvertedIndex<string> _index;
    }
}