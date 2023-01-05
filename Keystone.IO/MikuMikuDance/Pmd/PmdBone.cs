using System.Numerics;

namespace Linearstar.Keystone.IO.MikuMikuDance.Pmd
{
	public class PmdBone
	{
		public string Name { get; set; } = "ボーン";

		public string EnglishName { get; set; } = "Bone";

		public PmdBone? ParentBone { get; set; }

		public PmdBone? ConnectedToOrAssociatedBone { get; set; }

		public PmdBoneKind Kind { get; set; }

		public PmdBone? IKParentOrAffectedBone { get; set; }

		public float AssociationRate { get; set; }

		public Vector3 Position { get; set; }
		
		internal void Parse(ref BufferReader br, PmdDocument doc)
		{
			this.Name = br.ReadString(20);
			this.ParentBone = doc.GetBone(br.ReadInt16());
			this.ConnectedToOrAssociatedBone = doc.GetBone(br.ReadInt16());
			this.Kind = (PmdBoneKind)br.ReadByte();

			var parentOrRate = br.ReadInt16();

			if (this.Kind == PmdBoneKind.RotationAssociated)
				this.AssociationRate = parentOrRate / 100f;
			else
				this.IKParentOrAffectedBone = doc.GetBone(parentOrRate);

			this.Position = br.ReadVector3();
		}

		internal void Write(ref BufferWriter bw, PmdIndexCache cache)
		{
			bw.Write(this.Name, 20);
			bw.Write(this.ParentBone, cache);
			bw.Write(this.ConnectedToOrAssociatedBone, cache);
			bw.Write((byte)this.Kind);
			bw.Write((short)(this.Kind == PmdBoneKind.RotationAssociated ? this.AssociationRate * 100 : this.IKParentOrAffectedBone == null ? -1 : cache[this.IKParentOrAffectedBone]));
			bw.Write(this.Position);
		}
	}
}
