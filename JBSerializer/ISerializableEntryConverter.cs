using System;
using System.Runtime.Serialization;

namespace JBSerializer
{
    /// <summary>
    /// <see cref="ISerializable"/>を実装した要素を変換する<see cref="EntryConverter"/>のクラス
    /// </summary>
    [Serializable]
    internal sealed class ISerializableEntryConverter : EntryConverter
    {
        private readonly static StreamingContext streamingContext = new(StreamingContextStates.All);
        /// <inheritdoc/>
        public override bool CanConvert(Type type) => base.CanConvert(type) && ReflectionHelper.IsInherited<ISerializable>(type);
        /// <inheritdoc/>
        protected override object FromSerializeEntry(SerializeEntry entry, ValueConverterProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider), "引数がnullです");
            if (entry == null) throw new ArgumentNullException(nameof(entry), "引数がnullです");
            if (entry.IsNull) return null;
            var type = Type.GetType(entry.TypeName);

            // SerializationInfo, StreamingContextを引数に持つコンストラクタの取得
            var ctor = ReflectionHelper.GetConstructor(type, typeof(SerializationInfo), typeof(StreamingContext));
            if (ctor == null) throw new SerializationException("コンストラクタが発見できませんでした");

            // SerializationInfoの生成
            var siInfo = new SerializationInfo(type, new FormatterConverter());
            foreach (var (name, (typeName, value)) in entry.Fields)
            {
                var infoValue = provider.GetConverter(Type.GetType(typeName)).ConvertBack(value, provider);
                siInfo.AddValue(name, infoValue);
            }
            var result = (ISerializable)ctor.Invoke(new object[] { siInfo, streamingContext });

            // IDeserializationCallback.OnDeserialitionを実行
            if (result is IDeserializationCallback id) id.OnDeserialization(null);

            return result;
        }
        /// <inheritdoc/>
        protected override SerializeEntry ToSerializeEntry(object value, ValueConverterProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider), "引数がnullです");
            if (value == null) return SerializeEntry.Null;
            var type = value.GetType();
            var ise = value as ISerializable ?? throw new SerializationException($"{nameof(ISerializable)}への変換に失敗しました");
            var siInfo = new SerializationInfo(type, new FormatterConverter());

            ise.GetObjectData(siInfo, streamingContext);

            var result = new SerializeEntry(type);
            var enumerator = siInfo.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;
                var setValue = provider.GetConverter(current.ObjectType).Convert(current.Value, provider);
                result.Fields.Add(current.Name, (current.ObjectType.FullName, setValue));
            }

            return result;
        }
    }
}
