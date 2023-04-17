#region

using System.Collections.Generic;
using FluentAssertions;
using JCore.Collections;
using NUnit.Framework;

#endregion

namespace JCore.Tests
{
    [TestFixture]
    public class HashedListDictionary_Should
    {
        [SetUp]
        public void Setup() => _dictionary = new HashedListDictionary<string>();

        HashedListDictionary<string> _dictionary;

        [Test]
        public void When_Getting_Values_For_Existing_Key_Returns_Correct_Values()
        {
            var key = "key";
            var values = new List<string> { "value1", "value2" };
            _dictionary.SetElements(key, values);
            _dictionary.GetValuesById(key).Should().BeEquivalentTo(values);
        }

   
        [Test]
        public void When_Key_Exists_Deletes_Elements_And_Removes_Key_Returns_True()
        {
            var key = "key1";
            _dictionary.DeleteAll(key).Should().BeTrue();
            _dictionary.GetValuesById(key).Should().BeEmpty();
            _dictionary.GetElementCount(key).Should().Be(0);
        }

        
        [Test]
        public void When_Key_Does_Not_Exist_Returns_False()
            => _dictionary.DeleteAll("key3").Should().BeFalse();
        
        [Test]
        public void When_Getting_Values_For_Non_Existent_Key_Returns_Empty_List() => 
            _dictionary.GetValuesById("key").Should().BeEmpty();

        [Test]
        public void When_Adding_Element_To_Existing_Key_Returns_True() =>
            _dictionary.AddElement("key", "value").Should().BeTrue();

        [Test]
        public void When_Adding_Element_To_Non_Existent_Key_Returns_True() => 
            _dictionary.AddElement("key", "value").Should().BeTrue();

        [Test]
        public void When_Removing_Existing_Element_From_Key_Returns_True()
        {
            var key = "key";
            var value = "value";
            _dictionary.AddElement(key, value);
            _dictionary.RemoveElement(key, value).Should().BeTrue();
        }

        [Test]
        public void When_Removing_Non_Existent_Element_From_Key_Returns_False()
        {
            _dictionary.RemoveElement("key", "value").Should().BeFalse();
        }

        [Test]
        public void When_Updating_Existing_Key_Returns_True()
        {
            var key = "key";
            var value = "value";
            _dictionary.AddElement(key, value);
            _dictionary.Update(key, "updatedValue").Should().BeTrue();
        }

        [Test]
        public void When_Updating_Non_Existent_Key_Returns_False()
        {
            _dictionary.Update("key", "value").Should().BeFalse();
        }

        [Test]
        public void When_Deleting_Existing_Element_Returns_True()
        {
            var key = "key";
            var value = "value";
            _dictionary.AddElement(key, value);
            _dictionary.Delete(key, value).Should().BeTrue();
        }

        [Test]
        public void When_Deleting_Non_Existent_Element_Returns_False()
        {
            _dictionary.Delete("key", "value").Should().BeFalse();
        }
    }
}