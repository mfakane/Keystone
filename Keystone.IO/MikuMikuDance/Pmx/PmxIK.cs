using System.Collections.Generic;

namespace Linearstar.Keystone.IO.MikuMikuDance.Pmx
{
    public class PmxIK
    {
        public PmxBone? TargetBone { get; set; }

        public int LoopCount { get; set; }

        public float AngleLimitUnit { get; set; }

        public IList<PmxIKBinding> BindedBones { get; set; } = new List<PmxIKBinding>();

        internal static PmxIK Parse(ref BufferReader br, PmxDocument doc)
        {
            var ik = new PmxIK
            {
                TargetBone = doc.ReadBone(ref br),
                LoopCount = br.ReadInt32(),
                AngleLimitUnit = br.ReadSingle(),
            };

            for (var i = br.ReadInt32() - 1; i >= 0; i--)
                ik.BindedBones.Add(PmxIKBinding.Parse(ref br, doc));

            return ik;
        }

        internal void Write(ref BufferWriter bw, PmxIndexCache cache)
        {
            bw.Write(this.TargetBone, cache);
            bw.Write(this.LoopCount);
            bw.Write(this.AngleLimitUnit);
            bw.Write(this.BindedBones.Count);
            
            foreach (var bone in this.BindedBones)
                bone.Write(ref bw, cache);
        }
    }
}