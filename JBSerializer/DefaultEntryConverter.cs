using System;
using System.Runtime.Serialization;

namespace JBSerializer
{
    /// <summary>
    /// デフォルトの<see cref="EntryConverter"/>のクラス
    /// </summary>
    [Serializable]
    internal sealed class DefaultEntryConverter : EntryConverter
    {
        private readonly static StreamingContext streamingContext = new(StreamingContextStates.All);
        /// <summary>
        /// 変換する要素の型を取得する
        /// </summary>
        public Type Type { get; }
        internal DefaultEntryConverter(Type type)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type), "引数がnullです");
        }
        /// <inheritdoc/>
        protected override object FromSerializeEntry(SerializeEntry entry, ValueConverterProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider), "引数がnullです");
            if (entry == null) throw new ArgumentNullException(nameof(entry), "引数がnullです");
            if (entry.IsNull) return null;

            var type = Type.GetType(entry.TypeName);
            var result = ReflectionHelper.GetEmptyConstructor(type);
            var methods = ReflectionHelper.GetInstanceMethods(type);

            // OnDeserializing実装メソッドを実行
            for (int i = 0; i < methods.Length; i++)
            {
                if (!ReflectionHelper.HasAttribute<OnDeserializingAttribute>(methods[i])) continue;
                var parameters = methods[i].GetParameters();
                if (parameters.Length != 1 || parameters[0].ParameterType != typeof(StreamingContext)) continue;
                methods[i].Invoke(result, new object[] { streamingContext });
            }

            // フィールドの復元
            foreach (var (fieldName, value) in entry.Fields)
            {
                var fieldInfo = ReflectionHelper.GetInstanceField(type, fieldName);
                if (fieldInfo == null) throw new SerializationException("フィールドの復元に失敗しました");
                var setValue = provider.GetConverter(fieldInfo.FieldType).ConvertBack(value, provider);
                fieldInfo.SetValue(result, setValue);
            }

            // OnDeserialized実装メソッドを実行
            for (int i = 0; i < methods.Length; i++)
            {
                if (!ReflectionHelper.HasAttribute<OnDeserializedAttribute>(methods[i])) continue;
                var parameters = methods[i].GetParameters();
                if (parameters.Length != 1 || parameters[0].ParameterType != typeof(StreamingContext)) continue;
                methods[i].Invoke(result, new object[] { streamingContext });
            }

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
            var methods = ReflectionHelper.GetInstanceMethods(type);

            var result = new SerializeEntry(type);

            // OnSerializing実装メソッドを実行
            for (int i = 0; i < methods.Length; i++)
            {
                if (!ReflectionHelper.HasAttribute<OnSerializingAttribute>(methods[i])) continue;
                var parameters = methods[i].GetParameters();
                if (parameters.Length != 1 || parameters[0].ParameterType != typeof(StreamingContext)) continue;
                methods[i].Invoke(value, new object[] { streamingContext });
            }

            // フィールドの保存
            var fields = ReflectionHelper.GetInstanceFields(type);
            for (int i = 0; i < fields.Length; i++)
            {
                var savedValue = provider.GetConverter(fields[i].FieldType).Convert(fields[i].GetValue(value), provider);
                result.Fields.Add(fields[i].Name, savedValue);
            }

            // OnSerialized実装メソッドを実行
            for (int i = 0; i < methods.Length; i++)
            {
                if (!ReflectionHelper.HasAttribute<OnSerializedAttribute>(methods[i])) continue;
                var parameters = methods[i].GetParameters();
                if (parameters.Length != 1 || parameters[0].ParameterType != typeof(StreamingContext)) continue;
                methods[i].Invoke(value, new object[] { streamingContext });
            }

            return result;
        }
    }
}
