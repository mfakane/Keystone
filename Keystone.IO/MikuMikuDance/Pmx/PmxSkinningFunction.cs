using System;
using System.Numerics;

namespace Linearstar.Keystone.IO.MikuMikuDance.Pmx
{
    public abstract class PmxSkinningFunction
    {
        internal static PmxSkinningFunction Parse(ref BufferReader br, PmxDocument doc, PmxSkinningKind kind)
        {
            PmxSkinningFunction rt;

            switch (kind)
            {
                case PmxSkinningKind.LinearBlendDeforming1:
                    rt = new PmxLinearBlendDeforming1();

                    break;
                case PmxSkinningKind.LinearBlendDeforming2:
                    rt = new PmxLinearBlendDeforming2();

                    break;
                case PmxSkinningKind.LinearBlendDeforming4:
                    rt = new PmxLinearBlendDeforming4();

                    break;
                case PmxSkinningKind.SphericalDeforming:
                    rt = new PmxSphericalDeforming();

                    break;
                case PmxSkinningKind.DualQuaternionDeforming:
                    rt = new PmxDualQuaternionDeforming();

                    break;
                default:
                    throw new NotSupportedException();
            }

            rt.Read(ref br, doc);

            return rt;
        }

        internal abstract void Read(ref BufferReader br, PmxDocument doc);
        internal abstract void Write(ref BufferWriter bw, PmxDocument doc, PmxIndexCache cache);
    }

    public class PmxLinearBlendDeforming1 : PmxSkinningFunction
    {
        public PmxBone? Bone { get; set; }

        public PmxLinearBlendDeforming1()
        {
        }

        internal override void Read(ref BufferReader br, PmxDocument doc)
        {
            this.Bone = doc.ReadBone(ref br);
        }

        internal override void Write(ref BufferWriter bw, PmxDocument doc, PmxIndexCache cache)
        {
            bw.Write(this.Bone, cache);
        }
    }

    public class PmxLinearBlendDeforming2 : PmxSkinningFunction
    {
        public PmxBone? BoneA { get; set; }
        
        public PmxBone? BoneB { get; set; }

        public float Weight { get; set; }

        public PmxLinearBlendDeforming2()
        {
        }

        internal override void Read(ref BufferReader br, PmxDocument doc)
        {
            this.BoneA = doc.ReadBone(ref br);
            this.BoneB = doc.ReadBone(ref br);
            this.Weight = br.ReadSingle();
        }

        internal override void Write(ref BufferWriter bw, PmxDocument doc, PmxIndexCache cache)
        {
            bw.Write(this.BoneA, cache);
            bw.Write(this.BoneB, cache);
            bw.Write(this.Weight);
        }
    }

    public class PmxLinearBlendDeforming4 : PmxSkinningFunction
    {
        public PmxBone? BoneA { get; set; }
        
        public PmxBone? BoneB { get; set; }
        
        public PmxBone? BoneC { get; set; }
        
        public PmxBone? BoneD { get; set; }
        
        public Vector4 Weights { get; set; }

        public PmxLinearBlendDeforming4()
        {
        }

        internal override void Read(ref BufferReader br, PmxDocument doc)
        {
            this.BoneA = doc.ReadBone(ref br);
            this.BoneB = doc.ReadBone(ref br);
            this.BoneC = doc.ReadBone(ref br);
            this.BoneD = doc.ReadBone(ref br);
            this.Weights = br.ReadVector4();
        }

        internal override void Write(ref BufferWriter bw, PmxDocument doc, PmxIndexCache cache)
        {
            bw.Write(this.BoneA, cache);
            bw.Write(this.BoneB, cache);
            bw.Write(this.BoneC, cache);
            bw.Write(this.BoneD, cache);
            bw.Write(this.Weights);
        }
    }

    public class PmxSphericalDeforming : PmxSkinningFunction
    {
        public PmxBone? BoneA { get; set; }
        
        public PmxBone? BoneB { get; set; }

        public float Weight { get; set; }

        /// <summary>
        /// Vector3 SDEF-C
        /// </summary>
        public Vector3 Center { get; set; }

        /// <summary>
        /// Vector3 SDEF-R0
        /// </summary>
        public Vector3 RangeZero { get; set; }

        /// <summary>
        /// Vector3 SDEF-R1
        /// </summary>
        public Vector3 RangeOne { get; set; }

        public PmxSphericalDeforming()
        {
        }

        internal override void Read(ref BufferReader br, PmxDocument doc)
        {
            this.BoneA = doc.ReadBone(ref br);
            this.BoneB = doc.ReadBone(ref br);
            this.Weight = br.ReadSingle();
            this.Center = br.ReadVector3();
            this.RangeZero = br.ReadVector3();
            this.RangeOne = br.ReadVector3();
        }

        internal override void Write(ref BufferWriter bw, PmxDocument doc, PmxIndexCache cache)
        {
            bw.Write(this.BoneA, cache);
            bw.Write(this.BoneB, cache);
            bw.Write(this.Weight);
            bw.Write(this.Center);
            bw.Write(this.RangeZero);
            bw.Write(this.RangeOne);
        }
    }

    /// <summary>
    /// (PMX 2.1)
    /// </summary>
    public class PmxDualQuaternionDeforming : PmxLinearBlendDeforming4
    {
    }
}