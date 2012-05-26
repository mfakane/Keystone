using System.IO;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public class PmdConstraint
	{
		public string Name
		{
			get;
			set;
		}

		public int RigidA
		{
			get;
			set;
		}

		public int RigidB
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

		public float[] LinearLowerLimit
		{
			get;
			set;
		}

		public float[] LinearUpperLimit
		{
			get;
			set;
		}

		public float[] AngularLowerLimit
		{
			get;
			set;
		}

		public float[] AngularUpperLimit
		{
			get;
			set;
		}

		public float[] LinearSpringStiffness
		{
			get;
			set;
		}

		public float[] AngularSpringStiffness
		{
			get;
			set;
		}

		public PmdConstraint()
		{
			this.Position = new[] { 0f, 0, 0 };
			this.Rotation = new[] { 0f, 0, 0 };
			this.LinearLowerLimit = new[] { 0f, 0, 0 };
			this.LinearUpperLimit = new[] { 0f, 0, 0 };
			this.AngularLowerLimit = new[] { 0f, 0, 0 };
			this.AngularUpperLimit = new[] { 0f, 0, 0 };
			this.LinearSpringStiffness = new[] { 0f, 0, 0 };
			this.AngularSpringStiffness = new[] { 0f, 0, 0 };
		}

		public static PmdConstraint Parse(BinaryReader br)
		{
			return new PmdConstraint
			{
				Name = PmdDocument.ReadPmdString(br, 20),
				RigidA = br.ReadInt32(),
				RigidB = br.ReadInt32(),
				Position = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				Rotation = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				LinearLowerLimit = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				LinearUpperLimit = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				AngularLowerLimit = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				AngularUpperLimit = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				LinearSpringStiffness = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				AngularSpringStiffness = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
			};
		}

		public void Write(BinaryWriter bw)
		{
			PmdDocument.WritePmdString(bw, this.Name, 20);
			bw.Write(this.RigidA);
			bw.Write(this.RigidB);
			this.Position.ForEach(bw.Write);
			this.Rotation.ForEach(bw.Write);
			this.LinearLowerLimit.ForEach(bw.Write);
			this.LinearUpperLimit.ForEach(bw.Write);
			this.AngularLowerLimit.ForEach(bw.Write);
			this.AngularUpperLimit.ForEach(bw.Write);
			this.LinearSpringStiffness.ForEach(bw.Write);
			this.AngularSpringStiffness.ForEach(bw.Write);
		}
	}
}
