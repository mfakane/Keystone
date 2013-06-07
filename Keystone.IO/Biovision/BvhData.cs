using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Linearstar.Keystone.IO.Biovision
{
	public class BvhData
	{
		public string Name
		{
			get;
			set;
		}

		public List<string> Values
		{
			get;
			set;
		}

		public List<BvhData> Children
		{
			get;
			set;
		}

		public BvhData()
		{
			this.Values = new List<string>();
			this.Children = new List<BvhData>();
		}

		public BvhData(string name)
			: this()
		{
			this.Name = name;
		}

		public BvhData SetValues(IEnumerable<string> args)
		{
			return SetValues(args.ToArray());
		}

		public BvhData SetValues(params string[] args)
		{
			if (this.Values == null)
				this.Values = new List<string>();
			else
				this.Values.Clear();

			this.Values.AddRange(args);

			return this;
		}

		public BvhData Child(string name, bool createIfNotFound = true)
		{
			if (this.Children == null)
				if (createIfNotFound)
					this.Children = new List<BvhData>();
				else
					return null;

			var value = this.Children.FirstOrDefault(_ => _.Name.Equals(name));

			if (createIfNotFound && value == null)
				this.Children.Add(value = new BvhData
				{
					Name = name,
				});

			return value;
		}

		public void RemoveChildren(string name)
		{
			if (this.Children != null)
				this.Children.Where(_ => _.Name.Equals(name)).ToArray().ForEach(_ => this.Children.Remove(_));
		}

		public static BvhData Parse(BvhTokenizer tokenizer)
		{
			while (tokenizer.Current.Kind == BvhTokenizer.NewLineTokenKind)
				tokenizer.MoveNext();

			var rt = new BvhData();

			if (tokenizer.Current.Kind == BvhTokenizer.IdentifierTokenKind)
			{
				rt.Name = tokenizer.Current.Text;
				tokenizer.MoveNext();
			}

			rt.Values = tokenizer.StartWith(tokenizer.Current)
								 .TakeWhile(_ => _.Kind != BvhTokenizer.NewLineTokenKind && _.Kind != BvhTokenizer.BeginChildrenTokenKind)
								 .Select(_ => _.Text)
								 .ToList();

			if (tokenizer.Current.Kind == BvhTokenizer.NewLineTokenKind &&
				tokenizer.PeekNext().Kind == BvhTokenizer.BeginChildrenTokenKind)
			{
				tokenizer.MoveNext(BvhTokenizer.NewLineTokenKind);
				rt.Children = tokenizer.TakeWhile(_ => _.Kind != BvhTokenizer.EndChildrenTokenKind)
									   .Select(_ => Parse(tokenizer))
									   .ToList();
				tokenizer.MoveNext(BvhTokenizer.EndChildrenTokenKind);
			}

			return rt;
		}

		public string GetFormattedText()
		{
			var sb = new StringBuilder();

			if (this.Name != null)
				sb.Append(this.Name);

			if (this.Values != null)
			{
				if (this.Name != null)
					sb.Append(" ");

				sb.Append(string.Join(" ", this.Values));
			}

			if (this.Children != null)
			{
				sb.AppendLine(" {");

				foreach (var i in this.Children)
					sb.AppendLine("\t" + i.GetFormattedText().Replace("\n", "\n\t"));

				sb.Append("}");
			}

			return sb.ToString();
		}
	}
}
