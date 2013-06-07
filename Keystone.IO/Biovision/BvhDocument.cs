using System;
using System.Linq;
using System.Text;

namespace Linearstar.Keystone.IO.Biovision
{
	/// <summary>
	/// Biovision Hierarchy created by Biovision
	/// </summary>
	public class BvhDocument
	{
		public const string DisplayName = "Biovision Hierarchy";
		public const string Filter = "*.bvh";
		public const string NewLine = "\r\n";
		public static readonly Encoding Encoding = Encoding.GetEncoding(932);

		public BvhJoint Root
		{
			get;
			set;
		}

		public BvhMotion Motion
		{
			get;
			set;
		}

		public BvhDocument()
		{
			this.Motion = new BvhMotion();
		}

		public static BvhDocument Parse(string text)
		{
			var rt = new BvhDocument();
			var tokenizer = new BvhTokenizer(text);
			var header = new[] { "HIERARCHY" };

			if (!tokenizer.Take(header.Length).Select(_ => _.Text).SequenceEqual(header))
				throw new InvalidOperationException("invalid format");

			foreach (var i in tokenizer)
				if (i.Kind == BvhTokenizer.IdentifierTokenKind)
				{
					switch (i.Text)
					{
						case "ROOT":
							rt.Root = BvhJoint.Parse(BvhData.Parse(tokenizer));

							break;
						case "MOTION":
							rt.Motion = BvhMotion.Parse(rt, tokenizer);

							break;
					}
				}

			return rt;
		}

		public string GetFormattedText()
		{
			var sb = new StringBuilder();

			sb.AppendLine("HIERARCHY");
			sb.AppendLine(this.Root.GetFormattedText());
			sb.Append(this.Motion);

			return sb.ToString();
		}
	}
}
