using System.Numerics;

namespace Linearstar.Keystone.IO.MikuMikuDance.Pmd
{
	public class PmdVertex
	{
		public Vector3 Position { get; set; }

		public Vector3 Normal { get; set; }

		public Vector2 UV { get; set; }

		public PmdBone? BoneA { get; set; }
		
		public PmdBone? BoneB { get; set; }

		public float BoneWeight { get; set; } = 1;

		public bool NoEdge { get; set; }

		internal static PmdVertex Parse(ref BufferReader br, PmdDocument doc)
		{
			return new PmdVertex
			{
				Position = br.ReadVector3(),
				Normal = br.ReadVector3(),
				UV = br.ReadVector2(),
				BoneA = doc.GetBone(br.ReadInt16()),
				BoneB = doc.GetBone(br.ReadInt16()),
				BoneWeight = br.ReadByte() / 100f,
				NoEdge = br.ReadBoolean(),
			};
		}

		internal void Write(ref BufferWriter bw, PmdIndexCache cache)
		{
			bw.Write(this.Position);
			bw.Write(this.Normal);
			bw.Write(this.UV);
			bw.Write(this.BoneA, cache);
			bw.Write(this.BoneB, cache);
			bw.Write((byte)(this.BoneWeight * 100));
			bw.Write(this.NoEdge);
		}
	}
}
