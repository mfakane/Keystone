using System.Numerics;

namespace Linearstar.Keystone.IO.MikuMikuDance.Vmd
{
    public class VmdLightFrame
    {
        public uint FrameTime { get; set; }

        public Color3 Color { get; set; } = new(0.6f, 0.6f, 0.6f);

        public Vector3 Position { get; set; } = new(-0.5f, -1, 0.5f);

        internal static VmdLightFrame Parse(ref BufferReader br) =>
            new()
            {
                FrameTime = br.ReadUInt32(),
                Color = br.ReadColor3(),
                Position = br.ReadVector3(),
            };

        internal void Write(ref BufferWriter bw)
        {
            bw.Write(this.FrameTime);
            bw.Write(this.Color);
            bw.Write(this.Position);
        }
    }
}