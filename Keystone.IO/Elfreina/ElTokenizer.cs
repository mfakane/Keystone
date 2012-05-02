using Linearstar.Keystone.IO.Text;

namespace Linearstar.Keystone.IO.Elfreina
{
	public class ElTokenizer : Tokenizer
	{
		public const string StringTokenKind = "String";
		public const string NewLineTokenKind = "NewLine";
		public const string IdentifierTokenKind = "Identifier";
		public const string ValueTokenKind = "Value";
		public const string BeginChildrenTokenKind = "BeginChildren";
		public const string EndChildrenTokenKind = "EndChildren";

		public ElTokenizer(string code)
			: base(code)
		{
		}

		protected override Token CreateToken()
		{
			while (this.Index < this.Length)
			{
				var c = this.PeekChar();

				switch (c)
				{
					case '{':
						return new Token(BeginChildrenTokenKind, this.Index, "{");
					case '}':
						return new Token(EndChildrenTokenKind, this.Index, "}");
					case '\n':
						return new Token(NewLineTokenKind, this.Index, "\n");
					case '"':
						return new Token(StringTokenKind, this.Index, "\"" + this.TakeStringUntil("\"", "\"") + "\"");
					default:
						if (char.IsLetter(c) || c == '_')
							return new Token(IdentifierTokenKind, this.Index, this.TakeStringWhile(_ => char.IsLetterOrDigit(_) || _ == '_'));
						else if (char.IsDigit(c) || c == '-')
							return new Token(ValueTokenKind, this.Index, this.TakeStringWhile(_ => char.IsDigit(_) || _ == '.' || _ == '-' || _ == 'e' || _ == ','));

						break;
				}

				this.Index++;
			}

			return null;
		}
	}
}
