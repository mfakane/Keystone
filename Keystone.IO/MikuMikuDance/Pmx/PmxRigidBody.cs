using System.IO;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public class PmxRigidBody
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

		public int RelatedBone
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

		/// <summary>
		/// radians
		/// </summary>
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

		public PmxRigidKind Kind
		{
			get;
			set;
		}

		public PmxRigidBody()
		{
			this.Size = new[] { 0f, 0, 0 };
			this.Position = new[] { 0f, 0, 0 };
			this.Rotation = new[] { 0f, 0, 0 };
		}

		public static PmxRigidBody Parse(BinaryReader br, PmxDocument doc)
		{
			return new PmxRigidBody
			{
				Name = doc.ReadString(br),
				EnglishName = doc.ReadString(br),
				RelatedBone = doc.ReadIndex(br, PmxIndexKind.Bone),
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
				Kind = (PmxRigidKind)br.ReadByte(),
			};
		}

		public void Write(BinaryWriter bw, PmxDocument doc)
		{
			doc.WriteString(bw, this.Name);
			doc.WriteString(bw, this.EnglishName);
			doc.WriteIndex(bw, PmxIndexKind.Bone, this.RelatedBone);
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
