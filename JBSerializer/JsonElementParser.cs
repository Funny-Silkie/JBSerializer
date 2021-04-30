using System;
using System.Text.Json;

namespace JBSerializer
{
    /// <summary>
    /// <see cref="JsonElement"/>をもとに要素を復元する<see cref="ValueConverter"/>のクラス
    /// </summary>
    [Serializable]
    public sealed class JsonElementParser
    {
        private readonly Type type;
        /// <summary>
        /// <see cref="JsonElementParser"/>の新しいインスタンスを生成する
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="type"/>がnull</exception>
        /// <param name="type">復元する要素の型</param>
        internal JsonElementParser(Type type)
        {
            this.type = type ?? throw new ArgumentNullException(nameof(type), "引数がnullです");
        }
        /// <inheritdoc/>
        public object Parse(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Null) return null;
            switch (type)
            {
                case Type t when t == typeof(sbyte): return element.GetSByte();
                case Type t when t == typeof(byte): return element.GetByte();
                case Type t when t == typeof(short): return element.GetInt16();
                case Type t when t == typeof(ushort): return element.GetUInt16();
                case Type t when t == typeof(int): return element.GetInt32();
                case Type t when t == typeof(uint): return element.GetUInt32();
                case Type t when t == typeof(long): return element.GetInt64();
                case Type t when t == typeof(ulong): return element.GetUInt64();
                case Type t when t == typeof(float): return element.GetSingle();
                case Type t when t == typeof(double): return element.GetDouble();
                case Type t when t == typeof(decimal): return element.GetDecimal();
                case Type t when t == typeof(bool): return element.GetBoolean();
                case Type t when t == typeof(char): return element.GetString()[0];
                case Type t when t == typeof(string): return element.GetString();
                case Type t when t == typeof(DateTime): return element.GetDateTime();
                case Type t when t == typeof(DateTimeOffset): return element.GetDateTimeOffset();
            }
            if (element.ValueKind == JsonValueKind.Array)
            {
                var genericType = type.GetElementType();
                var array = Array.CreateInstance(genericType, element.GetArrayLength());
                var index = 0;
                var e = element.EnumerateArray();
                var parser = new JsonElementParser(genericType);
                while (e.MoveNext()) array.SetValue(parser.Parse(e.Current), index++);
                return array;
            }
            throw new NotSupportedException();
        }
    }
}
