using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public class PmdVertex
	{
		public float[] Position
		{
			get;
			set;
		}

		public float[] Normal
		{
			get;
			set;
		}

		public float[] UV
		{
			get;
			set;
		}

		public PmdBone[] RelatedBones
		{
			get;
			set;
		}

		public float BoneWeight
		{
			get;
			set;
		}

		public bool NoEdge
		{
			get;
			set;
		}

		public PmdVertex()
		{
			this.Position = new[] { 0f, 0, 0 };
			this.Normal = new[] { 0f, 0, 0 };
			this.UV = new[] { 0f, 0 };
			this.RelatedBones = new PmdBone[2];
			this.BoneWeight = 1;
		}

		public static PmdVertex Parse(BinaryReader br, PmdDocument doc)
		{
			return new PmdVertex
			{
				Position = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				Normal = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				UV = new[] {br.ReadSingle(), br.ReadSingle() },
				RelatedBones = new[] { doc.GetBone(br.ReadInt16()), doc.GetBone(br.ReadInt16()) },
				BoneWeight = br.ReadByte() / 100f,
				NoEdge = br.ReadBoolean(),
			};
		}

		public void Write(BinaryWriter bw, PmdIndexCache cache)
		{
			this.Position.ForEach(_ => bw.Write(_));
			this.Normal.ForEach(_ => bw.Write(_));
			this.UV.ForEach(_ => bw.Write(_));
			this.RelatedBones.Select(_ => _ == null ? -1 : cache.Bones[_]).ForEach(_ => bw.Write((short)_));
			bw.Write((byte)(this.BoneWeight * 100));
			bw.Write(this.NoEdge);
		}
	}
}
