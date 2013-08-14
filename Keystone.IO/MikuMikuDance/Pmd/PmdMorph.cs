using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public class PmdMorph
	{
		public string Name
		{
			get;
			set;
		}

		public string EnglishName
		{
			get;
			set;
		}

		public PmdMorphKind Kind
		{
			get;
			set;
		}

		public IList<PmdVertex> Indices
		{
			get;
			set;
		}

		public IList<float[]> Offsets
		{
			get;
			set;
		}

		public PmdMorph()
		{
			this.Indices = new List<PmdVertex>();
			this.Offsets = new List<float[]>();
		}

		public static PmdMorph Parse(BinaryReader br, PmdDocument doc, PmdMorph morphBase)
		{
			var rt = new PmdMorph
			{
				Name = PmdDocument.ReadPmdString(br, 20),
			};
			var count = br.ReadUInt32();

			rt.Kind = (PmdMorphKind)br.ReadByte();

			for (uint i = 0; i < count; i++)
			{
				var idx = (ushort)br.ReadUInt32();

				rt.Indices.Add(rt.Kind == PmdMorphKind.None ? doc.Vertices[idx] : morphBase.Indices[idx]);
				rt.Offsets.Add(new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() });
			}

			return rt;
		}

		public void Write(BinaryWriter bw, PmdIndexCache cache, Dictionary<PmdVertex, int> morphBaseIndices)
		{
			PmdDocument.WritePmdString(bw, this.Name, 20);
			bw.Write((uint)this.Offsets.Count);
			bw.Write((byte)this.Kind);
			this.Indices.Zip(this.Offsets, Tuple.Create).ForEach(_ =>
			{
				if (this.Kind == PmdMorphKind.None)
					bw.Write((uint)cache.Vertices[_.Item1]);
				else
					bw.Write((uint)morphBaseIndices[_.Item1]);

				_.Item2.ForEach(bw.Write);
			});
		}

		internal static PmdMorph CreateMorphBase(IEnumerable<PmdMorph> morphs)
		{
			var morphBaseIndices = morphs.SelectMany(_ => _.Indices).Distinct().ToList();

			return new PmdMorph
			{
				Name = "base",
				Indices = morphBaseIndices,
				Offsets = morphBaseIndices.Select(_ => _.Position).ToList(),
			};
		}
	}
}
