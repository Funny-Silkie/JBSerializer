using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace JBSerializer
{
    /// <summary>
    /// シリアライズに使用されるエントリー
    /// </summary>
    [Serializable]
    public sealed class SerializeEntry
    {
        /// <summary>
        /// 型名を取得する
        /// </summary>
        public string TypeName { get; internal set; }
        /// <summary>
        /// フィールドを取得する
        /// </summary>
        public ReadOnlyDictionary<string, object> Fields => _fields ??= new ReadOnlyDictionary<string, object>(_fieldsPrivate);
        [NonSerialized]
        private ReadOnlyDictionary<string, object> _fields;
        internal Dictionary<string, object> FieldsInternal
        {
            get => _fieldsPrivate;
            set
            {
                if (_fieldsPrivate == value) return;
                _fieldsPrivate = value;
                _fields = null;
            }
        }
        private Dictionary<string, object> _fieldsPrivate;
        /// <summary>
        /// <see cref="SerializeEntry"/>の新しいインスタンスを生成する
        /// </summary>
        internal SerializeEntry() { }
    }
}
