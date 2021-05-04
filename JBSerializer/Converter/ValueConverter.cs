using System;
using System.Runtime.Serialization;

namespace JBSerializer
{
    /// <summary>
    /// 値を変換するコンバーターのクラス
    /// </summary>
    [Serializable]
    public abstract class ValueConverter
    {
        /// <summary>
        /// メソッド呼び出しに使用する<see cref="System.Runtime.Serialization.StreamingContext"/>のインスタンス
        /// </summary>
        private protected static StreamingContext StreamingContext { get; } = new(StreamingContextStates.All);
        /// <summary>
        /// <see cref="ValueConverter"/>の新しいインスタンスを生成する
        /// </summary>
        protected ValueConverter() { }
        /// <summary>
        /// 変換可能かどうかを取得する
        /// </summary>
        /// <param name="type">判定する型</param>
        /// <exception cref="ArgumentNullException"><paramref name="type"/>が<see langword="null"/></exception>
        /// <returns><paramref name="type"/>を変換出来れば<see langword="true"/>，それ以外で<see langword="false"/></returns>
        public virtual bool CanConvert(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type), "引数がnullです");
            return ReflectionHelper.HasAttribute<SerializableAttribute>(type);
        }
        /// <summary>
        /// JSON出力するオブジェクトに変換する
        /// </summary>
        /// <param name="value">変換する値</param>
        /// <param name="serializer">使用するシリアライザ</param>
        /// <exception cref="ArgumentNullException"><paramref name="serializer"/>がnull</exception>
        /// <returns>JSON出力するオブジェクト</returns>
        public abstract object Convert(object value, BinaricJsonSerializer serializer);
        /// <summary>
        /// JSON出力するオブジェクトから元のオブジェクトに復元する
        /// </summary>
        /// <param name="value">変換する値</param>
        /// <param name="serializer">使用するシリアライザ</param>
        /// <exception cref="ArgumentNullException"><paramref name="serializer"/>がnull</exception>
        /// <returns>元のオブジェクト</returns>
        public abstract object ConvertBack(object value, BinaricJsonSerializer serializer);
        /// <summary>
        /// 変換後の型を取得する
        /// </summary>
        /// <param name="from">変換する要素の型</param>
        /// <exception cref="ArgumentNullException"><paramref name="from"/>がnull</exception>
        /// <returns><paramref name="from"/>から返還されたときに生成される要素の型</returns>
        public abstract Type GetConvertedType(Type from);
    }
}
