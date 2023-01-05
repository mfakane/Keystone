using System.Numerics;

namespace Linearstar.Keystone.IO.MikuMikuDance.Pmx
{
    public class PmxBone
    {
        public string Name { get; set; }

        public string EnglishName { get; set; }
        
        public Vector3 Position { get; set; }

        public PmxBone? ParentBone { get; set; }

        public int Priority { get; set; }

        public PmxBoneCapabilities Capabilities { get; set; }

        /// <summary>
        /// Vector3, !this.Capabilities.HasFlag(PmxBoneCapabilities.ConnectToBone)
        /// </summary>
        public Vector3 ConnectToOffset { get; set; }

        /// <summary>
        /// this.Capabilities.HasFlag(PmxBoneCapabilities.ConnectToBone)
        /// </summary>
        public PmxBone? ConnectToBone { get; set; }

        /// <summary>
        /// this.Capabilities.HasFlag(PmxBoneCapabilities.RotationAffected) || this.Capabilities.HasFlag(PmxBoneCapabilities.MovementAffected)
        /// </summary>
        public PmxBone? AffectedBone { get; set; }

        /// <summary>
        /// this.Capabilities.HasFlag(PmxBoneCapabilities.RotationAffected) || this.Capabilities.HasFlag(PmxBoneCapabilities.MovementAffected)
        /// </summary>
        public float AffectionRate { get; set; }

        /// <summary>
        /// Vector3, this.Capabilities.HasFlag(PmxBoneCapabilities.FixedAxis)
        /// </summary>
        public Vector3 FixedAxis { get; set; }

        /// <summary>
        /// Vector3, this.Capabilities.HasFlag(PmxBoneCapabilities.LocalAxis)
        /// </summary>
        public Vector3 LocalVectorX { get; set; }

        /// <summary>
        /// Vector3, this.Capabilities.HasFlag(PmxBoneCapabilities.LocalAxis)
        /// </summary>
        public Vector3 LocalVectorZ { get; set; }

        /// <summary>
        /// this.Capabilities.HasFlag(PmxBoneCapabilities.TransformByExternalParent)
        /// </summary>
        public int ExternalParentKey { get; set; }

        /// <summary>
        /// this.Capabilities.HasFlag(PmxBoneCapabilities.IK)
        /// </summary>
        public PmxIK IK { get; set; }

        public PmxBone()
        {
            this.IK = new PmxIK();
        }

        internal void Parse(ref BufferReader br, PmxDocument doc)
        {
            this.Name = br.ReadString(doc.Header);
            this.EnglishName = br.ReadString(doc.Header);
            this.Position = br.ReadVector3();
            this.ParentBone = doc.ReadBone(ref br);
            this.Priority = br.ReadInt32();
            this.Capabilities = (PmxBoneCapabilities)br.ReadUInt16();

            if (this.Capabilities.HasFlag(PmxBoneCapabilities.ConnectToBone))
                this.ConnectToBone = doc.ReadBone(ref br);
            else
                this.ConnectToOffset = br.ReadVector3();

            if (this.Capabilities.HasFlag(PmxBoneCapabilities.RotationAffected) ||
                this.Capabilities.HasFlag(PmxBoneCapabilities.MovementAffected))
            {
                this.AffectedBone = doc.ReadBone(ref br);
                this.AffectionRate = br.ReadSingle();
            }

            if (this.Capabilities.HasFlag(PmxBoneCapabilities.FixedAxis))
                this.FixedAxis = br.ReadVector3();

            if (this.Capabilities.HasFlag(PmxBoneCapabilities.LocalAxis))
            {
                this.LocalVectorX = br.ReadVector3();
                this.LocalVectorZ = br.ReadVector3();
            }

            if (this.Capabilities.HasFlag(PmxBoneCapabilities.TransformByExternalParent))
                this.ExternalParentKey = br.ReadInt32();

            if (this.Capabilities.HasFlag(PmxBoneCapabilities.IK))
                this.IK = PmxIK.Parse(ref br, doc);
        }

        internal void Write(ref BufferWriter bw, PmxDocument doc, PmxIndexCache cache)
        {
            bw.Write(this.Name, doc.Header);
            bw.Write(this.EnglishName, doc.Header);
            bw.Write(this.Position);
            bw.Write(this.ParentBone, cache);
            bw.Write(this.Priority);
            bw.Write((ushort)this.Capabilities);

            if (this.Capabilities.HasFlag(PmxBoneCapabilities.ConnectToBone))
                bw.Write(this.ConnectToBone, cache);
            else
                bw.Write(this.ConnectToOffset);

            if (this.Capabilities.HasFlag(PmxBoneCapabilities.RotationAffected) ||
                this.Capabilities.HasFlag(PmxBoneCapabilities.MovementAffected))
            {
                bw.Write(this.AffectedBone, cache);
                bw.Write(this.AffectionRate);
            }

            if (this.Capabilities.HasFlag(PmxBoneCapabilities.FixedAxis))
                bw.Write(this.FixedAxis);

            if (this.Capabilities.HasFlag(PmxBoneCapabilities.LocalAxis))
            {
                bw.Write(this.LocalVectorX);
                bw.Write(this.LocalVectorZ);
            }

            if (this.Capabilities.HasFlag(PmxBoneCapabilities.TransformByExternalParent))
                bw.Write(this.ExternalParentKey);

            if (this.Capabilities.HasFlag(PmxBoneCapabilities.IK))
                this.IK.Write(ref bw, cache);
        }
    }
}