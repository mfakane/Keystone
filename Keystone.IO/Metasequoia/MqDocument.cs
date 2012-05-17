using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Linearstar.Keystone.IO.Metasequoia
{
	/// <summary>
	/// メタセコイア オブジェクト created by O.Mizno
	/// </summary>
	public class MqDocument
	{
		public const string DisplayName = "Metasequoia Object";
		public const string Filter = "*.mqo;*.mqm";
		public const string NewLine = "\r\n";
		public static readonly Encoding Encoding = Encoding.GetEncoding(932);

		/// <summary>
		/// Format Text Ver
		/// </summary>
		public float Version
		{
			get;
			set;
		}

		/// <summary>
		/// IncludeXml
		/// </summary>
		public string IncludeXml
		{
			get;
			set;
		}

		/// <summary>
		/// Scene
		/// </summary>
		public MqScene Scene
		{
			get;
			set;
		}

		/// <summary>
		/// Material
		/// </summary>
		public IList<MqMaterial> Materials
		{
			get;
			set;
		}

		/// <summary>
		/// Object
		/// </summary>
		public IList<MqObject> Objects
		{
			get;
			set;
		}

		public IList<MqChunk> CustomChunks
		{
			get;
			set;
		}

		public MqDocument()
		{
			this.Version = 1.0f;
			this.Materials = new List<MqMaterial>();
			this.Objects = new List<MqObject>();
			this.CustomChunks = new List<MqChunk>();
		}

		public static MqDocument Parse(string text)
		{
			var rt = new MqDocument();
			var tokenizer = new MqTokenizer(text);
			var header = new[] { "Metasequoia", "Document", "\n", "Format", "Text", "Ver" };

			if (!tokenizer.Take(header.Length).Select(_ => _.Text).SequenceEqual(header))
				throw new InvalidOperationException("invalid format");

			rt.Version = float.Parse(tokenizer.MoveNext().EnsureKind(MqTokenizer.DigitTokenKind).Text);

			if (rt.Version >= 2)
				throw new NotSupportedException("specified format version not supported");

			foreach (var i in tokenizer)
				if (i.Kind == MqTokenizer.IdentifierTokenKind)
				{
					var chunk = MqChunk.Parse(tokenizer);

					switch (chunk.Name.ToLower())
					{
						case "includexml":
							rt.IncludeXml = chunk.Arguments.First().Trim('"');

							break;
						case "scene":
							rt.Scene = MqScene.Parse(chunk);

							break;
						case "material":
							rt.Materials = chunk.Children.Select(MqMaterial.Parse).ToList();

							break;
						case "object":
							rt.Objects.Add(MqObject.Parse(chunk));

							break;
						case "eof":
							break;
						default:
							rt.CustomChunks.Add(chunk);

							break;
					}
				}

			return rt;
		}

		public string GetFormattedText()
		{
			var sb = new StringBuilder();

			sb.AppendLine("Metasequoia Document");
			sb.AppendLine("Format Text Ver " + this.Version.ToString("0.0"));
			sb.AppendLine();

			if (!string.IsNullOrEmpty(this.IncludeXml))
				sb.AppendLine("IncludeXml \"" + this.IncludeXml + "\"");

			if (this.Scene != null)
				sb.AppendLine(this.Scene.GetFormattedText());

			sb.AppendLine("Material " + this.Materials.Count + " {");

			foreach (var i in this.Materials)
				sb.AppendLine("\t" + i.GetFormattedText().Replace("\n", "\n\t"));

			sb.AppendLine("}");

			foreach (var i in this.Objects)
				sb.AppendLine(i.GetFormattedText());

			foreach (var i in this.CustomChunks)
				sb.AppendLine(i.GetFormattedText());

			sb.AppendLine("Eof");

			return sb.ToString();
		}
	}
}