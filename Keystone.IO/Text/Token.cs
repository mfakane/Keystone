using System.Linq;

namespace Linearstar.Keystone.IO.Text
{
	public sealed class Token
	{
		public string Kind
		{
			get;
			private set;
		}

		public int Index
		{
			get;
			private set;
		}

		public string Text
		{
			get;
			private set;
		}

		public Token(string kind, int index, string text)
		{
			this.Kind = kind;
			this.Index = index;
			this.Text = text;
		}

		public Token EnsureKind(params string[] kind)
		{
			if (!kind.Contains(this.Kind))
				throw new ParseException(this, kind);

			return this;
		}

		public override string ToString()
		{
			return this.Kind.ToString() + " " + this.Text;
		}
	}
}
