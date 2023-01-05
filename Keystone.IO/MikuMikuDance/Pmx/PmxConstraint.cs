using System;
using System.Numerics;

namespace Linearstar.Keystone.IO.MikuMikuDance.Pmx
{
    public abstract class PmxConstraint
    {
        public string Name { get; set; }

        public string EnglishName { get; set; }

        public abstract PmxConstraintKind Kind { get; }

        public PmxRigidBody? RigidA { get; set; }

        public PmxRigidBody? RigidB { get; set; }

        public Vector3 Position { get; set; }

        /// <summary>
        /// radians
        /// </summary>
        public Vector3 Rotation { get; set; }

        internal static PmxConstraint Parse(ref BufferReader br, PmxDocument doc)
        {
            var rt = new PmxSpringSixDegreesOfFreedomConstraint
            {
                Name = br.ReadString(doc.Header),
                EnglishName = br.ReadString(doc.Header),
            };
            var kind = (PmxConstraintKind)br.ReadByte();

            rt.Read(ref br, doc);

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

        internal abstract void Read(ref BufferReader br, PmxDocument doc);

        internal virtual void Write(ref BufferWriter bw, PmxDocument doc, PmxIndexCache cache)
        {
            bw.Write(this.Name, doc.Header);
            bw.Write(this.EnglishName, doc.Header);
            bw.Write((byte)(doc.Version < 2.1f ? PmxConstraintKind.SpringSixDegreesOfFreedom : this.Kind));
        }
    }

    public class PmxSpringSixDegreesOfFreedomConstraint : PmxConstraint
    {
        readonly PmxConstraintKind kind = PmxConstraintKind.SpringSixDegreesOfFreedom;

        public override PmxConstraintKind Kind => kind;

        public Vector3 LinearLowerLimit { get; set; }

        public Vector3 LinearUpperLimit { get; set; }

        /// <summary>
        /// radians
        /// </summary>
        public Vector3 AngularLowerLimit { get; set; }

        /// <summary>
        /// radians
        /// </summary>
        public Vector3 AngularUpperLimit { get; set; }

        public Vector3 LinearSpringStiffness { get; set; }

        public Vector3 AngularSpringStiffness { get; set; }

        public PmxSpringSixDegreesOfFreedomConstraint()
        {
        }

        internal PmxSpringSixDegreesOfFreedomConstraint(PmxConstraintKind kind)
            : this()
        {
            this.kind = kind;
        }

        internal override void Read(ref BufferReader br, PmxDocument doc)
        {
            this.RigidA = doc.ReadRigidBody(ref br);
            this.RigidB = doc.ReadRigidBody(ref br);
            this.Position = br.ReadVector3();
            this.Rotation = br.ReadVector3();
            this.LinearLowerLimit = br.ReadVector3();
            this.LinearUpperLimit = br.ReadVector3();
            this.AngularLowerLimit = br.ReadVector3();
            this.AngularUpperLimit = br.ReadVector3();
            this.LinearSpringStiffness = br.ReadVector3();
            this.AngularSpringStiffness = br.ReadVector3();
        }

