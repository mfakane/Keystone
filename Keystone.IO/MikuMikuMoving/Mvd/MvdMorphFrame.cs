namespace Linearstar.Keystone.IO.MikuMikuMoving.Mvd
{
    public class MvdMorphFrame
    {
        public long FrameTime { get; set; }

        public float Weight { get; set; }

        public MvdInterpolationPair Interpolation { get; set; } = MvdInterpolationPair.Default;

        internal static MvdMorphFrame Parse(ref BufferReader br) =>
            new()
            {
                FrameTime = br.ReadInt64(),
                Weight = br.ReadSingle(),
                Interpolation = MvdInterpolationPair.Parse(ref br),
            };

        internal void Write(ref BufferWriter bw)
        {
            bw.Write(this.FrameTime);
            bw.Write(this.Weight);
            this.Interpolation.Write(ref bw);
        }
    }
}