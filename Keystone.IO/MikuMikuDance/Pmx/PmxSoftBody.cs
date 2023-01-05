using System;
using System.Collections.Generic;
using System.Linq;
using Linearstar.Keystone.IO.MikuMikuDance.Pmd;

namespace Linearstar.Keystone.IO.MikuMikuDance.Pmx
{
    /// <summary>
    /// (PMX 2.1)
    /// </summary>
    public class PmxSoftBody
    {
        public string Name { get; set; } = "";

        public string EnglishName { get; set; } = "";

        public PmxSoftBodyKind Kind { get; set; }

        public PmxMaterial? RelatedMaterial { get; set; }

        public byte Group { get; set; }

        public PmdRigidGroups CollidableGroups { get; set; }

        public PmxSoftBodyOptions Options { get; set; }

        public int BendingLinkDistance { get; set; }

        public int ClusterCount { get; set; }

        public float TotalMass { get; set; }

        public float Margin { get; set; }

        public PmxSoftBodyAeroModel AeroModel { get; set; }

        public IDictionary<PmxConfigurationIndex, float> Configuration { get; set; }

        /// <summary>
        /// V_IT
        /// </summary>
        public int VelocitySolverIteration { get; set; }

        /// <summary>
        /// P_IT
        /// </summary>
        public int PositonSolverIteration { get; set; }

        /// <summary>
        /// D_IT
        /// </summary>
        public int DriftSolverIteration { get; set; }

        /// <summary>
        /// C_IT
        /// </summary>
        public int ClusterSolverIteration { get; set; }

        /// <summary>
        /// LST
        /// </summary>
        public float LinearStiffnessCoefficient { get; set; }

        /// <summary>
        /// AST
        /// </summary>
        public float AreaAngularStiffnessCoefficient { get; set; }

        /// <summary>
        /// VST
        /// </summary>
        public float VolumeStiffnessCoefficient { get; set; }

        public IList<PmxSoftBodyAnchor> Anchors { get; set; } = new List<PmxSoftBodyAnchor>();

        public IList<PmxVertex> PinnedVertices { get; set; } = new List<PmxVertex>();

        public PmxSoftBody()
        {
            this.Configuration = Enum.GetValues(typeof(PmxConfigurationIndex)).Cast<PmxConfigurationIndex>()
                .ToDictionary(_ => _, _ => 0f);
        }

        internal static PmxSoftBody Parse(ref BufferReader br, PmxDocument doc)
        {
            var softBody = new PmxSoftBody
            {
                Name = br.ReadString(doc.Header),
                EnglishName = br.ReadString(doc.Header),
                Kind = (PmxSoftBodyKind)br.ReadByte(),
                RelatedMaterial = doc.ReadMaterial(ref br),
                Group = br.ReadByte(),
                CollidableGroups = (PmdRigidGroups)br.ReadUInt16(),
                Options = (PmxSoftBodyOptions)br.ReadByte(),
                BendingLinkDistance = br.ReadInt32(),
                ClusterCount = br.ReadInt32(),
                TotalMass = br.ReadSingle(),
                Margin = br.ReadSingle(),
                AeroModel = (PmxSoftBodyAeroModel)br.ReadInt32(),
            };
            
            foreach (var configurationIndex in Enum.GetValues(typeof(PmxConfigurationIndex)).Cast<PmxConfigurationIndex>())
                softBody.Configuration.Add(configurationIndex, br.ReadSingle());

            softBody.VelocitySolverIteration = br.ReadInt32();
            softBody.PositonSolverIteration = br.ReadInt32();
            softBody.DriftSolverIteration = br.ReadInt32();
            softBody.ClusterSolverIteration = br.ReadInt32();
            softBody.LinearStiffnessCoefficient = br.ReadSingle();
            softBody.AreaAngularStiffnessCoefficient = br.ReadSingle();
            softBody.VolumeStiffnessCoefficient = br.ReadSingle();

            for (var i = br.ReadInt32() - 1; i >= 0; i--)
                softBody.Anchors.Add(PmxSoftBodyAnchor.Parse(ref br, doc));
            
            for (var i = br.ReadInt32() - 1; i >= 0; i--)
                softBody.PinnedVertices.Add(doc.ReadVertex(ref br)!);

            return softBody;
        }

        internal void Write(ref BufferWriter bw, PmxDocument doc, PmxIndexCache cache)
        {
            bw.Write(this.Name, doc.Header);
            bw.Write(this.EnglishName, doc.Header);
            bw.Write((byte)this.Kind);
            bw.Write(this.RelatedMaterial, cache);
            bw.Write(this.Group);
            bw.Write((ushort)this.CollidableGroups);
            bw.Write((byte)this.Options);
            bw.Write(this.BendingLinkDistance);
            bw.Write(this.ClusterCount);
            bw.Write(this.TotalMass);
            bw.Write(this.Margin);
            bw.Write((int)this.AeroModel);

            foreach (var pair in this.Configuration.OrderBy(x => x.Key))
                bw.Write(pair.Value);
            
            bw.Write(this.VelocitySolverIteration);
            bw.Write(this.PositonSolverIteration);
            bw.Write(this.DriftSolverIteration);
            bw.Write(this.ClusterSolverIteration);
            bw.Write(this.LinearStiffnessCoefficient);
            bw.Write(this.AreaAngularStiffnessCoefficient);
            bw.Write(this.VolumeStiffnessCoefficient);
            
            bw.Write(this.Anchors.Count);
            foreach (var anchor in this.Anchors)
                anchor.Write(ref bw, cache);
            
            bw.Write(this.PinnedVertices.Count);

            foreach (var vertex in this.PinnedVertices)
                bw.Write(vertex, cache);
        }
    }
}