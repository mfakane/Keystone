using System;
using System.Numerics;

namespace Linearstar.Keystone.IO.MikuMikuMoving.Mvd
{
    public class MvdCameraFrame
    {
        public int StageId { get; set; }

        public long FrameTime { get; set; }

        public float Radius { get; set; } = 50;

        public Vector3 Position { get; set; } = new(0, 10, 0);

        public Vector3 Angle { get; set; } = new(0, (float)Math.PI, 0);

        public float Fov { get; set; } = (float)(Math.PI * 1.666667);

        public bool Spline { get; set; }

        public MvdInterpolationPair PositionInterpolation { get; set; } = MvdInterpolationPair.Default;

        public MvdInterpolationPair AngleInterpolation { get; set; } = MvdInterpolationPair.Default;

        public MvdInterpolationPair RadiusInterpolation { get; set; } = MvdInterpolationPair.Default;

        public MvdInterpolationPair FovInterpolation { get; set; } = MvdInterpolationPair.Default;

        internal static MvdCameraFrame Parse(ref BufferReader br, MvdCameraData cd, out MvdCameraPropertyFrame? propertyFrame)
        {
            var rt = new MvdCameraFrame
            {
                StageId = br.ReadInt32(),
                FrameTime = br.ReadInt64(),
                Radius = br.ReadSingle(),
                Position = br.ReadVector3(),
                Angle = br.ReadVector3(),
                Fov = br.ReadSingle(),
            };

            propertyFrame = null;

            switch (cd.MinorType)
            {
                case 0:
                    propertyFrame = new MvdCameraPropertyFrame
                    {
                        FrameTime = rt.FrameTime,
                        Perspective = br.ReadBoolean(),
                    };

                    break;
                case 1:
                    propertyFrame = new MvdCameraPropertyFrame
                    {
                        FrameTime = rt.FrameTime,
                        Enabled = br.ReadBoolean(),
                        Perspective = br.ReadBoolean(),
                        Alpha = br.ReadSingle(),
                        EffectEnabled = br.ReadBoolean(),
                    };

                    break;
                case 3:
                    rt.Spline = br.ReadBoolean();
                    br.ReadBytes(3);

                    break;
            }

            rt.PositionInterpolation = MvdInterpolationPair.Parse(ref br);
            rt.AngleInterpolation = MvdInterpolationPair.Parse(ref br);
            rt.RadiusInterpolation = MvdInterpolationPair.Parse(ref br);
            rt.FovInterpolation = MvdInterpolationPair.Parse(ref br);

            return rt;
        }

        internal void Write(ref BufferWriter bw, MvdCameraData cd)
        {
            if (cd.MinorType != 3)
                throw new NotImplementedException("MinorType != 3 not implemented");

            bw.Write(this.StageId);
            bw.Write(this.FrameTime);
            bw.Write(this.Radius);
            bw.Write(this.Position);
            bw.Write(this.Angle);
            bw.Write(this.Fov);

            bw.Write(this.Spline);
            bw.Write(new byte[] { 0, 0, 0 });

            this.PositionInterpolation.Write(ref bw);
            this.AngleInterpolation.Write(ref bw);
            this.RadiusInterpolation.Write(ref bw);
            this.FovInterpolation.Write(ref bw);
        }

        public string GetName(MvdNameList names, MvdCameraData cameraData)
        {
            if (this.StageId == 0)
                return names.Names[cameraData.Key];
            else
            {
                var key = cameraData.Key * -1000 - this.StageId;

                return names.Names.ContainsKey(key)
                    ? names.Names[key]
                    : this.StageId.ToString("000");
            }
        }
    }
}