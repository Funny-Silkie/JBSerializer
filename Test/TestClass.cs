using NUnit.Framework;
using JBSerializer;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Test
{
    [TestFixture]
    public class TestClass
    {
        const string ResultDirectory = "Result";
        private static T Serialize<T>(T value, string name)
        {
            Directory.CreateDirectory(ResultDirectory);
            var serializer = new BinaricJsonSerializer();
            var json = serializer.Serialize(value);
            using var writer = new StreamWriter(Path.Combine(ResultDirectory, $"{name}.json"), false, Encoding.UTF8) { AutoFlush = true };
            writer.WriteLine(json);
            return serializer.Deserialize<T>(json);
        }
        [Test]
        public void SerializeTestInt()
        {
            var value = 1;
            var ds = Serialize(value, "int");
            Assert.AreEqual(value, ds);
        }
        [Test]
        public void SerializeTestString()
        {
            var value = "HogeHogeあいう";
            var ds = Serialize(value, "string");
            Assert.AreEqual(value, ds);
        }
        [Test]
        public void SerializeTestType()
        {
            var value = typeof(int);
            var ds = Serialize(value, "type");
            Assert.AreEqual(value, ds);
        }
        [Test]
        public void SerializeTestArray()
        {
            var value = new[] { false, false, true };
            var ds = Serialize(value, "array");
            Assert.AreEqual(value, ds);
        }
        [Test]
        public void SerializeTestJagArray()
        {
            var value = new int[][] { new[] { 1, 2 }, new[] { 3, 4 } };
            var ds = Serialize(value, "2array");
            Assert.AreEqual(value, ds);
        }
        [Test]
        public void SerializeTestList()
        {
            var value = new List<char>() { 'a', 'b', 'c' };
            var ds = Serialize(value, "list");
            Assert.AreEqual(value, ds);
        }
        [Test]
        public void SerializeTestNullable ()
        {
            int? value = 3;
            var ds = Serialize(value, "nullable");
            Assert.AreEqual(value, ds);
        }
        [Test]
        public void SerializeTestList2()
        {
            var value = new List<ISerializableEntry>();
            for (int i = 0; i < 5; i++) value.Add(new ISerializableEntry());
            var ds = Serialize(value, "list2");
            Assert.AreEqual(value, ds);
        }
        [Test]
        public void SerializeStruct()
        {
            var value = new Struct(3);
            var ds = Serialize(value, "struct");
            Assert.AreEqual(value, ds);
        }
        [Test]
        public void SerializeTestTuple()
        {
            var value = (1, "ABC");
            var ds = Serialize(value, "tuple");
            Assert.AreEqual(value, ds);
        }
        [Test]
        public void SerializeTestSerializableEntry()
        {
            var value = new SerializableEntry();
            var ds = Serialize(value, "serializable");
            Assert.AreEqual(value, ds);
        }
        [Test]
        public void SerializeTestISerializableEntry()
        {
            var value = new ISerializableEntry();
            var ds = Serialize(value, "iserializable");
            Assert.AreEqual(value, ds);
        }
    }
}
