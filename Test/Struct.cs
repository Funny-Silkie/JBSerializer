using System;

namespace Test
{
    [Serializable]
    public struct Struct : IEquatable<Struct>
    {
        public int Value { get; set; }
        public Struct(int value)
        {
            Value = value;
        }
        public readonly bool Equals(Struct other) => Value == other.Value;
        public override readonly bool Equals(object obj) => obj is Struct other && Equals(other);
    }
}
