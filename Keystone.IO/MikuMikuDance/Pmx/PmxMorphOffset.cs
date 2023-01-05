using System;
using System.Numerics;

namespace Linearstar.Keystone.IO.MikuMikuDance.Pmx
{
    public abstract class PmxMorphOffset
    {
        internal abstract void Read(ref BufferReader br, PmxDocument doc);
        internal abstract void Write(ref BufferWriter bw, PmxDocument doc, PmxIndexCache cache);

        internal static PmxMorphOffset Parse(ref BufferReader br, PmxDocument doc, PmxMorphKind kind)
        {
            PmxMorphOffset rt;

            switch (kind)
            {
                case PmxMorphKind.Group:
                    rt = new PmxGroupMorphOffset();

                    break;
                case PmxMorphKind.Vertex:
                    rt = new PmxVertexMorphOffset();

                    break;
                case PmxMorphKind.Bone:
                    rt = new PmxBoneMorphOffset();

                    break;
                case PmxMorphKind.UV:
                    rt = new PmxUVMorphOffset();

                    break;
                case PmxMorphKind.AdditionalUV1:
                    rt = new PmxUVMorphOffset();

                    break;
                case PmxMorphKind.AdditionalUV2:
                    rt = new PmxUVMorphOffset();

                    break;
                case PmxMorphKind.AdditionalUV3:
                    rt = new PmxUVMorphOffset();

                    break;
                case PmxMorphKind.AdditionalUV4:
                    rt = new PmxUVMorphOffset();

                    break;
                case PmxMorphKind.Material:
                    rt = new PmxMaterialMorphOffset();

                    break;
                case PmxMorphKind.Flip:
                    rt = new PmxFlipMorphOffset();

                    break;
                case PmxMorphKind.Impulse:
                    rt = new PmxImpulseMorphOffset();

                    break;
                default:
                    throw new ArgumentException();
            }

            rt.Read(ref br, doc);

            return rt;
        }
    }

    public class PmxVertexMorphOffset : PmxMorphOffset
    {
        public PmxVertex? Vertex { get; set; }

        public Vector3 Offset { get; set; }

        internal override void Read(ref BufferReader br, PmxDocument doc)
        {
            this.Vertex = doc.ReadVertex(ref br);
            this.Offset = br.ReadVector3();
        }

        internal override void Write(ref BufferWriter bw, PmxDocument doc, PmxIndexCache cache)
        {
            bw.Write(this.Vertex, cache);
            bw.Write(this.Offset);
        }
    }

    public class PmxUVMorphOffset : PmxMorphOffset
    {
        public PmxVertex? Vertex { get; set; }

        public Vector4 Offset { get; set; }

        internal override void Read(ref BufferReader br, PmxDocument doc)
        {
            this.Vertex = doc.ReadVertex(ref br);
            this.Offset = br.ReadVector4();
        }

        internal override void Write(ref BufferWriter bw, PmxDocument doc, PmxIndexCache cache)
        {
            bw.Write(this.Vertex, cache);
            bw.Write(this.Offset);
        }
    }

    public class PmxBoneMorphOffset : PmxMorphOffset
    {
        public PmxBone? Bone { get; set; }

        public Vector3 MovementOffset { get; set; }

        public Quaternion RotationOffset { get; set; }

        internal override void Read(ref BufferReader br, PmxDocument doc)
        {
            this.Bone = doc.ReadBone(ref br);
            this.MovementOffset = br.ReadVector3();
            this.RotationOffset = br.ReadQuaternion();
        }

        internal override void Write(ref BufferWriter bw, PmxDocument doc, PmxIndexCache cache)
        {
            bw.Write(this.Bone, cache);
            bw.Write(this.MovementOffset);
            bw.Write(this.RotationOffset);
        }
    }

    public class PmxMaterialMorphOffset : PmxMorphOffset
    {
        public PmxMaterial? Material { get; set; }

        public PmxMaterialMorphKind Kind { get; set; }

        /// <summary>
        /// r, g, b, a
        /// </summary>
        public Color4 Diffuse { get; set; }

        public Color3 Specular { get; set; }

        public float Power { get; set; }

        public Color3 Ambient { get; set; }

