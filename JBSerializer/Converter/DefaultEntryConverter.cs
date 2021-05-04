using System;
using System.Runtime.Serialization;

namespace JBSerializer
{
    /// <summary>
    /// デフォルトの<see cref="EntryConverter"/>のクラス
    /// </summary>
    [Serializable]
    internal class DefaultEntryConverter : EntryConverter
    {
        /// <summary>
        /// <see cref="DefaultEntryConverter"/>の新しいインスタンスを生成する
        /// </summary>
        /// <param name="type">復元する要素の型</param>
        /// <exception cref="ArgumentNullException"><paramref name="type"/>がnull</exception>
        internal DefaultEntryConverter(Type type) : base(type) { }
        /// <inheritdoc/>
        protected override object FromSerializeEntry(SerializeEntry entry, BinaricJsonSerializer serializer)
        {
            if (serializer == null) throw new ArgumentNullException(nameof(serializer), "引数がnullです");
            if (entry == null) throw new ArgumentNullException(nameof(entry), "引数がnullです");
            if (entry.IsNull) return null;

            var type = Type.GetType(entry.TypeName);
            var result = FormatterServices.GetUninitializedObject(type);
            var methods = ReflectionHelper.GetInstanceMethods(type);

            // OnDeserializing実装メソッドを実行
            for (int i = 0; i < methods.Length; i++)
            {
                if (!ReflectionHelper.HasAttribute<OnDeserializingAttribute>(methods[i])) continue;
                var parameters = methods[i].GetParameters();
                if (parameters.Length != 1 || parameters[0].ParameterType != typeof(StreamingContext)) continue;
                methods[i].Invoke(result, new object[] { StreamingContext });
            }

            // フィールドの復元
            foreach (var (fieldName, (typeName, value)) in entry.Fields)
            {
                var fieldInfo = ReflectionHelper.GetInstanceField(type, fieldName);
                if (fieldInfo == null) throw new SerializationException("フィールドの復元に失敗しました");
                var converter = serializer.GetConverter(Type.GetType(typeName)) ?? throw new SerializationException("コンバータの取得に失敗しました");
                var setValue = converter.ConvertBack(value, serializer);
                fieldInfo.SetValue(result, setValue);
            }

            // OnDeserialized実装メソッドを実行
            for (int i = 0; i < methods.Length; i++)
            {
                if (!ReflectionHelper.HasAttribute<OnDeserializedAttribute>(methods[i])) continue;
                var parameters = methods[i].GetParameters();
                if (parameters.Length != 1 || parameters[0].ParameterType != typeof(StreamingContext)) continue;
                methods[i].Invoke(result, new object[] { StreamingContext });
            }

            // IDeserializationCallback.OnDeserialitionを実行
            if (result is IDeserializationCallback id) id.OnDeserialization(null);

            return result;
        }
        /// <inheritdoc/>
        protected override SerializeEntry ToSerializeEntry(object value, BinaricJsonSerializer serializer)
        {
            if (serializer == null) throw new ArgumentNullException(nameof(serializer), "引数がnullです");
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
                methods[i].Invoke(value, new object[] { StreamingContext });
            }

            // フィールドの保存
            var fields = ReflectionHelper.GetInstanceFields(type);
            for (int i = 0; i < fields.Length; i++)
            {
                if (ReflectionHelper.HasAttribute<NonSerializedAttribute>(fields[i])) continue;
                var converter = serializer.GetConverter(fields[i].FieldType);
                var savedValue = converter.Convert(fields[i].GetValue(value), serializer);
                result.Fields.Add(fields[i].Name, (ReflectionHelper.GetTypeName(fields[i].FieldType), savedValue));
            }

            // OnSerialized実装メソッドを実行
            for (int i = 0; i < methods.Length; i++)
            {
                if (!ReflectionHelper.HasAttribute<OnSerializedAttribute>(methods[i])) continue;
                var parameters = methods[i].GetParameters();
                if (parameters.Length != 1 || parameters[0].ParameterType != typeof(StreamingContext)) continue;
                methods[i].Invoke(value, new object[] { StreamingContext });
            }

            return result;
        }
    }
}
