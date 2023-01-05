using System;

namespace Linearstar.Keystone.IO;

public readonly struct Color4B : IEquatable<Color4B>
{
    public static readonly Color4B Zero = new();

    public readonly byte R;
    public readonly byte G;
    public readonly byte B;
    public readonly byte A;

    public Color4B(byte r, byte g, byte b, byte a)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    public static bool operator ==(Color4B a, Color4B b) => a.Equals(b);

    public static bool operator !=(Color4B a, Color4B b) => !a.Equals(b);

    public override bool Equals(object obj) => obj is Color4B other && Equals(other);

    public bool Equals(Color4B other) => R.Equals(other.R) && G.Equals(other.G) && B.Equals(other.B) && A.Equals(other.A);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = R.GetHashCode();
            hashCode = (hashCode * 397) ^ G.GetHashCode();
            hashCode = (hashCode * 397) ^ B.GetHashCode();
            hashCode = (hashCode * 397) ^ A.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString() => $"({R}, {G}, {B}, {A})";
}