        /// <summary>
        /// r, g, b, a
        /// </summary>
        public Color4 EdgeColor { get; set; }

        public float EdgeSize { get; set; }

        /// <summary>
        /// r, g, b, a
        /// </summary>
        public Color4 Texture { get; set; }

        /// <summary>
        /// r, g, b, a
        /// </summary>
        public Color4 SubTexture { get; set; }

        /// <summary>
        /// r, g, b, a
        /// </summary>
        public Color4 ToonTexture { get; set; }

        public PmxMaterialMorphOffset()
            : this(true)
        {
        }

        public PmxMaterialMorphOffset(bool initializeWithOne)
        {
            if (initializeWithOne)
            {
                this.Diffuse = Color4.One;
                this.Specular = Color3.One;
                this.Ambient = Color3.One;
                this.EdgeColor = Color4.One;
                this.EdgeSize = 1;
                this.Texture = Color4.One;
                this.SubTexture = Color4.One;
                this.ToonTexture = Color4.One;
            }
            else
            {
                this.Diffuse = Color4.Zero;
                this.Specular = Color3.Zero;
                this.Ambient = Color3.Zero;
                this.EdgeColor = Color4.Zero;
                this.Texture = Color4.Zero;
                this.SubTexture = Color4.Zero;
                this.ToonTexture = Color4.Zero;
            }
        }

        internal override void Read(ref BufferReader br, PmxDocument doc)
        {
            this.Material = doc.ReadMaterial(ref br);
            this.Kind = (PmxMaterialMorphKind)br.ReadByte();
            this.Diffuse = br.ReadColor4();
            this.Specular = br.ReadColor3();
            this.Power = br.ReadSingle();
            this.Ambient = br.ReadColor3();
            this.EdgeColor = br.ReadColor4();
            this.EdgeSize = br.ReadSingle();
            this.Texture = br.ReadColor4();
            this.SubTexture = br.ReadColor4();
            this.ToonTexture = br.ReadColor4();
        }

        internal override void Write(ref BufferWriter bw, PmxDocument doc, PmxIndexCache cache)
        {
            bw.Write(this.Material, cache);
            bw.Write((byte)this.Kind);
            bw.Write(this.Diffuse);
            bw.Write(this.Specular);
            bw.Write(this.Power);
            bw.Write(this.Ambient);
            bw.Write(this.EdgeColor);
            bw.Write(this.EdgeSize);
            bw.Write(this.Texture);
            bw.Write(this.SubTexture);
            bw.Write(this.ToonTexture);
        }
    }

    public class PmxGroupMorphOffset : PmxMorphOffset
    {
        public PmxMorph? Morph { get; set; }

        public float Weight { get; set; }

        internal override void Read(ref BufferReader br, PmxDocument doc)
        {
            this.Morph = doc.ReadMorph(ref br);
            this.Weight = br.ReadSingle();
        }

        internal override void Write(ref BufferWriter bw, PmxDocument doc, PmxIndexCache cache)
        {
            bw.Write(this.Morph, cache);
            bw.Write(this.Weight);
        }
    }

    /// <summary>
    /// (PMX 2.1)
    /// </summary>
    public class PmxFlipMorphOffset : PmxGroupMorphOffset
    {
    }

    /// <summary>
    /// (PMX 2.1)
    /// </summary>
    public class PmxImpulseMorphOffset : PmxMorphOffset
    {
        public PmxRigidBody? Rigid { get; set; }

        public bool IsLocal { get; set; }

        public Vector3 CentralImpulse { get; set; }

        public Vector3 TorqueImpulse { get; set; }

        internal override void Read(ref BufferReader br, PmxDocument doc)
        {
            this.Rigid = doc.ReadRigidBody(ref br);
            this.IsLocal = br.ReadBoolean();
            this.CentralImpulse = br.ReadVector3();
            this.TorqueImpulse = br.ReadVector3();
        }

        internal override void Write(ref BufferWriter bw, PmxDocument doc, PmxIndexCache cache)
        {
            bw.Write(this.Rigid, cache);
            bw.Write(this.IsLocal);
            bw.Write(this.CentralImpulse);
            bw.Write(this.TorqueImpulse);
        }
    }
}