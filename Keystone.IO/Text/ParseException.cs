using System;
using System.Linq;

namespace Linearstar.Keystone.IO.Text
{
	public sealed class ParseException : Exception
	{
		public Token Token
		{
			get;
			private set;
		}

		public string[] Expected
		{
			get;
			private set;
		}

		public ParseException(Token unexpectedToken, params string[] expected)
			: base(string.Format("Unexpected {0}, {1} expected", unexpectedToken, string.Join(" or ", expected.Select(_ => _.ToString()).ToArray())))
		{
			this.Token = unexpectedToken;
			this.Expected = expected;
		}

		public ParseException(Token unexpectedToken, string message)
			: base(string.Format("Unexpected {0}, {1}", unexpectedToken, message))
		{
			this.Token = unexpectedToken;
		}

		public ParseException(Token unexpectedToken)
			: base(string.Format("Unexpected {0}", unexpectedToken))
		{
			this.Token = unexpectedToken;
		}
	}
}
