namespace Linearstar.Keystone.IO.MikuMikuDance.Vmd
{
    public class VmdMorphFrame
    {
        public string Name { get; set; }

        public uint FrameTime { get; set; }

        public float Weight { get; set; }

        internal static VmdMorphFrame Parse(ref BufferReader br) =>
            new()
            {
                Name = br.ReadString(15),
                FrameTime = br.ReadUInt32(),
                Weight = br.ReadSingle(),
            };

        internal void Write(ref BufferWriter bw)
        {
            bw.Write(this.Name, 15);
            bw.Write(this.FrameTime);
            bw.Write(this.Weight);
        }
    }
}