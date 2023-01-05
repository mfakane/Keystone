namespace Linearstar.Keystone.IO.MikuMikuMoving.Mvd
{
    public class MvdMotionClip
    {
        public int TrackIndex { get; set; }
        
        public float FrameStart { get; set; }

        public float FrameLength { get; set; }

        public int RepeatCount { get; set; }

        public float Weight { get; set; }

        public float Scale { get; set; }

        public int TimeWarpSplineType { get; set; }

        public MvdTimeWarpPoint[] TimeWarpPoints { get; set; } = { };

        internal static MvdMotionClip Parse(ref BufferReader br)
        {
            var motionClip = new MvdMotionClip
            {
                TrackIndex = br.ReadInt32(),
                FrameStart = br.ReadSingle(),
                FrameLength = br.ReadSingle(),
                RepeatCount = br.ReadInt32(),
                Weight = br.ReadSingle(),
                Scale = br.ReadSingle(),
                TimeWarpSplineType = br.ReadInt32(),
            };

            var points = new MvdTimeWarpPoint[br.ReadInt32()];

            for (var i = 0; i < points.Length; i++)
                points[i] = MvdTimeWarpPoint.Parse(ref br);

            motionClip.TimeWarpPoints = points;
            
            return motionClip;
        }

        internal void Write(ref BufferWriter bw)
        {
            bw.Write(this.TrackIndex);
            bw.Write(this.FrameStart);
            bw.Write(this.FrameLength);
            bw.Write(this.RepeatCount);
            bw.Write(this.Weight);
            bw.Write(this.Scale);
            bw.Write(this.TimeWarpSplineType);
            bw.Write(this.TimeWarpPoints.Length);

            foreach (var point in this.TimeWarpPoints)
                point.Write(ref bw);
        }
    }
}