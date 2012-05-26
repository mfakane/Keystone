using System;
using System.IO;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public abstract class PmxConstraint
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

		public abstract PmxConstraintKind Kind
		{
			get;
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

		/// <summary>
		/// radians
		/// </summary>
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

		/// <summary>
		/// radians
		/// </summary>
		public float[] AngularLowerLimit
		{
			get;
			set;
		}

		/// <summary>
		/// radians
		/// </summary>
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

		public PmxConstraint()
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

		public static PmxConstraint Parse(BinaryReader br, PmxDocument doc)
		{
			var name = doc.ReadString(br);
			var englishName = doc.ReadString(br);
			PmxConstraint rt;

			switch ((PmxConstraintKind)br.ReadByte())
			{
				case PmxConstraintKind.SpringSixDegreesOfFreedom:
					rt = new PmxSpringSixDegreesOfFreedomConstraint();

					break;
				default:
					throw new NotSupportedException();
			}

			rt.Name = name;
			rt.EnglishName = name;
			rt.Read(br, doc);

			return rt;
		}

		public abstract void Read(BinaryReader br, PmxDocument doc);

		public virtual void Write(BinaryWriter bw, PmxDocument doc)
		{
			doc.WriteString(bw, this.Name);
			doc.WriteString(bw, this.EnglishName);
			bw.Write((byte)this.Kind);
		}
	}

	public class PmxSpringSixDegreesOfFreedomConstraint : PmxConstraint
	{
		public override PmxConstraintKind Kind
		{
			get
			{
				return PmxConstraintKind.SpringSixDegreesOfFreedom;
			}
		}

		public override void Read(BinaryReader br, PmxDocument doc)
		{
			this.RigidA = doc.ReadIndex(br, PmxIndexKind.Rigid);
			this.RigidB = doc.ReadIndex(br, PmxIndexKind.Rigid);
			this.Position = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
			this.Rotation = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
			this.LinearLowerLimit = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
			this.LinearUpperLimit = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
			this.AngularLowerLimit = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
			this.AngularUpperLimit = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
			this.LinearSpringStiffness = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
			this.AngularSpringStiffness = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
		}

		public override void Write(BinaryWriter bw, PmxDocument doc)
		{
			base.Write(bw, doc);
			doc.WriteIndex(bw, PmxIndexKind.Rigid, this.RigidA);
			doc.WriteIndex(bw, PmxIndexKind.Rigid, this.RigidB);
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
