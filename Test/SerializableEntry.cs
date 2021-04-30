using System;
using System.Collections.Generic;

namespace Test
{
    [Serializable]
    public class SerializableEntry : IEquatable<SerializableEntry>
    {
        private readonly int Private = 1;
        internal char Internal = '2';
        protected int Protected { get; set; } = 3;
        public string Public = "4";
        [NonSerialized]
        public bool NonSerializedField = false;
        public bool Equals(SerializableEntry other)
        {
            return other is not null &&
                   Private == other.Private &&
                   Internal == other.Internal &&
                   Protected == other.Protected &&
                   NonSerializedField == other.NonSerializedField &&
                   Public == other.Public;
        }
        public override bool Equals(object obj) => Equals(obj as SerializableEntry);
        public static bool operator ==(SerializableEntry left, SerializableEntry right) => EqualityComparer<SerializableEntry>.Default.Equals(left, right);
        public static bool operator !=(SerializableEntry left, SerializableEntry right) => !(left == right);
    }
}
