using System.IO;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public class PmdRigidBody
	{
		public string Name
		{
			get;
			set;
		}

		public short RelatedBone
		{
			get;
			set;
		}

		public byte Group
		{
			get;
			set;
		}

		public PmdRigidGroups CollidableGroups
		{
			get;
			set;
		}

		public PmdRigidShape Shape
		{
			get;
			set;
		}

		public float[] Size
		{
			get;
			set;
		}

		public float[] Position
		{
			get;
			set;
		}

		public float[] Rotation
		{
			get;
			set;
		}

		public float Mass
		{
			get;
			set;
		}

		public float LinearDamping
		{
			get;
			set;
		}

		public float AngularDamping
		{
			get;
			set;
		}

		public float Restitution
		{
			get;
			set;
		}

		public float Friction
		{
			get;
			set;
		}

		public PmdRigidKind Kind
		{
			get;
			set;
		}

		public PmdRigidBody()
		{
			this.Size = new[] { 0f, 0, 0 };
			this.Position = new[] { 0f, 0, 0 };
			this.Rotation = new[] { 0f, 0, 0 };
		}

		public static PmdRigidBody Parse(BinaryReader br)
		{
			return new PmdRigidBody
			{
				Name = PmdDocument.ReadPmdString(br, 20),
				RelatedBone = br.ReadInt16(),
				Group = br.ReadByte(),
				CollidableGroups = (PmdRigidGroups)br.ReadUInt16(),
				Shape = (PmdRigidShape)br.ReadByte(),
				Size = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				Position = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				Rotation = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				Mass = br.ReadSingle(),
				LinearDamping = br.ReadSingle(),
				AngularDamping = br.ReadSingle(),
				Restitution = br.ReadSingle(),
				Friction = br.ReadSingle(),
				Kind = (PmdRigidKind)br.ReadByte(),
			};
		}

		public void Write(BinaryWriter bw)
		{
			PmdDocument.WritePmdString(bw, this.Name, 20);
			bw.Write(this.RelatedBone);
			bw.Write(this.Group);
			bw.Write((ushort)this.CollidableGroups);
			bw.Write((byte)this.Shape);
			this.Size.ForEach(bw.Write);
			this.Position.ForEach(bw.Write);
			this.Rotation.ForEach(bw.Write);
			bw.Write(this.Mass);
			bw.Write(this.LinearDamping);
			bw.Write(this.AngularDamping);
			bw.Write(this.Restitution);
			bw.Write(this.Friction);
			bw.Write((byte)this.Kind);
		}
	}
}