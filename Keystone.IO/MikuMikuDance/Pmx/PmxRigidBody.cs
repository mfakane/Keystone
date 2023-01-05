using System.Numerics;
using Linearstar.Keystone.IO.MikuMikuDance.Pmd;

namespace Linearstar.Keystone.IO.MikuMikuDance.Pmx
{
    public class PmxRigidBody
    {
        public string Name { get; set; }

        public string EnglishName { get; set; }

        public PmxBone? RelatedBone { get; set; }

        public byte Group { get; set; }

        public PmdRigidGroups CollidableGroups { get; set; }

        public PmdRigidShape Shape { get; set; }

        public Vector3 Size { get; set; }

        public Vector3 Position { get; set; }

        /// <summary>
        /// radians
        /// </summary>
        public Vector3 Rotation { get; set; }

        public float Mass { get; set; }

        public float LinearDamping { get; set; }

        public float AngularDamping { get; set; }

        public float Restitution { get; set; }

        public float Friction { get; set; }

        public PmxRigidKind Kind { get; set; }

        internal static PmxRigidBody Parse(ref BufferReader br, PmxDocument doc) =>
            new()
            {
                Name = br.ReadString(doc.Header),
                EnglishName = br.ReadString(doc.Header),
                RelatedBone = doc.ReadBone(ref br),
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
                Kind = (PmxRigidKind)br.ReadByte(),
            };

        internal void Write(ref BufferWriter bw, PmxDocument doc, PmxIndexCache cache)
        {
            bw.Write(this.Name, doc.Header);
            bw.Write(this.EnglishName, doc.Header);
            bw.Write(this.RelatedBone, cache);
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