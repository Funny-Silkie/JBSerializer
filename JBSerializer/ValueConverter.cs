using System;

namespace JBSerializer
{
    /// <summary>
    /// 値を変換するコンバーターのクラス
    /// </summary>
    [Serializable]
    public abstract class ValueConverter
    {
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
        /// <param name="provider"><see cref="ValueConverter"/>を与えるプロバイダ</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/>がnull</exception>
        /// <returns>JSON出力するオブジェクト</returns>
        public abstract object Convert(object value, ValueConverterProvider provider);
        /// <summary>
        /// JSON出力するオブジェクトから元のオブジェクトに復元する
        /// </summary>
        /// <param name="value">変換する値</param>
        /// <param name="provider"><see cref="ValueConverter"/>を与えるプロバイダ</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/>がnull</exception>
        /// <returns>元のオブジェクト</returns>
        public abstract object ConvertBack(object value, ValueConverterProvider provider);
    }
}
