using System;

namespace Linearstar.Keystone.IO;

public readonly struct Color3B : IEquatable<Color3B>
{
    public static readonly Color3B Zero = new();

    public readonly byte R;
    public readonly byte G;
    public readonly byte B;

    public Color3B(byte r, byte g, byte b)
    {
        R = r;
        G = g;
        B = b;
    }

    public static bool operator ==(Color3B a, Color3B b) => a.Equals(b);

    public static bool operator !=(Color3B a, Color3B b) => !a.Equals(b);

    public override bool Equals(object obj) => obj is Color3B other && Equals(other);

    public bool Equals(Color3B other) => R.Equals(other.R) && G.Equals(other.G) && B.Equals(other.B);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = R.GetHashCode();
            hashCode = (hashCode * 397) ^ G.GetHashCode();
            hashCode = (hashCode * 397) ^ B.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString() => $"({R}, {G}, {B})";
}