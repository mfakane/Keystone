namespace Linearstar.Keystone.IO.MikuMikuMoving.Mvd
{
    public struct MvdTimeWarpPoint
    {
        public float X;
        public float Y;

        public MvdTimeWarpPoint(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        internal static MvdTimeWarpPoint Parse(ref BufferReader br) => new(br.ReadSingle(), br.ReadSingle());

        internal void Write(ref BufferWriter bw)
        {
            bw.Write(this.X);
            bw.Write(this.Y);
        }

        public override string ToString() => "{X:" + this.X + " Y:" + this.Y + "}";
    }
}