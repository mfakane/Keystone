using System.IO;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public class PmdBone
	{
		public string Name
		{
			get;
			set;
		}

		public short ParentBone
		{
			get;
			set;
		}

		public short ConnectedToOrAssociatedBone
		{
			get;
			set;
		}

		public PmdBoneKind Kind
		{
			get;
			set;
		}

		public short IKParentBoneOrAssociationRate
		{
			get;
			set;
		}

		public float[] Position
		{
			get;
			set;
		}

		public PmdBone()
		{
			this.ParentBone = this.ConnectedToOrAssociatedBone = -1;
			this.Position = new[] { 0f, 0, 0 };
		}

		public static PmdBone Parse(BinaryReader br)
		{
			return new PmdBone
			{
				Name = PmdDocument.ReadPmdString(br, 20),
				ParentBone = br.ReadInt16(),
				ConnectedToOrAssociatedBone = br.ReadInt16(),
				Kind = (PmdBoneKind)br.ReadByte(),
				IKParentBoneOrAssociationRate = br.ReadInt16(),
				Position = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
			};
		}

		public void Write(BinaryWriter bw)
		{
			PmdDocument.WritePmdString(bw, this.Name, 20);
			bw.Write(this.ParentBone);
			bw.Write(this.ConnectedToOrAssociatedBone);
			bw.Write((byte)this.Kind);
			bw.Write(this.IKParentBoneOrAssociationRate);
			this.Position.ForEach(bw.Write);
		}
	}
}
