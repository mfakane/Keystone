using System;
using System.IO;
using System.Linq;

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

		public PmxConstraint()
		{
			this.Position = new[] { 0f, 0, 0 };
			this.Rotation = new[] { 0f, 0, 0 };
		}

		public static PmxConstraint Parse(BinaryReader br, PmxDocument doc)
		{
			var rt = new PmxSpringSixDegreesOfFreedomConstraint
			{
				Name = doc.ReadString(br),
				EnglishName = doc.ReadString(br),
			};
			var kind = (PmxConstraintKind)br.ReadByte();

			rt.Read(br, doc);

			switch (kind)
			{
				case PmxConstraintKind.SpringSixDegreesOfFreedom:
					return rt;
				case PmxConstraintKind.SixDegreesOfFreedom:
					return (PmxSixDegreesOfFreedomConstraint)rt;
				case PmxConstraintKind.PointToPoint:
					return (PmxPointToPointConstraint)rt;
				case PmxConstraintKind.ConeTwist:
					return (PmxConeTwistConstraint)rt;
				case PmxConstraintKind.Slider:
					return (PmxSliderConstraint)rt;
				case PmxConstraintKind.Hinge:
					return (PmxHingeConstraint)rt;
				default:
					throw new NotSupportedException();
			}
		}

		public abstract void Read(BinaryReader br, PmxDocument doc);

		public virtual void Write(BinaryWriter bw, PmxDocument doc)
		{
			doc.WriteString(bw, this.Name);
			doc.WriteString(bw, this.EnglishName);
			bw.Write((byte)(doc.Version < 2.1f ? PmxConstraintKind.SpringSixDegreesOfFreedom : this.Kind));
		}
	}

	public class PmxSpringSixDegreesOfFreedomConstraint : PmxConstraint
	{
		readonly PmxConstraintKind kind = PmxConstraintKind.SpringSixDegreesOfFreedom;

		public override PmxConstraintKind Kind
		{
			get
			{
				return kind;
			}
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

		public PmxSpringSixDegreesOfFreedomConstraint()
		{
			this.LinearLowerLimit = new[] { 0f, 0, 0 };
			this.LinearUpperLimit = new[] { 0f, 0, 0 };
			this.AngularLowerLimit = new[] { 0f, 0, 0 };
			this.AngularUpperLimit = new[] { 0f, 0, 0 };
			this.LinearSpringStiffness = new[] { 0f, 0, 0 };
			this.AngularSpringStiffness = new[] { 0f, 0, 0 };
		}

		internal PmxSpringSixDegreesOfFreedomConstraint(PmxConstraintKind kind)
			: this()
		{
			this.kind = kind;
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

	public class PmxSixDegreesOfFreedomConstraint : PmxConstraint
	{
		public override PmxConstraintKind Kind
		{
			get
			{
				return PmxConstraintKind.SixDegreesOfFreedom;
			}
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

		public PmxSixDegreesOfFreedomConstraint()
		{
			this.LinearLowerLimit = new[] { 0f, 0, 0 };
			this.LinearUpperLimit = new[] { 0f, 0, 0 };
			this.AngularLowerLimit = new[] { 0f, 0, 0 };
			this.AngularUpperLimit = new[] { 0f, 0, 0 };
		}

		public override void Read(BinaryReader br, PmxDocument doc)
		{
			throw new NotSupportedException("use casting from PmxSpringSixDegreesOfFreedomConstraint instead");
		}

		public override void Write(BinaryWriter bw, PmxDocument doc)
		{
			((PmxSpringSixDegreesOfFreedomConstraint)this).Write(bw, doc);
		}

		public static explicit operator PmxSixDegreesOfFreedomConstraint(PmxSpringSixDegreesOfFreedomConstraint self)
		{
			return new PmxSixDegreesOfFreedomConstraint
			{
				Name = self.Name,
				EnglishName = self.Name,
				RigidA = self.RigidA,
				RigidB = self.RigidB,
				Position = self.Position,
				Rotation = self.Rotation,
				LinearLowerLimit = self.LinearLowerLimit,
				LinearUpperLimit = self.LinearUpperLimit,
				AngularLowerLimit = self.AngularLowerLimit,
				AngularUpperLimit = self.AngularUpperLimit,
			};
		}

		public static explicit operator PmxSpringSixDegreesOfFreedomConstraint(PmxSixDegreesOfFreedomConstraint self)
		{
			return new PmxSpringSixDegreesOfFreedomConstraint(self.Kind)
			{
				Name = self.Name,
				EnglishName = self.Name,
				RigidA = self.RigidA,
				RigidB = self.RigidB,
				Position = self.Position,
				Rotation = self.Rotation,
				LinearLowerLimit = self.LinearLowerLimit,
				LinearUpperLimit = self.LinearUpperLimit,
				AngularLowerLimit = self.AngularLowerLimit,
				AngularUpperLimit = self.AngularUpperLimit,
			};
		}
	}

	/// <summary>
	/// (PMX 2.1)
	/// </summary>
	public class PmxPointToPointConstraint : PmxConstraint
	{
		public override PmxConstraintKind Kind
		{
			get
			{
				return PmxConstraintKind.PointToPoint;
			}
		}

		public override void Read(BinaryReader br, PmxDocument doc)
		{
			throw new NotSupportedException("use casting from PmxSpringSixDegreesOfFreedomConstraint instead");
		}

		public override void Write(BinaryWriter bw, PmxDocument doc)
		{
			((PmxSpringSixDegreesOfFreedomConstraint)this).Write(bw, doc);
		}

		public static explicit operator PmxPointToPointConstraint(PmxSpringSixDegreesOfFreedomConstraint self)
		{
			return new PmxPointToPointConstraint
			{
				Name = self.Name,
				EnglishName = self.Name,
				RigidA = self.RigidA,
				RigidB = self.RigidB,
				Position = self.Position,
				Rotation = self.Rotation,
			};
		}

		public static explicit operator PmxSpringSixDegreesOfFreedomConstraint(PmxPointToPointConstraint self)
		{
			return new PmxSpringSixDegreesOfFreedomConstraint(self.Kind)
			{
				Name = self.Name,
				EnglishName = self.Name,
				RigidA = self.RigidA,
				RigidB = self.RigidB,
				Position = self.Position,
				Rotation = self.Rotation,
			};
		}
	}

	/// <summary>
	/// (PMX 2.1)
	/// </summary>
	public class PmxConeTwistConstraint : PmxConstraint
	{
		public override PmxConstraintKind Kind
		{
			get
			{
				return PmxConstraintKind.ConeTwist;
			}
		}

		public float SwingSpan1
		{
			get;
			set;
		}

		public float SwingSpan2
		{
			get;
			set;
		}

		public float TwistSpan
		{
			get;
			set;
		}

		public float Softness
		{
			get;
			set;
		}

		public float BiasFactor
		{
			get;
			set;
		}

		public float RelaxationFactor
		{
			get;
			set;
		}

		public float Damping
		{
			get;
			set;
		}

		public float FixThreshold
		{
			get;
			set;
		}

		public bool IsMotorEnabled
		{
			get;
			set;
		}

		public float MaxMotorImpulse
		{
			get;
			set;
		}

		/// <summary>
		/// Vector3
		/// </summary>
		public float[] MotorTarget
		{
			get;
			set;
		}

		public PmxConeTwistConstraint()
		{
			this.FixThreshold = 1;
			this.Softness = 1;
			this.BiasFactor = 0.3f;
			this.RelaxationFactor = 1;
		}

		public override void Read(BinaryReader br, PmxDocument doc)
		{
			throw new NotSupportedException("use casting from PmxSpringSixDegreesOfFreedomConstraint instead");
		}

		public override void Write(BinaryWriter bw, PmxDocument doc)
		{
			((PmxSpringSixDegreesOfFreedomConstraint)this).Write(bw, doc);
		}

		public static explicit operator PmxConeTwistConstraint(PmxSpringSixDegreesOfFreedomConstraint self)
		{
			return new PmxConeTwistConstraint
			{
				Name = self.Name,
				EnglishName = self.Name,
				RigidA = self.RigidA,
				RigidB = self.RigidB,
				Position = self.Position,
				Rotation = self.Rotation,
				SwingSpan1 = self.AngularLowerLimit[2],
				SwingSpan2 = self.AngularLowerLimit[1],
				TwistSpan = self.AngularLowerLimit[0],
				Softness = self.LinearSpringStiffness[0],
				BiasFactor = self.LinearSpringStiffness[1],
				RelaxationFactor = self.LinearSpringStiffness[2],
				Damping = self.LinearLowerLimit[0],
				FixThreshold = self.LinearUpperLimit[0],
				IsMotorEnabled = self.LinearLowerLimit[2] == 1,
				MaxMotorImpulse = self.LinearUpperLimit[2],
				MotorTarget = self.AngularSpringStiffness,
			};
		}

		public static explicit operator PmxSpringSixDegreesOfFreedomConstraint(PmxConeTwistConstraint self)
		{
			return new PmxSpringSixDegreesOfFreedomConstraint(self.Kind)
			{
				Name = self.Name,
				EnglishName = self.Name,
				RigidA = self.RigidA,
				RigidB = self.RigidB,
				Position = self.Position,
				Rotation = self.Rotation,
				LinearLowerLimit = new[] { self.Damping, 0, self.IsMotorEnabled ? 1 : 0 },
				LinearUpperLimit = new[] { self.FixThreshold, 0, self.MaxMotorImpulse },
				AngularLowerLimit = new[] { self.TwistSpan, self.SwingSpan2, self.SwingSpan1 },
				LinearSpringStiffness = new[] { self.Softness, self.BiasFactor, self.RelaxationFactor },
				AngularSpringStiffness = self.MotorTarget,
			};
		}
	}

	/// <summary>
	/// (PMX 2.1)
	/// </summary>
	public class PmxSliderConstraint : PmxConstraint
	{
		public override PmxConstraintKind Kind
		{
			get
			{
				return PmxConstraintKind.Slider;
			}
		}

		public float LinearLowerLimit
		{
			get;
			set;
		}

		public float LinearUpperLimit
		{
			get;
			set;
		}

		public float AngularLowerLimit
		{
			get;
			set;
		}

		public float AngularUpperLimit
		{
			get;
			set;
		}

		public bool IsLinearMotorEnabled
		{
			get;
			set;
		}

		public float LinearMotorVelocity
		{
			get;
			set;
		}

		public float LinearMotorForce
		{
			get;
			set;
		}

		public bool IsAngularMotorEnabled
		{
			get;
			set;
		}

		public float AngularMotorVelocity
		{
			get;
			set;
		}

		public float AngularMotorForce
		{
			get;
			set;
		}

		public PmxSliderConstraint()
		{
			this.LinearUpperLimit = 1;
		}

		public override void Read(BinaryReader br, PmxDocument doc)
		{
			throw new NotSupportedException("use casting from PmxSpringSixDegreesOfFreedomConstraint instead");
		}

		public override void Write(BinaryWriter bw, PmxDocument doc)
		{
			((PmxSpringSixDegreesOfFreedomConstraint)this).Write(bw, doc);
		}

		public static explicit operator PmxSliderConstraint(PmxSpringSixDegreesOfFreedomConstraint self)
		{
			return new PmxSliderConstraint
			{
				Name = self.Name,
				EnglishName = self.Name,
				RigidA = self.RigidA,
				RigidB = self.RigidB,
				Position = self.Position,
				Rotation = self.Rotation,
				LinearLowerLimit = self.LinearLowerLimit[0],
				LinearUpperLimit = self.LinearUpperLimit[0],
				AngularLowerLimit = self.AngularLowerLimit[0],
				AngularUpperLimit = self.AngularUpperLimit[0],
				IsLinearMotorEnabled = self.LinearSpringStiffness[0] == 1,
				LinearMotorVelocity = self.LinearSpringStiffness[1],
				LinearMotorForce = self.LinearSpringStiffness[2],
				IsAngularMotorEnabled = self.AngularSpringStiffness[0] == 1,
				AngularMotorVelocity = self.AngularSpringStiffness[1],
				AngularMotorForce = self.AngularSpringStiffness[2],
			};
		}

		public static explicit operator PmxSpringSixDegreesOfFreedomConstraint(PmxSliderConstraint self)
		{
			return new PmxSpringSixDegreesOfFreedomConstraint(self.Kind)
			{
				Name = self.Name,
				EnglishName = self.Name,
				RigidA = self.RigidA,
				RigidB = self.RigidB,
				Position = self.Position,
				Rotation = self.Rotation,
				LinearLowerLimit = new[] { self.LinearLowerLimit, 0, 0 },
				LinearUpperLimit = new[] { self.LinearUpperLimit, 0, 0 },
				AngularLowerLimit = new[] { self.AngularLowerLimit, 0, 0 },
				AngularUpperLimit = new[] { self.AngularUpperLimit, 0, 0 },
				LinearSpringStiffness = new[] { self.IsLinearMotorEnabled ? 1 : 0, self.LinearMotorVelocity, self.LinearMotorForce },
				AngularSpringStiffness = new[] { self.IsAngularMotorEnabled ? 1 : 0, self.AngularMotorVelocity, self.AngularMotorForce },
			};
		}
	}

	/// <summary>
	/// (PMX 2.1)
	/// </summary>
	public class PmxHingeConstraint : PmxConstraint
	{
		public override PmxConstraintKind Kind
		{
			get
			{
				return PmxConstraintKind.Hinge;
			}
		}

		public float AngularLowerLimit
		{
			get;
			set;
		}

		public float AngularUpperLimit
		{
			get;
			set;
		}

		public float Softness
		{
			get;
			set;
		}

		public float BiasFactor
		{
			get;
			set;
		}

		public float RelaxationFactor
		{
			get;
			set;
		}

		public bool IsAngularMotorEnabled
		{
			get;
			set;
		}

		public float AngularMotorVelocity
		{
			get;
			set;
		}

		public float AngularMotorForce
		{
			get;
			set;
		}

		public PmxHingeConstraint()
		{
			this.Softness = 0.9f;
			this.AngularMotorVelocity = 0.3f;
			this.RelaxationFactor = 1;
		}

		public override void Read(BinaryReader br, PmxDocument doc)
		{
			throw new NotSupportedException("use casting from PmxSpringSixDegreesOfFreedomConstraint instead");
		}

		public override void Write(BinaryWriter bw, PmxDocument doc)
		{
			((PmxSpringSixDegreesOfFreedomConstraint)this).Write(bw, doc);
		}

		public static explicit operator PmxHingeConstraint(PmxSpringSixDegreesOfFreedomConstraint self)
		{
			return new PmxHingeConstraint
			{
				Name = self.Name,
				EnglishName = self.Name,
				RigidA = self.RigidA,
				RigidB = self.RigidB,
				Position = self.Position,
				Rotation = self.Rotation,
				AngularLowerLimit = self.AngularLowerLimit[0],
				AngularUpperLimit = self.AngularUpperLimit[0],
				Softness = self.LinearSpringStiffness[0],
				BiasFactor = self.LinearSpringStiffness[1],
				RelaxationFactor = self.LinearSpringStiffness[2],
				IsAngularMotorEnabled = self.AngularSpringStiffness[0] == 1,
				AngularMotorVelocity = self.AngularSpringStiffness[1],
				AngularMotorForce = self.AngularSpringStiffness[2],
			};
		}

		public static explicit operator PmxSpringSixDegreesOfFreedomConstraint(PmxHingeConstraint self)
		{
			return new PmxSpringSixDegreesOfFreedomConstraint(self.Kind)
			{
				Name = self.Name,
				EnglishName = self.Name,
				RigidA = self.RigidA,
				RigidB = self.RigidB,
				Position = self.Position,
				Rotation = self.Rotation,
				AngularLowerLimit = new[] { self.AngularLowerLimit, 0, 0 },
				AngularUpperLimit = new[] { self.AngularUpperLimit, 0, 0 },
				LinearSpringStiffness = new[] { self.Softness, self.BiasFactor, self.RelaxationFactor },
				AngularSpringStiffness = new[] { self.IsAngularMotorEnabled ? 1 : 0, self.AngularMotorVelocity, self.AngularMotorForce },
			};
		}
	}
}
