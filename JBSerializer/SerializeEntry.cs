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
        /// 値がnullかどうかを取得または設定する
        /// </summary>
        public bool IsNull { get; set; }
        /// <summary>
        /// 型名を取得または設定する
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// フィールドを取得する
        /// </summary>
        public Dictionary<string, TypeValueEntry> Fields { get; set; }
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
            TypeName = ReflectionHelper.GetTypeName(type);
            IsNull = false;
            Fields = new();
        }
    }
}
