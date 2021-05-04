using System;
using System.Text.Json;

namespace JBSerializer
{
    /// <summary>
    /// リテラル型を変換する<see cref="ValueConverter"/>のクラス
    /// </summary>
    /// <remarks>
    /// 対応している型：<see cref="sbyte"/>，<see cref="byte"/>，<see cref="short"/>，<see cref="ushort"/>，<see cref="int"/>，<see cref="uint"/>，<see cref="long"/>，<see cref="float"/>，<see cref="double"/>，<see cref="decimal"/>，<see cref="char"/>，<see cref="bool"/>，<see cref="DateTime"/>，<see cref="string"/>
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
        public override object Convert(object value, BinaricJsonSerializer serializer) => value;
        /// <inheritdoc/>
        public override object ConvertBack(object value, BinaricJsonSerializer serializer)
        {
            if (value is JsonElement element)
            {
                var parser = new JsonElementParser(type);
                return parser.Parse(element, serializer);
            }
            return value;
        }
        /// <inheritdoc/>
        public override Type GetConvertedType(Type from) => from ?? throw new ArgumentNullException(nameof(from), "引数がnullです");
    }
}
