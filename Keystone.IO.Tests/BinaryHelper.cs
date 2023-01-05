using System.Globalization;

namespace Linearstar.Keystone.IO.Tests;

public static class BinaryHelper
{
    public static byte[] FromString(string str) =>
        str.Split(new[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(x => byte.Parse(x, NumberStyles.HexNumber))
            .ToArray();
}