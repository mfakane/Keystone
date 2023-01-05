using System.Linq;

namespace Linearstar.Keystone.IO.Text
{
    public sealed class Token
    {
        public string Kind { get; }

        public int Index { get; }

        public string Text { get; }

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

        public override string ToString() => this.Kind + " " + this.Text;
    }
}