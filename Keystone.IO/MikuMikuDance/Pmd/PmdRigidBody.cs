using System.Numerics;

namespace Linearstar.Keystone.IO.MikuMikuDance.Pmd
{
	public class PmdRigidBody
	{
		public string Name { get; set; }

		public short RelatedBone { get; set; }

		public byte Group { get; set; }

		public PmdRigidGroups CollidableGroups { get; set; }

		public PmdRigidShape Shape { get; set; }

		public Vector3 Size { get; set; }

		public Vector3 Position { get; set; } 

		public Vector3 Rotation { get; set; }

		public float Mass { get; set; }

		public float LinearDamping { get; set; }

		public float AngularDamping { get; set; }

		public float Restitution { get; set; }

		public float Friction { get; set; }

		public PmdRigidKind Kind { get; set; }

		internal static PmdRigidBody Parse(ref BufferReader br) =>
			new()
			{
				Name = br.ReadString(20),
				RelatedBone = br.ReadInt16(),
				Group = br.ReadByte(),
				CollidableGroups = (PmdRigidGroups)br.ReadUInt16(),
				Shape = (PmdRigidShape)br.ReadByte(),
				Size = br.ReadVector3(),
				Position = br.ReadVector3(),
				Rotation = br.ReadVector3(),
				Mass = br.ReadSingle(),
				LinearDamping = br.ReadSingle(),
				AngularDamping = br.ReadSingle(),
				Restitution = br.ReadSingle(),
				Friction = br.ReadSingle(),
				Kind = (PmdRigidKind)br.ReadByte(),
			};

		internal void Write(ref BufferWriter bw)
		{
			bw.Write(this.Name, 20);
			bw.Write(this.RelatedBone);
			bw.Write(this.Group);
			bw.Write((ushort)this.CollidableGroups);
			bw.Write((byte)this.Shape);
			bw.Write(this.Size);
			bw.Write(this.Position);
			bw.Write(this.Rotation);
			bw.Write(this.Mass);
			bw.Write(this.LinearDamping);
			bw.Write(this.AngularDamping);
			bw.Write(this.Restitution);
			bw.Write(this.Friction);
			bw.Write((byte)this.Kind);
		}
	}
}