namespace Linearstar.Keystone.IO.MikuMikuMoving.Mvd
{
    public class MvdAccessoryPropertyFrame
    {
        public long FrameTime { get; set; }

        public bool Visible { get; set; } = true;

        public bool Shadow { get; set; }

        public bool AddBlending { get; set; }

        public bool Reserved { get; set; }

        public float Scaling { get; set; } = 1;

        public float Alpha { get; set; } = 1;

        public int RelatedModelId { get; set; }

        public int RelatedBoneId { get; set; }

        internal static MvdAccessoryPropertyFrame Parse(ref BufferReader br) =>
            new()
            {
                FrameTime = br.ReadInt64(),
                Visible = br.ReadBoolean(),
                Shadow = br.ReadBoolean(),
                AddBlending = br.ReadBoolean(),
                Reserved = br.ReadBoolean(),
                Scaling = br.ReadSingle(),
                Alpha = br.ReadSingle(),
                RelatedModelId = br.ReadInt32(),
                RelatedBoneId = br.ReadInt32(),
            };

        internal virtual void Write(ref BufferWriter bw)
        {
            bw.Write(this.FrameTime);
            bw.Write(this.Visible);
            bw.Write(this.Shadow);
            bw.Write(this.AddBlending);
            bw.Write(this.Reserved);
            bw.Write(this.Scaling);
            bw.Write(this.Alpha);
            bw.Write(this.RelatedModelId);
            bw.Write(this.RelatedBoneId);
        }
    }
}