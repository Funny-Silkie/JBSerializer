using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace JBSerializer
{
    /// <summary>
    /// <see cref="ITuple"/>を変換する<see cref="ValueConverter"/>のクラス
    /// </summary>
    [Serializable]
    internal sealed class TupleConverter : EntryConverter
    {
        /// <summary>
        /// <see cref="TupleConverter"/>の新しいインスタンスを生成する
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="type"/>がnull</exception>
        /// <param name="type">変換する要素の型</param>
        internal TupleConverter(Type type) : base(type) { }
        /// <inheritdoc/>
        public override bool CanConvert(Type type) => base.CanConvert(type) && ReflectionHelper.IsInherited<ITuple>(type);
        /// <inheritdoc/>
        protected override SerializeEntry ToSerializeEntry(object value, BinaricJsonSerializer serializer)
        {
            if (serializer == null) throw new ArgumentNullException(nameof(serializer), "引数がnullです");
            if (value is not ITuple tuple) throw new SerializationException("ITupleに変換出来ませんでした");
            var result = new SerializeEntry(Type);
            var fields = ReflectionHelper.GetInstanceFields(Type);
            for (int i = 0; i < fields.Length; i++)
            {
                var name = fields[i].Name;
                var field = ReflectionHelper.GetInstanceField(Type, name);
                var converter = serializer.GetConverter(field.FieldType) ?? throw new SerializationException("コンバータを取得出来ませんでした");
                var setValue = converter.Convert(tuple[i], serializer);
                result.Fields.Add(name, (ReflectionHelper.GetTypeName(field.FieldType), setValue));
            }
            return result;
        }
        /// <inheritdoc/>
        protected override object FromSerializeEntry(SerializeEntry entry, BinaricJsonSerializer serializer)
        {
            if (serializer == null) throw new ArgumentNullException(nameof(serializer), "引数がnullです");
            var values = entry.Fields.OrderBy(x => x.Value.TypeName);
            var paramTypes = values.Select(x => Type.GetType(x.Value.TypeName)).ToArray();
            var paramArray = new object[paramTypes.Length];
            var index = 0;
            foreach (var (fieldName, (typeName, value)) in values)
            {
                var fieldType = Type.GetType(typeName);
                var field = ReflectionHelper.GetInstanceField(fieldType, fieldName);
                var converter = serializer.GetConverter(fieldType) ?? throw new SerializationException("コンバータを取得出来ませんでした");
                var getValue = converter.ConvertBack(value, serializer);
                paramArray[index++] = getValue;
            }
            return ReflectionHelper.GetConstructor(Type, paramTypes).Invoke(paramArray);
        }
    }
}
