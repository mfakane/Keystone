namespace Linearstar.Keystone.IO.MikuMikuDance.Vmd
{
    public class VmdSelfShadowFrame
    {
        public uint FrameTime { get; set; }

        public VmdSelfShadowModel Model { get; set; } = VmdSelfShadowModel.Model1;

        public float Distance { get; set; } = (10000f - 8875) / 100000;

        internal static VmdSelfShadowFrame Parse(ref BufferReader br) =>
            new()
            {
                FrameTime = br.ReadUInt32(),
                Model = (VmdSelfShadowModel)br.ReadByte(),
                Distance = br.ReadSingle(),
            };

        internal void Write(ref BufferWriter bw)
        {
            bw.Write(this.FrameTime);
            bw.Write((byte)this.Model);
            bw.Write(this.Distance);
        }
    }
}