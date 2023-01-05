using System;

namespace Linearstar.Keystone.IO
{
	static class EnumEx
	{
		public static T Parse<T>(string value)
			where T : struct, IComparable, IFormattable, IConvertible
		{
			return (T)Enum.Parse(typeof(T), value, false);
		}
	}
}
