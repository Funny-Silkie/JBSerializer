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
        /// <summary>
        /// <see cref="JsonElement"/>を復元する
        /// </summary>
        /// <param name="element">復元する<see cref="JsonElement"/>のインスタンス</param>
        /// <param name="serializer">使用するシリアライザ</param>
        /// <exception cref="ArgumentNullException"><paramref name="serializer"/>がnull</exception>
        /// <returns><paramref name="element"/>を復元したオブジェクト</returns>
        public object Parse(JsonElement element, BinaricJsonSerializer serializer)
        {
            if (serializer == null) throw new ArgumentNullException(nameof(serializer), "引数がnullです");
            if (element.ValueKind == JsonValueKind.Null) return null;
            if (element.ValueKind == JsonValueKind.Array)
            {
                var genericType = type.GetElementType();
                var array = Array.CreateInstance(genericType, element.GetArrayLength());
                var index = 0;
                var e = element.EnumerateArray();
                var parser = new JsonElementParser(genericType);
                while (e.MoveNext()) array.SetValue(parser.Parse(e.Current, serializer), index++);
                return array;
            }
            return serializer.Deserialize(element.GetRawText(), type);
        }
    }
}
