using System.Numerics;

namespace Linearstar.Keystone.IO.MikuMikuMoving.Mvd
{
    public class MvdFilterFrame
    {
        public long FrameTime { get; set; }

        public int FilterType { get; set; }

        public bool Interpolation { get; set; }

        /// <summary>
        /// r, g, b
        /// </summary>
        public Color3 FadeColor { get; set; }

        public float FadeValue { get; set; }

        public Vector3 HSVValue { get; set; }

        public MvdTimeWarpPoint[] ToneCurveControlPoints { get; set; } = { };

        internal static MvdFilterFrame Parse(ref BufferReader br)
        {
            var rt = new MvdFilterFrame
            {
                FrameTime = br.ReadInt64(),
                FilterType = br.ReadInt32(),
                Interpolation = br.ReadBoolean(),
            };

            br.ReadBytes(3);
            rt.FadeColor = br.ReadColor3();
            rt.FadeValue = br.ReadSingle();
            rt.HSVValue = br.ReadVector3();

            var points = new MvdTimeWarpPoint[br.ReadInt32()];

            for (var i = 0; i < points.Length; i++)
                points[i] = MvdTimeWarpPoint.Parse(ref br);

            rt.ToneCurveControlPoints = points;

            return rt;
        }

        internal void Write(ref BufferWriter bw)
        {
            bw.Write(this.FrameTime);
            bw.Write(this.FilterType);
            bw.Write(this.Interpolation);
            bw.Write(new byte[3]);
            bw.Write(this.FadeColor);
            bw.Write(this.FadeValue);
            bw.Write(this.HSVValue);
            bw.Write(this.ToneCurveControlPoints.Length);

            foreach (var point in this.ToneCurveControlPoints)
                point.Write(ref bw);
        }
    }
}