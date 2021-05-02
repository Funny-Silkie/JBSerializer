using System;
using System.ComponentModel;
using System.Text.Json;

namespace JBSerializer
{
    /// <summary>
    /// <see cref="SerializeEntry"/>としてオブジェクトを変換するクラス
    /// </summary>
    [Serializable]
    internal abstract class EntryConverter : ValueConverter
    {
        protected Type Type { get; }
        /// <summary>
        /// <see cref="EntryConverter"/>の新しいインスタンスを生成する
        /// </summary>
        /// <param name="type">変換する型</param>
        /// <exception cref="ArgumentNullException"><paramref name="type"/>がnull</exception>
        protected EntryConverter(Type type)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type), "引数がnullです");
        }
        /// <summary>
        /// <see cref="SerializeEntry"/>からオブジェクトを復元する
        /// </summary>
        /// <param name="entry">使用する<see cref="SerializeEntry"/></param>
        /// <param name="serializer">使用するシリアライザ</param>
        /// <exception cref="ArgumentNullException"><paramref name="entry"/>または<paramref name="serializer"/>がnull</exception>
        /// <returns><paramref name="entry"/>をもとに復元された値</returns>
        protected abstract object FromSerializeEntry(SerializeEntry entry, BinaricJsonSerializer serializer);
        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public sealed override object ConvertBack(object value, BinaricJsonSerializer serializer)
        {
            if (value is JsonElement element)
            {
                var parser = new JsonElementParser(Type);
                return parser.Parse(element, serializer);
            }
            if (value is not SerializeEntry entry) throw new ArgumentException("エントリへの変換が出来ませんでした", nameof(value));
            return FromSerializeEntry(entry, serializer);
        }
        /// <summary>
        /// <see cref="SerializeEntry"/>に変換する
        /// </summary>
        /// <param name="value"><see cref="SerializeEntry"/>に変換する値</param>
        /// <param name="serializer">使用するシリアライザ</param>
        /// <exception cref="ArgumentNullException"><paramref name="serializer"/>がnull</exception>
        /// <returns><paramref name="value"/>の情報を格納する<see cref="SerializeEntry"/></returns>
        protected abstract SerializeEntry ToSerializeEntry(object value, BinaricJsonSerializer serializer);
        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public sealed override object Convert(object value, BinaricJsonSerializer provider) => ToSerializeEntry(value, provider);
        /// <inheritdoc/>
        public override Type GetConvertedType(Type from) => typeof(SerializeEntry);
    }
}
