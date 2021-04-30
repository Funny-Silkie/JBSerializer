﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace JBSerializer
{
    /// <summary>
    /// <see cref="SerializeEntry"/>としてオブジェクトを変換するクラス
    /// </summary>
    [Serializable]
    internal abstract class EntryConverter : ValueConverter
    {
        /// <summary>
        /// <see cref="EntryConverter"/>の新しいインスタンスを生成する
        /// </summary>
        protected EntryConverter() { }
        /// <summary>
        /// <see cref="SerializeEntry"/>からオブジェクトを復元する
        /// </summary>
        /// <param name="dictionary">使用する<see cref="SerializeEntry"/></param>
        /// <param name="provider"><see cref="ValueConverter"/>を与えるプロバイダ</param>
        /// <exception cref="ArgumentNullException"><paramref name="dictionary"/>または<paramref name="provider"/>がnull</exception>
        /// <returns><paramref name="dictionary"/>をもとに復元された値</returns>
        protected abstract object FromSerializeEntry(SerializeEntry dictionary, ValueConverterProvider provider);
        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public sealed override object ConvertBack(object value, ValueConverterProvider provider)
        {
            if (value is not SerializeEntry entry) throw new ArgumentException("エントリへの変換が出来ませんでした", nameof(value));
            return FromSerializeEntry(entry, provider);
        }
        /// <summary>
        /// <see cref="SerializeEntry"/>に変換する
        /// </summary>
        /// <param name="value"><see cref="SerializeEntry"/>に変換する値</param>
        /// <param name="provider"><see cref="ValueConverter"/>を与えるプロバイダ</param>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/>がnull</exception>
        /// <returns><paramref name="value"/>の情報を格納する<see cref="SerializeEntry"/></returns>
        protected abstract Dictionary<string, object> ToSerializeEntry(object value, ValueConverterProvider provider);
        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public sealed override object Convert(object value, ValueConverterProvider provider) => ToSerializeEntry(value, provider);
    }
}