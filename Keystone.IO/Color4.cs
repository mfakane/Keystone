using System;

namespace Linearstar.Keystone.IO;

public readonly struct Color4 : IEquatable<Color4>
{
    public static readonly Color4 Zero = new();
    public static readonly Color4 One = new(1, 1, 1, 1);

    public readonly float R;
    public readonly float G;
    public readonly float B;
    public readonly float A;

    public Color4(float r, float g, float b, float a)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }
    
    public Color4(params float[] array)
    {
        if (array.Length < 4) throw new ArgumentException("Array must have 4 or more elements.", nameof(array));
        
        R = array[0];
        G = array[1];
        B = array[2];
        A = array[3];
    }
    
    public static Color4 Parse(string str, char delimiter)
    {
        var sl = str.Split(new[] { delimiter }, 4);

        return new(float.Parse(sl[0]), float.Parse(sl[1]), float.Parse(sl[2]), float.Parse(sl[3]));
    }
    
    public static Color4 operator +(Color4 a, Color4 b) => new(a.R + b.R, a.G + b.G, a.B + b.B, a.A + b.A);

    public static Color4 operator -(Color4 a, Color4 b) => new(a.R - b.R, a.G - b.G, a.B - b.B, a.A - b.A);

    public static Color4 operator *(Color4 a, float b) => new(a.R * b, a.G * b, a.B * b, a.A * b);

    public static Color4 operator /(Color4 a, float b) => new(a.R / b, a.G / b, a.B / b, a.A / b);

    public static Color4 operator -(Color4 a) => new(-a.R, -a.G, -a.B, -a.A);

    public static bool operator ==(Color4 a, Color4 b) => a.Equals(b);

    public static bool operator !=(Color4 a, Color4 b) => !a.Equals(b);

    public override bool Equals(object obj) => obj is Color4 other && Equals(other);

    public bool Equals(Color4 other) => R.Equals(other.R) && G.Equals(other.G) && B.Equals(other.B) && A.Equals(other.A);

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