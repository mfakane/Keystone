using System.Numerics;

namespace Linearstar.Keystone.IO.MikuMikuMoving.Mvd
{
    public class MvdProjectFrame
    {
        public long FrameTime { get; set; }

        public float Gravity { get; set; } = 0.98f;

        public Vector3 GravityVector { get; set; } = new(0, -1, 0);

        public MvdSelfShadowModel SelfShadowModel { get; set; }

        public float SelfShadowDistance { get; set; } = 1;

        public float SelfShadowDeep { get; set; } = 1;

        internal static MvdProjectFrame Parse(ref BufferReader br, MvdProjectData pd)
        {
            var rt = new MvdProjectFrame
            {
                FrameTime = br.ReadInt64(),
            };

            if (pd.MinorType >= 1)
            {
                rt.Gravity = br.ReadSingle();
                rt.GravityVector = br.ReadVector3();
            }

            rt.SelfShadowModel = (MvdSelfShadowModel)br.ReadInt32();
            rt.SelfShadowDistance = br.ReadSingle();

            if (pd.MinorType >= 1 || !br.IsCompleted)
                rt.SelfShadowDeep = br.ReadSingle();

            return rt;
        }

        internal void Write(ref BufferWriter bw, MvdProjectData pd)
        {
            bw.Write(this.FrameTime);

            if (pd.MinorType >= 1)
            {
                bw.Write(this.Gravity);
                bw.Write(this.GravityVector);
            }

            bw.Write((int)this.SelfShadowModel);
            bw.Write(this.SelfShadowDistance);
            bw.Write(this.SelfShadowDeep);
        }
    }
}