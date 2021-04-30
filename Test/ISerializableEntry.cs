using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Test
{
    [Serializable]
    public class ISerializableEntry : IEquatable<ISerializableEntry>, ISerializable, IDeserializationCallback
    {
        private SerializationInfo siInfo;
        private const string IdName = "Id";
        private const string ValueName = "Value";
        public int Id { get; set; } = 5;
        private string value = "AAA";
        public ISerializableEntry() { }
        private ISerializableEntry(SerializationInfo info, StreamingContext context)
        {
            if (info == null) throw new ArgumentNullException(nameof(info), "引数がnullです");
            siInfo = info;
        }
        public bool Equals(ISerializableEntry other)
        {
            return other is not null &&
                Id == other.Id &&
                value == other.value;
        }
        public override bool Equals(object obj) => Equals(obj as ISerializableEntry);
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) throw new ArgumentNullException(nameof(info), "引数がnullです");
            info.AddValue(IdName, Id);
            info.AddValue(ValueName, value);
        }
        void IDeserializationCallback.OnDeserialization(object sender)
        {
            if (siInfo == null) return;
            Id = siInfo.GetInt32(IdName);
            value = siInfo.GetString(ValueName);
            siInfo = null;
        }
        public static bool operator ==(ISerializableEntry left, ISerializableEntry right) => EqualityComparer<ISerializableEntry>.Default.Equals(left, right);
        public static bool operator !=(ISerializableEntry left, ISerializableEntry right) => !(left == right);
    }
}
