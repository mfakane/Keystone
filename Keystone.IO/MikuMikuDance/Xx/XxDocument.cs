using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Linearstar.Keystone.IO.MikuMikuDance.Xx
{
	/// <summary>
	/// XX file created by Higuchi_U
	/// </summary>
	public class XxDocument
	{
		public const string DisplayName = "XX file";
		public const string Filter = "*.xx";
		
		internal static Encoding Encoding => Encoding.GetEncoding(932);
		internal const int StringCapacity = 256;

		public IList<XxVertex> Vertices { get; set; } = new List<XxVertex>();

		public IList<XxLine> Lines { get; set; } = new List<XxLine>();

		public IList<XxMaterial> Materials { get; set; } = new List<XxMaterial>();
		
		static XxDocument()
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		}
		
		public static XxDocument FromFile(string path)
		{
			using var fs = File.OpenRead(path);

			return Parse(fs);
		}

		public static XxDocument Parse(in ReadOnlySequence<byte> sequence)
		{
			var rt = new XxDocument();
			var br = new BufferReader(sequence);
			
			var vertices = br.ReadUInt32();
			for (uint i = 0; i < vertices; i++)
				rt.Vertices.Add(XxVertex.Parse(ref br));

			var lines = br.ReadUInt32();
			for (uint i = 0; i < lines; i++)
				rt.Lines.Add(XxLine.Parse(ref br));

			var materials = br.ReadUInt32();
			for (uint i = 0; i < materials; i++)
				rt.Materials.Add(XxMaterial.Parse(ref br));

			return rt;
		}

		public static XxDocument Parse(Stream stream) =>
			Parse(stream.AsReadOnlySequence());
		
		public void Write(IBufferWriter<byte> writer)
		{
			var bw = new BufferWriter(writer);

			bw.Write(this.Vertices.Count);
			foreach (var vertex in this.Vertices) vertex.Write(ref bw);
			
			bw.Write(this.Lines.Count);
			foreach (var line in this.Lines) line.Write(ref bw);
			
			bw.Write(this.Materials.Count);
			foreach (var material in this.Materials) material.Write(ref bw);
		}
		
		public void Write(Stream stream)
		{
			using var sbw = new StreamBufferWriter(stream);

			Write(sbw);
		}
	}
}
