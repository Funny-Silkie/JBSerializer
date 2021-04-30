using System;
using System.Collections.Generic;

namespace JBSerializer
{
    /// <summary>
    /// シリアライズに使用されるエントリー
    /// </summary>
    [Serializable]
    internal sealed class SerializeEntry
    {
        /// <summary>
        /// nullを表すインスタンスを取得する
        /// </summary>
        public static SerializeEntry Null => new() { IsNull = true };
        /// <summary>
        /// 値がnullかどうかを取得する
        /// </summary>
        public bool IsNull { get; internal set; }
        /// <summary>
        /// 型名を取得する
        /// </summary>
        public string TypeName { get; internal set; }
        /// <summary>
        /// フィールドを取得する
        /// </summary>
        public Dictionary<string, (string typeName, object value)> Fields { get; set; }
        /// <summary>
        /// <see cref="SerializeEntry"/>の新しいインスタンスを生成する
        /// </summary>
        public SerializeEntry() { }
        /// <summary>
        /// <see cref="SerializeEntry"/>の新しいインスタンスを生成する
        /// </summary>
        /// <param name="type">シリアライズする要素の型</param>
        /// <exception cref="ArgumentNullException"><paramref name="type"/>がnull</exception>
        public SerializeEntry(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type), "引数がnullです");
            TypeName = type.FullName;
            IsNull = false;
            Fields = new();
        }
    }
}
