using System.Numerics;

namespace Linearstar.Keystone.IO.MikuMikuDance.Pmx
{
    public class PmxIKBinding
    {
        public PmxBone? Bone { get; set; }

        public bool IsAngleLimitEnabled { get; set; }

        public Vector3 LowerAngleLimit { get; set; }

        public Vector3 HigherAngleLimit { get; set; }

        internal static PmxIKBinding Parse(ref BufferReader br, PmxDocument doc)
        {
            var rt = new PmxIKBinding
            {
                Bone = doc.ReadBone(ref br),
                IsAngleLimitEnabled = br.ReadBoolean(),
            };

            if (rt.IsAngleLimitEnabled)
            {
                rt.LowerAngleLimit = br.ReadVector3();
                rt.HigherAngleLimit = br.ReadVector3();
            }

            return rt;
        }

        internal void Write(ref BufferWriter bw, PmxIndexCache cache)
        {
            bw.Write(this.Bone, cache);
            bw.Write(this.IsAngleLimitEnabled);

            if (!this.IsAngleLimitEnabled) return;
            
            bw.Write(this.LowerAngleLimit);
            bw.Write(this.HigherAngleLimit);
        }
    }
}