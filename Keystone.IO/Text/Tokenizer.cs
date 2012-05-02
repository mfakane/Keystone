using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Linearstar.Keystone.IO.Text
{
	public abstract class Tokenizer : IEnumerable<Token>, IEnumerator<Token>
	{
		/// <summary>
		/// 現在処理中の文字インデックスです。
		/// </summary>
		public int Index
		{
			get;
			set;
		}

		/// <summary>
		/// 現在処理中のトークンの長さです。-1 の場合、トークンの Text の長さが自動的に設定されます。
		/// </summary>
		public int TokenLength
		{
			get;
			set;
		}

		public int Length
		{
			get
			{
				return this.Code.Length;
			}
		}

		public Token Current
		{
			get;
			private set;
		}

		public string Code
		{
			get;
			private set;
		}

		public bool HasNext
		{
			get
			{
				return this.Index < this.Length;
			}
		}

		public virtual void Dispose()
		{
			GC.SuppressFinalize(this);
		}

		~Tokenizer()
		{
			Dispose();
		}

		object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		public Tokenizer(string code)
		{
			this.Code = code;
		}

		protected abstract Token CreateToken();

		public Token PeekNext()
		{
			return CreateToken();
		}

		public char PeekChar()
		{
			return PeekChar(0);
		}

		public char PeekChar(int shift)
		{
			return this.Index + shift >= this.Length ? default(char) : this.Code[this.Index + shift];
		}

		public string TakeStringWhile(Func<char, bool> condition)
		{
			return TakeStringWhile(0, (_, i) => condition(_.PeekChar(i)));
		}

		public string TakeStringWhile(Func<Tokenizer, int, bool> condition)
		{
			return TakeStringWhile(0, condition);
		}

		public string TakeStringWhile(int skip, Func<char, bool> condition)
		{
			return TakeStringWhile(skip, (_, i) => condition(_.PeekChar(i)));
		}

		public string TakeStringWhile(int skip, Func<Tokenizer, int, bool> condition)
		{
			return string.Join(null, Util.Repeat(this)
										 .TakeWhile((_, i) => condition(_, skip + i))
										 .Select((_, i) => _.PeekChar(skip + i).ToString()).ToArray());
		}

		public string TakeStringUntil(string start, string end)
		{
			return TakeStringUntil(start, end, new string[0]);
		}

		public string TakeStringUntil(string start, string end, params string[] ignore)
		{
			var ignored = new List<KeyValuePair<int, int>>();

			return TakeStringWhile(start.Length, (_, i) =>
			{
				if (ignored.Any(__ => i >= __.Key && i < __.Key + __.Value))
					return true;

				if (_.PeekChar(i) == default(char))
					return false;

				foreach (var j in ignore)
				{
					var match = false;

					for (int k = 0; k < j.Length; k++)
						if (!(match = _.PeekChar(i + k) == j[k]))
							break;

					if (match)
					{
						ignored.Add(new KeyValuePair<int, int>(i, j.Length));

						return true;
					}
				}

				for (int j = 0; j < end.Length; j++)
					if (_.PeekChar(i + j) != end[j])
						return true;

				return false;
			});
		}

		public Token MoveNext(params string[] ensureCurrent)
		{
			if (!ensureCurrent.Contains(this.Current.Kind))
				throw new ParseException(this.Current, ensureCurrent);

			return MoveNext();
		}

		public Token MoveNext()
		{
			this.TokenLength = -1;
			this.Current = PeekNext();

			if (this.Current != null)
				this.Index += this.TokenLength == -1 ? this.Current.Text.Length : this.TokenLength;

			return this.Current;
		}

		bool IEnumerator.MoveNext()
		{
			return MoveNext() != null;
		}

		public void Reset()
		{
			this.Index = 0;
			this.Current = null;
		}

		public IEnumerator<Token> GetEnumerator()
		{
			return this;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
