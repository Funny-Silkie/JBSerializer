using System;
using System.ComponentModel;

namespace JBSerializer
{
    /// <summary>
    /// シリアライズされる要素の型と値のエントリーのクラス
    /// </summary>
    [Serializable]
    internal class TypeValueEntry : IEquatable<TypeValueEntry>, ICloneable
    {
        /// <summary>
        /// 要素の型名を取得または設定する
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 値を取得または設定する
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// インスタンスの複製を作成する
        /// </summary>
        /// <returns>このインスタンスの複製</returns>
        public TypeValueEntry Clone() => new() { TypeName = TypeName, Value = Value };
        object ICloneable.Clone() => Clone();
        /// <inheritdoc/>
        public bool Equals(TypeValueEntry other)
        {
            if (other is null || TypeName != other.TypeName) return false;
            if (Value == null) return other.Value == null;
            return Value.Equals(other.Value);
        }
        /// <inheritdocs/>
        public override bool Equals(object obj) => Equals(obj as TypeValueEntry);
        /// <inheritdoc/>
        public override int GetHashCode() => HashCode.Combine(TypeName, Value);
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Deconstruct(out string name, out object value)
        {
            name = TypeName;
            value = Value;
        }
        public static implicit operator (string, object)(TypeValueEntry entry) => (entry?.TypeName, entry?.Value);
        public static implicit operator TypeValueEntry((string name, object value) tuple) => new() { TypeName = tuple.name, Value = tuple.value };
        public static bool operator ==(TypeValueEntry left, TypeValueEntry right)
        {
            if (left is null) return right is null;
            return left.Equals(right);
        }
        public static bool operator !=(TypeValueEntry left, TypeValueEntry right) => !(left == right);
    }
}
