using System;

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
        /// <inheritdoc/>
        public override object Convert(object value, ValueConverterProvider provider) => value;
        /// <inheritdoc/>
        public override object ConvertBack(object value, ValueConverterProvider provider) => value;
        /// <inheritdoc/>
        public override Type GetConvertedType(Type from) => from ?? throw new ArgumentNullException(nameof(from), "引数がnullです");
    }
}
