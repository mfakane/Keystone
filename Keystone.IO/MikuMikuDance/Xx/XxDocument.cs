using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	/// <summary>
	/// XX file created by Higuchi_U
	/// </summary>
	public class XxDocument
	{
		public const string DisplayName = "XX file";
		public const string Filter = "*.xx";
		public static readonly Encoding Encoding = Encoding.GetEncoding(932);

		public IList<XxVertex> Vertices
		{
			get;
			set;
		}

		public IList<XxLine> Lines
		{
			get;
			set;
		}

		public IList<XxMaterial> Materials
		{
			get;
			set;
		}

		public XxDocument()
		{
			this.Vertices = new List<XxVertex>();
			this.Lines = new List<XxLine>();
			this.Materials = new List<XxMaterial>();
		}

		public static XxDocument Parse(Stream stream)
		{
			var rt = new XxDocument();

			using (var br = new BinaryReader(stream))
			{
				var vertices = br.ReadUInt32();

				for (uint i = 0; i < vertices; i++)
					rt.Vertices.Add(XxVertex.Parse(br));

				var lines = br.ReadUInt32();

				for (uint i = 0; i < lines; i++)
					rt.Lines.Add(XxLine.Parse(br));

				var materials = br.ReadUInt32();

				for (uint i = 0; i < materials; i++)
					rt.Materials.Add(XxMaterial.Parse(br));
			}

			return rt;
		}

		internal static string ReadXxString(BinaryReader br, int count)
		{
			return Encoding.GetString(br.ReadBytes(count).TakeWhile(_ => _ != '\0').ToArray());
		}

		internal static void WriteXxString(BinaryWriter bw, string s, int count, byte padding = 0xFD)
		{
			var bytes = Encoding.GetBytes(s);

			bw.Write(bytes.Take(count - 1).ToArray());
			bw.Write(Enumerable.Repeat(padding, Math.Max(count - bytes.Length, 1)).Select((_, idx) => idx == 0 ? (byte)0 : _).ToArray());
		}

		public void Write(Stream stream)
		{
			using (var bw = new BinaryWriter(stream))
			{
				bw.Write((uint)this.Vertices.Count);
				this.Vertices.ForEach(_ => _.Write(bw));
				bw.Write((uint)this.Lines.Count);
				this.Lines.ForEach(_ => _.Write(bw));
				bw.Write((uint)this.Materials.Count);
				this.Materials.ForEach(_ => _.Write(bw));
			}
		}
	}
}
