using System;

namespace Linearstar.Keystone.IO;

public readonly struct Color3 : IEquatable<Color3>
{
    public static readonly Color3 Zero = new();
    public static readonly Color3 One = new(1, 1, 1);

    public readonly float R;
    public readonly float G;
    public readonly float B;

    public Color3(float r, float g, float b)
    {
        R = r;
        G = g;
        B = b;
    }
    
    public Color3(params float[] array)
    {
        if (array.Length < 3) throw new ArgumentException("Array must have 3 or more elements.", nameof(array));
        
        R = array[0];
        G = array[1];
        B = array[2];
    }
    
    public static Color3 Parse(string str, char delimiter)
    {
        var sl = str.Split(new[] { delimiter }, 3);

        return new(float.Parse(sl[0]), float.Parse(sl[1]), float.Parse(sl[2]));
    }
    
    public static Color3 operator +(Color3 a, Color3 b) => new(a.R + b.R, a.G + b.G, a.B + b.B);

    public static Color3 operator -(Color3 a, Color3 b) => new(a.R - b.R, a.G - b.G, a.B - b.B);

    public static Color3 operator *(Color3 a, float b) => new(a.R * b, a.G * b, a.B * b);

    public static Color3 operator /(Color3 a, float b) => new(a.R / b, a.G / b, a.B / b);

    public static Color3 operator -(Color3 a) => new(-a.R, -a.G, -a.B);

    public static bool operator ==(Color3 a, Color3 b) => a.Equals(b);

    public static bool operator !=(Color3 a, Color3 b) => !a.Equals(b);

    public override bool Equals(object obj) => obj is Color3 other && Equals(other);

    public bool Equals(Color3 other) => R.Equals(other.R) && G.Equals(other.G) && B.Equals(other.B);

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