        internal override void Write(ref BufferWriter bw, PmxDocument doc, PmxIndexCache cache)
        {
            base.Write(ref bw, doc, cache);
            bw.Write(this.RigidA, cache);
            bw.Write(this.RigidB, cache);
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

    public class PmxSixDegreesOfFreedomConstraint : PmxConstraint
    {
        public override PmxConstraintKind Kind => PmxConstraintKind.SixDegreesOfFreedom;

        public Vector3 LinearLowerLimit { get; set; }

        public Vector3 LinearUpperLimit { get; set; }

        /// <summary>
        /// radians
        /// </summary>
        public Vector3 AngularLowerLimit { get; set; }

        /// <summary>
        /// radians
        /// </summary>
        public Vector3 AngularUpperLimit { get; set; }

        internal override void Read(ref BufferReader br, PmxDocument doc)
        {
            throw new NotSupportedException("use casting from PmxSpringSixDegreesOfFreedomConstraint instead");
        }

        internal override void Write(ref BufferWriter bw, PmxDocument doc, PmxIndexCache cache)
        {
            ((PmxSpringSixDegreesOfFreedomConstraint)this).Write(ref bw, doc, cache);
        }

        public static explicit operator PmxSixDegreesOfFreedomConstraint(PmxSpringSixDegreesOfFreedomConstraint self) =>
            new()
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

        public static explicit operator PmxSpringSixDegreesOfFreedomConstraint(PmxSixDegreesOfFreedomConstraint self) =>
            new(self.Kind)
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

    /// <summary>
    /// (PMX 2.1)
    /// </summary>
    public class PmxPointToPointConstraint : PmxConstraint
    {
        public override PmxConstraintKind Kind => PmxConstraintKind.PointToPoint;

        internal override void Read(ref BufferReader br, PmxDocument doc)
        {
            throw new NotSupportedException("use casting from PmxSpringSixDegreesOfFreedomConstraint instead");
        }

        internal override void Write(ref BufferWriter bw, PmxDocument doc, PmxIndexCache cache)
        {
            ((PmxSpringSixDegreesOfFreedomConstraint)this).Write(ref bw, doc, cache);
        }

        public static explicit operator PmxPointToPointConstraint(PmxSpringSixDegreesOfFreedomConstraint self) =>
            new()
            {
                Name = self.Name,
                EnglishName = self.Name,
                RigidA = self.RigidA,
                RigidB = self.RigidB,
                Position = self.Position,
                Rotation = self.Rotation,
            };

        public static explicit operator PmxSpringSixDegreesOfFreedomConstraint(PmxPointToPointConstraint self) =>
            new(self.Kind)
            {
                Name = self.Name,
                EnglishName = self.Name,
                RigidA = self.RigidA,
                RigidB = self.RigidB,
                Position = self.Position,
                Rotation = self.Rotation,
            };
    }

    /// <summary>
    /// (PMX 2.1)
    /// </summary>
    public class PmxConeTwistConstraint : PmxConstraint
    {
        public override PmxConstraintKind Kind => PmxConstraintKind.ConeTwist;

        public float SwingSpan1 { get; set; }

        public float SwingSpan2 { get; set; }

        public float TwistSpan { get; set; }

        public float Softness { get; set; }

        public float BiasFactor { get; set; }

        public float RelaxationFactor { get; set; }

        public float Damping { get; set; }

        public float FixThreshold { get; set; }

        public bool IsMotorEnabled { get; set; }

        public float MaxMotorImpulse { get; set; }

        public Vector3 MotorTarget { get; set; }

        public PmxConeTwistConstraint()
        {
            this.FixThreshold = 1;
            this.Softness = 1;
            this.BiasFactor = 0.3f;
            this.RelaxationFactor = 1;
        }

        internal override void Read(ref BufferReader br, PmxDocument doc)
        {
            throw new NotSupportedException("use casting from PmxSpringSixDegreesOfFreedomConstraint instead");
        }

        internal override void Write(ref BufferWriter bw, PmxDocument doc, PmxIndexCache cache)
        {
            ((PmxSpringSixDegreesOfFreedomConstraint)this).Write(ref bw, doc, cache);
        }

        public static explicit operator PmxConeTwistConstraint(PmxSpringSixDegreesOfFreedomConstraint self) =>
            new()
            {
                Name = self.Name,
                EnglishName = self.Name,
                RigidA = self.RigidA,
                RigidB = self.RigidB,
                Position = self.Position,
                Rotation = self.Rotation,
                SwingSpan1 = self.AngularLowerLimit.Z,
                SwingSpan2 = self.AngularLowerLimit.Y,
                TwistSpan = self.AngularLowerLimit.X,
                Softness = self.LinearSpringStiffness.X,
                BiasFactor = self.LinearSpringStiffness.Y,
                RelaxationFactor = self.LinearSpringStiffness.Z,
                Damping = self.LinearLowerLimit.X,
                FixThreshold = self.LinearUpperLimit.X,
                IsMotorEnabled = self.LinearLowerLimit.Z == 1,
                MaxMotorImpulse = self.LinearUpperLimit.Z,
                MotorTarget = self.AngularSpringStiffness,
            };

        public static explicit operator PmxSpringSixDegreesOfFreedomConstraint(PmxConeTwistConstraint self) =>
            new(self.Kind)
            {
                Name = self.Name,
                EnglishName = self.Name,
                RigidA = self.RigidA,
                RigidB = self.RigidB,
                Position = self.Position,
                Rotation = self.Rotation,
                LinearLowerLimit = new(self.Damping, 0, self.IsMotorEnabled ? 1 : 0),
                LinearUpperLimit = new(self.FixThreshold, 0, self.MaxMotorImpulse),
                AngularLowerLimit = new(self.TwistSpan, self.SwingSpan2, self.SwingSpan1),
                LinearSpringStiffness = new(self.Softness, self.BiasFactor, self.RelaxationFactor),
                AngularSpringStiffness = self.MotorTarget,
            };
    }

    /// <summary>
    /// (PMX 2.1)
    /// </summary>
    public class PmxSliderConstraint : PmxConstraint
    {
        public override PmxConstraintKind Kind => PmxConstraintKind.Slider;

        public float LinearLowerLimit { get; set; }

        public float LinearUpperLimit { get; set; }

        public float AngularLowerLimit { get; set; }

        public float AngularUpperLimit { get; set; }

        public bool IsLinearMotorEnabled { get; set; }

        public float LinearMotorVelocity { get; set; }

        public float LinearMotorForce { get; set; }

        public bool IsAngularMotorEnabled { get; set; }

        public float AngularMotorVelocity { get; set; }

        public float AngularMotorForce { get; set; }

        public PmxSliderConstraint()
        {
            this.LinearUpperLimit = 1;
        }

        internal override void Read(ref BufferReader br, PmxDocument doc)
        {
            throw new NotSupportedException("use casting from PmxSpringSixDegreesOfFreedomConstraint instead");
        }

        internal override void Write(ref BufferWriter bw, PmxDocument doc, PmxIndexCache cache)
        {
            ((PmxSpringSixDegreesOfFreedomConstraint)this).Write(ref bw, doc, cache);
        }

        public static explicit operator PmxSliderConstraint(PmxSpringSixDegreesOfFreedomConstraint self) =>
            new()
            {
                Name = self.Name,
                EnglishName = self.Name,
                RigidA = self.RigidA,
                RigidB = self.RigidB,
                Position = self.Position,
                Rotation = self.Rotation,
                LinearLowerLimit = self.LinearLowerLimit.X,
                LinearUpperLimit = self.LinearUpperLimit.X,
                AngularLowerLimit = self.AngularLowerLimit.X,
                AngularUpperLimit = self.AngularUpperLimit.X,
                IsLinearMotorEnabled = self.LinearSpringStiffness.X == 1,
                LinearMotorVelocity = self.LinearSpringStiffness.Y,
                LinearMotorForce = self.LinearSpringStiffness.Z,
                IsAngularMotorEnabled = self.AngularSpringStiffness.X == 1,
                AngularMotorVelocity = self.AngularSpringStiffness.Y,
                AngularMotorForce = self.AngularSpringStiffness.Z,
            };

        public static explicit operator PmxSpringSixDegreesOfFreedomConstraint(PmxSliderConstraint self) =>
            new(self.Kind)
            {
                Name = self.Name,
                EnglishName = self.Name,
                RigidA = self.RigidA,
                RigidB = self.RigidB,
                Position = self.Position,
                Rotation = self.Rotation,
                LinearLowerLimit = new(self.LinearLowerLimit, 0, 0),
                LinearUpperLimit = new(self.LinearUpperLimit, 0, 0),
                AngularLowerLimit = new(self.AngularLowerLimit, 0, 0),
                AngularUpperLimit = new(self.AngularUpperLimit, 0, 0),
                LinearSpringStiffness = new(self.IsLinearMotorEnabled ? 1 : 0, self.LinearMotorVelocity, self.LinearMotorForce),
                AngularSpringStiffness = new(self.IsAngularMotorEnabled ? 1 : 0, self.AngularMotorVelocity, self.AngularMotorForce),
            };
    }

    /// <summary>
    /// (PMX 2.1)
    /// </summary>
    public class PmxHingeConstraint : PmxConstraint
    {
        public override PmxConstraintKind Kind => PmxConstraintKind.Hinge;

        public float AngularLowerLimit { get; set; }

        public float AngularUpperLimit { get; set; }

        public float Softness { get; set; }

        public float BiasFactor { get; set; }

        public float RelaxationFactor { get; set; }

        public bool IsAngularMotorEnabled { get; set; }

        public float AngularMotorVelocity { get; set; }

        public float AngularMotorForce { get; set; }

        public PmxHingeConstraint()
        {
            this.Softness = 0.9f;
            this.AngularMotorVelocity = 0.3f;
            this.RelaxationFactor = 1;
        }

        internal override void Read(ref BufferReader br, PmxDocument doc)
        {
            throw new NotSupportedException("use casting from PmxSpringSixDegreesOfFreedomConstraint instead");
        }

        internal override void Write(ref BufferWriter bw, PmxDocument doc, PmxIndexCache cache)
        {
            ((PmxSpringSixDegreesOfFreedomConstraint)this).Write(ref bw, doc, cache);
        }

        public static explicit operator PmxHingeConstraint(PmxSpringSixDegreesOfFreedomConstraint self) =>
            new()
            {
                Name = self.Name,
                EnglishName = self.Name,
                RigidA = self.RigidA,
                RigidB = self.RigidB,
                Position = self.Position,
                Rotation = self.Rotation,
                AngularLowerLimit = self.AngularLowerLimit.X,
                AngularUpperLimit = self.AngularUpperLimit.X,
                Softness = self.LinearSpringStiffness.X,
                BiasFactor = self.LinearSpringStiffness.Y,
                RelaxationFactor = self.LinearSpringStiffness.Z,
                IsAngularMotorEnabled = self.AngularSpringStiffness.X == 1,
                AngularMotorVelocity = self.AngularSpringStiffness.Y,
                AngularMotorForce = self.AngularSpringStiffness.Z,
            };

        public static explicit operator PmxSpringSixDegreesOfFreedomConstraint(PmxHingeConstraint self) =>
            new(self.Kind)
            {
                Name = self.Name,
                EnglishName = self.Name,
                RigidA = self.RigidA,
                RigidB = self.RigidB,
                Position = self.Position,
                Rotation = self.Rotation,
                AngularLowerLimit = new(self.AngularLowerLimit, 0, 0),
                AngularUpperLimit = new(self.AngularUpperLimit, 0, 0),
                LinearSpringStiffness = new(self.Softness, self.BiasFactor, self.RelaxationFactor),
                AngularSpringStiffness = new(self.IsAngularMotorEnabled ? 1 : 0, self.AngularMotorVelocity, self.AngularMotorForce),
            };
    }
}