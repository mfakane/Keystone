using System.IO;

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

		public short[] RelatedBones
		{
			get;
			set;
		}

		public byte BoneWeight
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
			this.RelatedBones = new short[] { 0, 0 };
			this.BoneWeight = 100;
		}

		public static PmdVertex Parse(BinaryReader br)
		{
			return new PmdVertex
			{
				Position = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				Normal = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				UV = new[] {br.ReadSingle(), br.ReadSingle() },
				RelatedBones = new[] { br.ReadInt16(), br.ReadInt16() },
				BoneWeight = br.ReadByte(),
				NoEdge = br.ReadBoolean(),
			};
		}

		public void Write(BinaryWriter bw)
		{
			this.Position.ForEach(_ => bw.Write(_));
			this.Normal.ForEach(_ => bw.Write(_));
			this.UV.ForEach(_ => bw.Write(_));
			this.RelatedBones.ForEach(_ => bw.Write(_));
			bw.Write(this.BoneWeight);
			bw.Write(this.NoEdge);
		}
	}
}
