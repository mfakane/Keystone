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

		public string EnglishName
		{
			get;
			set;
		}

		public PmdBone ParentBone
		{
			get;
			set;
		}

		public PmdBone ConnectedToOrAssociatedBone
		{
			get;
			set;
		}

		public PmdBoneKind Kind
		{
			get;
			set;
		}

		public PmdBone IKParentOrAffectedBone
		{
			get;
			set;
		}

		public float AssosiationRate
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
			this.Position = new[] { 0f, 0, 0 };
		}

		public void Parse(BinaryReader br, PmdDocument doc)
		{
			this.Name = PmdDocument.ReadPmdString(br, 20);
			this.ParentBone = doc.GetBone(br.ReadInt16());
			this.ConnectedToOrAssociatedBone = doc.GetBone(br.ReadInt16());
			this.Kind = (PmdBoneKind)br.ReadByte();

			var parentOrRate = br.ReadInt16();

			if (this.Kind == PmdBoneKind.RotationAssociated)
				this.AssosiationRate = parentOrRate / 100f;
			else
				this.IKParentOrAffectedBone = doc.GetBone(parentOrRate);

			this.Position = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
		}

		public void Write(BinaryWriter bw, PmdIndexCache cache)
		{
			PmdDocument.WritePmdString(bw, this.Name, 20);
			bw.Write((short)(this.ParentBone == null ? -1 : cache.Bones[this.ParentBone]));
			bw.Write((short)(this.ConnectedToOrAssociatedBone == null ? -1 : cache.Bones[this.ConnectedToOrAssociatedBone]));
			bw.Write((byte)this.Kind);
			bw.Write((short)(this.Kind == PmdBoneKind.RotationAssociated ? this.AssosiationRate * 100 : this.IKParentOrAffectedBone == null ? -1 : cache.Bones[this.IKParentOrAffectedBone]));
			this.Position.ForEach(bw.Write);
		}
	}
}
