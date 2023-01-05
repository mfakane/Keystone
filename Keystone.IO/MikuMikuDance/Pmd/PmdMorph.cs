using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Linearstar.Keystone.IO.MikuMikuDance.Pmd
{
	public class PmdMorph
	{
		public string Name { get; set; } = "モーフ";

		public string EnglishName { get; set; } = "Morph";

		public PmdMorphKind Kind { get; set; }

		public IList<PmdVertex> Indices { get; set; } = new List<PmdVertex>();

		public IList<Vector3> Offsets { get; set; } = new List<Vector3>();

		internal static PmdMorph Parse(ref BufferReader br, PmdDocument doc, PmdMorph morphBase)
		{
			var rt = new PmdMorph
			{
				Name = br.ReadString(20),
			};
			var count = br.ReadUInt32();

			rt.Kind = (PmdMorphKind)br.ReadByte();

			for (uint i = 0; i < count; i++)
			{
				var idx = (ushort)br.ReadUInt32();

				rt.Indices.Add(rt.Kind == PmdMorphKind.None ? doc.Vertices[idx] : morphBase.Indices[idx]);
				rt.Offsets.Add(br.ReadVector3());
			}

			return rt;
		}

		internal void Write(ref BufferWriter bw, PmdIndexCache cache, Dictionary<PmdVertex, int> morphBaseIndices)
		{
			bw.Write(this.Name, 20);
			bw.Write((uint)this.Offsets.Count);
			bw.Write((byte)this.Kind);

			var count = this.Offsets.Count;
			for (var i = 0; i < count; i++)
			{
				var vertex = this.Indices[i];
				
				if (this.Kind == PmdMorphKind.None)
					bw.Write(cache[vertex]);
				else
					bw.Write((uint)morphBaseIndices[vertex]);

				bw.Write(this.Offsets[i]);
			}
		}

		internal static PmdMorph CreateMorphBase(IEnumerable<PmdMorph> morphs)
		{
			var morphBaseIndices = morphs.SelectMany(x => x.Indices).Distinct().ToList();

			return new PmdMorph
			{
				Name = "base",
				Indices = morphBaseIndices,
				Offsets = morphBaseIndices.Select(x => x.Position).ToList(),
			};
		}
	}
}
