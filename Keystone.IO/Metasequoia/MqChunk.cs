using System;
using System.Collections.Generic;
using System.Linq;
using Linearstar.Keystone.IO;
using Linearstar.Keystone.IO.Text;

namespace Linearstar.Keystone.IO.Metasequoia
{
	/// <summary>
	/// chunk [args] [attribute(args)] [{ ... }]
	/// </summary>
	public class MqChunk
	{
		public string Name
		{
			get;
			set;
		}

		public IList<string> Arguments
		{
			get;
			set;
		}

		public IList<MqChunkAttribute> Attributes
		{
			get;
			set;
		}

		public IList<MqChunk> Children
		{
			get;
			set;
		}

		public bool AlwaysHaveChildren
		{
			get;
			set;
		}

		public MqChunk()
		{
			this.Attributes = new List<MqChunkAttribute>();
			this.Arguments = new List<string>();
			this.Children = new List<MqChunk>();
		}

		public MqChunk SetArguments(IEnumerable<string> args)
		{
			return SetArguments(args.ToArray());
		}

		public MqChunk SetArguments(params string[] args)
		{
			this.Arguments.Clear();
			this.Arguments = args.ToList();

			return this;
		}

		public MqChunk SetAttributes(IEnumerable<MqChunkAttribute> attrs)
		{
			return SetAttributes(attrs.ToArray());
		}

		public MqChunk SetAttributes(params MqChunkAttribute[] attrs)
		{
			this.Attributes.Clear();
			this.Attributes = attrs.ToList();

			return this;
		}

		public MqChunk SetChildren(IEnumerable<MqChunk> children)
		{
			return SetChildren(children.ToArray());
		}

		public MqChunk SetChildren(params MqChunk[] children)
		{
			this.AlwaysHaveChildren = true;
			this.Children.Clear();
			this.Children = children.ToList();

			return this;
		}

		public MqChunkAttribute Attribute(string name, bool createIfNotFound = true)
		{
			var value = this.Attributes.FirstOrDefault(_ => _.Name.Equals(name, StringComparison.CurrentCulture));

			if (createIfNotFound && value == null)
				this.Attributes.Add(value = new MqChunkAttribute
				{
					Name = name,
				});

			return value;
		}

		public void RemoveAttributes(string name)
		{
			this.Attributes.Where(_ => _.Name.Equals(name, StringComparison.CurrentCulture)).ToArray().ForEach(_ => this.Attributes.Remove(_));
		}

		public MqChunk Child(string name, bool createIfNotFound = true)
		{
			var value = this.Children.FirstOrDefault(_ => _.Name.Equals(name, StringComparison.CurrentCulture));

			if (createIfNotFound && value == null)
				this.Children.Add(value = new MqChunk
				{
					Name = name,
				});

			return value;
		}

		public void RemoveChildren(string name)
		{
			this.Children.Where(_ => _.Name.Equals(name, StringComparison.CurrentCulture)).ToArray().ForEach(_ => this.Children.Remove(_));
		}

		public static MqChunk Parse(MqTokenizer tokenizer)
		{
			while (tokenizer.Current.Kind == MqTokenizer.NewLineTokenKind)
				tokenizer.MoveNext();

			var rt = new MqChunk
			{
				Name = tokenizer.Current.Kind == MqTokenizer.IdentifierTokenKind || tokenizer.Current.Kind == MqTokenizer.StringTokenKind
					? tokenizer.Current.Text
					: null,
			};
			var args = tokenizer.StartWith(tokenizer.Current.Kind == MqTokenizer.DigitTokenKind ? new[] { tokenizer.Current } : new Token[0])
								.TakeWhile(_ => _.Kind != MqTokenizer.BeginChildrenTokenKind && _.Kind != MqTokenizer.NewLineTokenKind)
								.ToArray();

			rt.Arguments = args.TakeWhile(_ => _.Kind == MqTokenizer.DigitTokenKind || _.Kind == MqTokenizer.StringTokenKind).Select(_ => _.Text).ToList();
			rt.Attributes = Util.Defer(() =>
			{
				var attrs = args.Skip(rt.Arguments.Count);
				var current = attrs.FirstOrDefault();

				return attrs.GroupBy(_ =>
				{
					if (_.Kind == MqTokenizer.IdentifierTokenKind)
						current = _;

					return current;
				})
				.Select(_ => new MqChunkAttribute
				{
					Name = _.Key.Text,
					Arguments = _.Skip(1).Select(t => t.Text).ToList(),
				});
			}).ToList();

			if (tokenizer.Current.Kind == MqTokenizer.BeginChildrenTokenKind)
			{
				rt.Children = tokenizer.TakeWhile(_ => _.Kind != MqTokenizer.EndChildrenTokenKind)
									   .Select(_ => Parse(tokenizer))
									   .ToList();
				tokenizer.MoveNext(MqTokenizer.EndChildrenTokenKind);
			}

			return rt;
		}

		public string GetFormattedText()
		{
			var str = string.Join(" ", new[] { this.Name }.Concat(this.Arguments).Concat(this.Attributes.Select(_ => _.GetFormattedText())));

			if (this.Children.Any())
				str += " {" + MqDocument.NewLine + "\t" + string.Join(MqDocument.NewLine, this.Children.Select(_ => _.GetFormattedText())).Replace(MqDocument.NewLine, MqDocument.NewLine + "\t") + MqDocument.NewLine + "}";
			else if (this.AlwaysHaveChildren)
				str += " {" + MqDocument.NewLine + "}";

			return str.Trim();
		}
	}
}
