using System;
using System.Runtime.Serialization;

namespace JBSerializer
{
    /// <summary>
    /// <see cref="Type"/>を変換する<see cref="ValueConverter"/>のクラス
    /// </summary>
    internal sealed class TypeConverter : ValueConverter
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type type) => type == typeof(Type);
        /// <inheritdoc/>
        public override object Convert(object value, BinaricJsonSerializer serializer)
        {
            if (serializer == null) throw new ArgumentNullException(nameof(serializer), "引数がnullです");
            if (value == null) return null;
            return ((Type)value).FullName;
        }
        /// <inheritdoc/>
        public override object ConvertBack(object value, BinaricJsonSerializer serializer)
        {
            if (serializer == null) throw new ArgumentNullException(nameof(serializer), "引数がnullです");
            if (value == null) return null;
            if (value is not string typeName) throw new SerializationException("文字列に変換できませんでした");
            return Type.GetType(typeName);
        }
        /// <inheritdoc/>
        public override Type GetConvertedType(Type from) => typeof(string);
    }
}
