using System;

namespace JBSerializer
{
    /// <summary>
    /// <see cref="ValueConverter"/>を提供するクラス
    /// </summary>
    [Serializable]
    public abstract class ValueConverterProvider
    {
        /// <summary>
        /// <see cref="ValueConverterProvider"/>の新しいインスタンスを生成する
        /// </summary>
        protected ValueConverterProvider() { }
        /// <summary>
        /// 指定した型に使用する<see cref="ValueConverter"/>を取得する
        /// </summary>
        /// <param name="type">取得する要素の型</param>
        /// <exception cref="ArgumentNullException"><paramref name="type"/>がnull</exception>
        /// <returns><paramref name="type"/>に対応する<see cref="ValueConverter"/>のインスタンス　無かったらnull</returns>
        public abstract ValueConverter GetConverter(Type type);
    }
}
