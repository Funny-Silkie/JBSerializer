using System;

namespace JBSerializer
{
    /// <summary>
    /// 配列のシリアライズに使用するエントリー
    /// </summary>
    [Serializable]
    internal sealed class ArraySerializeEntry
    {
        /// <summary>
        /// nullを表す<see cref="ArraySerializeEntry"/>のインスタンスを取得する
        /// </summary>
        public static ArraySerializeEntry Null => new() { IsNull = true };
        /// <summary>
        /// 値がnullを表すかどうかを取得または設定する
        /// </summary>
        public bool IsNull { get; set; }
        /// <summary>
        /// 要素の型名を取得または設定する
        /// </summary>
        public string ItemTypeName { get; set; }
        /// <summary>
        /// 要素を取得または設定する
        /// </summary>
        public TypeValueEntry[] Items { get; set; }
        /// <summary>
        /// <see cref="ArraySerializeEntry"/>の新しいインスタンスを生成する
        /// </summary>
        public ArraySerializeEntry() { }
        /// <summary>
        /// <see cref="ArraySerializeEntry"/>の新しいインスタンスを生成する
        /// </summary>
        /// <param name="itemType">要素の型</param>
        /// <param name="length">配列長</param>
        /// <exception cref="ArgumentNullException"><paramref name="itemType"/>がnull</exception>
        public ArraySerializeEntry(Type itemType, int length)
        {
            ItemTypeName = ReflectionHelper.GetTypeName(itemType);
            Items = new TypeValueEntry[length];
        }
    }
}
