using System;
using System.Runtime.Serialization;

namespace JBSerializer
{
    /// <summary>
    /// <see cref="ValueConverter"/>のデフォルト実装
    /// </summary>
    [Serializable]
    internal sealed class DefaultValueConverterProvider : ValueConverterProvider
    {
        private static readonly Type[] LiteralTypes = new[]
        {
            typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong),
            typeof(float), typeof(double), typeof(decimal), typeof(char), typeof(string), typeof(bool), typeof(DateTime)
        };
        /// <inheritdoc/>
        public override ValueConverter GetConverter(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type), "引数がnullです");
            if (type.IsArray || type == typeof(ArraySerializeEntry)) return new ArrayConverter();
            if (type == typeof(Type)) return new TypeConverter();
            if (!ReflectionHelper.HasAttribute<SerializableAttribute>(type)) return null;
            return type switch
            {
                Type t when Array.IndexOf(LiteralTypes, t) >= 0 => new LiteralConverter(type),
                Type t when ReflectionHelper.IsInherited<ISerializable>(t) => new ISerializableEntryConverter(type),
                _ => new DefaultEntryConverter(type)
            };
        }
    }
}
