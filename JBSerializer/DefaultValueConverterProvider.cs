﻿using System;
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
            typeof(float), typeof(double), typeof(decimal), typeof(char), typeof(string), typeof(string), typeof(DateTime)
        };
        /// <inheritdoc/>
        public override ValueConverter GetConverter(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type), "引数がnullです");
            if (!ReflectionHelper.HasAttribute<SerializableAttribute>(type)) return null;
            return type switch
            {
                Type t when Array.IndexOf(LiteralTypes, t) >= 0 || t.IsArray => new LiteralConverter(),
                Type t when ReflectionHelper.IsInherited<ISerializable>(t) => new ISerializableEntryConverter(),
                Type t when t == typeof(Type) => new TypeConverter(),
                _ => new DefaultEntryConverter(type)
            };
        }
    }
}
