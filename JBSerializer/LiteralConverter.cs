using System;
using System.Runtime.Serialization;
using System.Text.Json;

namespace JBSerializer
{
    /// <summary>
    /// リテラル型を変換する<see cref="ValueConverter"/>のクラス
    /// </summary>
    /// <remarks>
    /// 対応している型：<see cref="sbyte"/>，<see cref="byte"/>，<see cref="short"/>，<see cref="ushort"/>，<see cref="int"/>，<see cref="uint"/>，<see cref="long"/>，<see cref="float"/>，<see cref="double"/>，<see cref="decimal"/>，<see cref="char"/>，<see cref="bool"/>，<see cref="DateTime"/>，<see cref="string"/>，<see cref="Array"/>
    /// </remarks>
    [Serializable]
    internal sealed class LiteralConverter : ValueConverter
    {
        private readonly Type type;
        /// <summary>
        /// <see cref="LiteralConverter"/>の新しいインスタンスを生成する
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="type"/>がnull</exception>
        /// <param name="type">変換する型</param>
        internal LiteralConverter(Type type)
        {
            this.type = type ?? throw new ArgumentNullException(nameof(type), "引数がnullです");
        }
        /// <inheritdoc/>
        public override object Convert(object value, ValueConverterProvider provider) => value;
        /// <inheritdoc/>
        public override object ConvertBack(object value, ValueConverterProvider provider)
        {
            if (value is JsonElement element)
            {
                if (element.ValueKind == JsonValueKind.Null) return null;
                if (element.ValueKind == JsonValueKind.Array)
                {
                    // Implement Array Converting
                    throw new NotImplementedException();
                }
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
                    case Type t when t == typeof(char): return element.GetString()[0];
                    case Type t when t == typeof(bool): return element.GetBoolean();
                    case Type t when t == typeof(string): return element.GetString();
                    case Type t when t == typeof(DateTime): return element.GetDateTime();
                    case Type t when t == typeof(DateTimeOffset): return element.GetDateTimeOffset();
                }
            }
            return value;
        }
        /// <inheritdoc/>
        public override Type GetConvertedType(Type from) => from ?? throw new ArgumentNullException(nameof(from), "引数がnullです");
    }
}
