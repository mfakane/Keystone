namespace Linearstar.Keystone.IO.MikuMikuMoving.Mvd
{
    public class MvdMotionBlendLink
    {
        public int ClipAId { get; set; }

        public int ClipBId { get; set; }

        public int BlendSplineType { get; set; }

        public MvdTimeWarpPoint[] BlendPoints { get; set; } = { };

        internal static MvdMotionBlendLink Parse(ref BufferReader br)
        {
            var motionBlendLink = new MvdMotionBlendLink
            {
                ClipAId = br.ReadInt32(),
                ClipBId = br.ReadInt32(),
                BlendSplineType = br.ReadInt32(),
            };

            var points = new MvdTimeWarpPoint[br.ReadInt32()];

            for (var i = 0; i < points.Length; i++)
                points[i] = MvdTimeWarpPoint.Parse(ref br);
            
            motionBlendLink.BlendPoints = points;

            return motionBlendLink;
        }

        internal void Write(ref BufferWriter bw)
        {
            bw.Write(this.ClipAId);
            bw.Write(this.ClipBId);
            bw.Write(this.BlendSplineType);
            bw.Write(this.BlendPoints.Length);

            foreach (var point in this.BlendPoints)
                point.Write(ref bw);
        }
    }
}