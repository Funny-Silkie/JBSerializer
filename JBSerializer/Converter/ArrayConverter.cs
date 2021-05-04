using System;
using System.Runtime.Serialization;
using System.Text.Json;

namespace JBSerializer
{
    /// <summary>
    /// 配列を変換する<see cref="ValueConverter"/>のクラス
    /// </summary>
    [Serializable]
    internal sealed class ArrayConverter : ValueConverter
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type), "引数がnullです");
            return type.IsArray;
        }
        /// <inheritdoc/>
        public override object Convert(object value, BinaricJsonSerializer serializer)
        {
            if (serializer == null) throw new ArgumentNullException(nameof(serializer), "引数がnullです");
            if (value == null) return ArraySerializeEntry.Null;
            if (value is not Array array) throw new SerializationException("配列に変換出来ませんでした");
            var result = new ArraySerializeEntry(array.GetType().GetElementType(), array.Length);
            for (int i = 0; i < array.Length; i++)
            {
                var current = array.GetValue(i);
                if (current == null)
                {
                    result.Items[i] = null;
                    continue;
                }
                var converter = serializer.GetConverter(current.GetType()) ?? throw new SerializationException("コンバータを取得出来ませんでした");
                var setValue = converter.Convert(current, serializer);
                result.Items[i] = (ReflectionHelper.GetTypeName(current.GetType()), setValue);
            }
            return result;
        }
        /// <inheritdoc/>
        public override object ConvertBack(object value, BinaricJsonSerializer serializer)
        {
            if (serializer == null) throw new ArgumentNullException(nameof(serializer), "引数がnullです");
            if (value is JsonElement __element)
            {
                var parser = new JsonElementParser(typeof(ArraySerializeEntry));
                var v = parser.Parse(__element, serializer);
                return v;
            }
            //if (value is Array __array) return __array;
            if (value is not ArraySerializeEntry entry) throw new SerializationException("エントリーに変換出来ませんでした");
            if (entry.IsNull) return null;
            var result = Array.CreateInstance(Type.GetType(entry.ItemTypeName), entry.Items.Length);
            for (int i = 0; i < entry.Items.Length; i++)
            {
                if (entry.Items[i] == null)
                {
                    result.SetValue(null, i);
                    continue;
                }
                var (name, current) = entry.Items[i];
                var valueType = Type.GetType(name);
                if (current == null)
                {
                    result.SetValue(null, i);
                    continue;
                }
                if (current is JsonElement element)
                {
                    var parser = new JsonElementParser(valueType);
                    result.SetValue(parser.Parse(element, serializer), i);
                    continue;
                }
                var converter = serializer.GetConverter(valueType) ?? throw new SerializationException("コンバータを取得出来ませんでした");
                result.SetValue(converter.ConvertBack(current, serializer), i);
            }
            return result;
        }
        /// <inheritdoc/>
        public override Type GetConvertedType(Type from) => typeof(ArraySerializeEntry);
    }
}
