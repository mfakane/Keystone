using System.Numerics;

namespace Linearstar.Keystone.IO.MikuMikuDance.Pmd
{
	public class PmdConstraint
	{
		public string Name { get; set; }

		public int RigidA { get; set; }

		public int RigidB { get; set; }

		public Vector3 Position { get; set; }

		public Vector3 Rotation { get; set; }

		public Vector3 LinearLowerLimit { get; set; }

		public Vector3 LinearUpperLimit { get; set; }

		public Vector3 AngularLowerLimit { get; set; }

		public Vector3 AngularUpperLimit { get; set; }

		public Vector3 LinearSpringStiffness { get; set; }

		public Vector3 AngularSpringStiffness { get; set; }

		internal static PmdConstraint Parse(ref BufferReader br) =>
			new()
			{
				Name = br.ReadString(20),
				RigidA = br.ReadInt32(),
				RigidB = br.ReadInt32(),
				Position = br.ReadVector3(),
				Rotation = br.ReadVector3(),
				LinearLowerLimit = br.ReadVector3(),
				LinearUpperLimit = br.ReadVector3(),
				AngularLowerLimit = br.ReadVector3(),
				AngularUpperLimit = br.ReadVector3(),
				LinearSpringStiffness = br.ReadVector3(),
				AngularSpringStiffness = br.ReadVector3(),
			};

		internal void Write(ref BufferWriter bw)
		{
			bw.Write(this.Name, 20);
			bw.Write(this.RigidA);
			bw.Write(this.RigidB);
			bw.Write(this.Position);
			bw.Write(this.Rotation);
			bw.Write(this.LinearLowerLimit);
			bw.Write(this.LinearUpperLimit);
			bw.Write(this.AngularLowerLimit);
			bw.Write(this.AngularUpperLimit);
			bw.Write(this.LinearSpringStiffness);
			bw.Write(this.AngularSpringStiffness);
		}
	}
}
