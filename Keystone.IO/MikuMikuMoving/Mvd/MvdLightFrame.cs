using System.Numerics;

namespace Linearstar.Keystone.IO.MikuMikuMoving.Mvd
{
    public class MvdLightFrame
    {
        public long FrameTime { get; set; }

        public Vector3 Position { get; set; } = new(-0.5f, -1, 0.5f);

        public Color3B Color { get; set; } = new(153, 153, 153);

        public bool Enabled { get; set; } = true;

        internal static MvdLightFrame Parse(ref BufferReader br) =>
            new()
            {
                FrameTime = br.ReadInt64(),
                Position = br.ReadVector3(),
                Color = br.ReadColor3B(),
                Enabled = br.ReadBoolean(),
            };

        internal void Write(ref BufferWriter bw)
        {
            bw.Write(this.FrameTime);
            bw.Write(this.Position);
            bw.Write(this.Color);
            bw.Write(this.Enabled);
        }
    }
}