using System;
using System.Numerics;

namespace Linearstar.Keystone.IO.MikuMikuDance.Vmd
{
    public class VmdCameraFrame
    {
        public uint FrameTime { get; set; }

        public float Radius { get; set; } = 50;

        public Vector3 Position { get; set; } = new(0, 10, 0);

        public Vector3 Angle { get; set; } = new(0, (float)Math.PI, 0);

        public VmdInterpolationPair XInterpolation { get; set; } = VmdInterpolationPair.Default;

        public VmdInterpolationPair YInterpolation { get; set; } = VmdInterpolationPair.Default;

        public VmdInterpolationPair ZInterpolation { get; set; } = VmdInterpolationPair.Default;

        public VmdInterpolationPair AngleInterpolation { get; set; } = VmdInterpolationPair.Default;

        public VmdInterpolationPair RadiusInterpolation { get; set; } = VmdInterpolationPair.Default;

        public VmdInterpolationPair FovInterpolation { get; set; } = VmdInterpolationPair.Default;

        public int FovInDegree { get; set; } = 30;

        public bool Ortho { get; set; }

        internal static VmdCameraFrame Parse(ref BufferReader br, VmdVersion version)
        {
            var rt = new VmdCameraFrame
            {
                FrameTime = br.ReadUInt32(),
                Radius = br.ReadSingle(),
                Position = br.ReadVector3(),
                Angle = br.ReadVector3(),
            };

            if (version == VmdVersion.MMDVer3)
            {
                rt.XInterpolation = ReadInterpolationPair(ref br);
                rt.YInterpolation = ReadInterpolationPair(ref br);
                rt.ZInterpolation = ReadInterpolationPair(ref br);
                rt.AngleInterpolation = ReadInterpolationPair(ref br);
                rt.RadiusInterpolation = ReadInterpolationPair(ref br);
                rt.FovInterpolation = ReadInterpolationPair(ref br);
                rt.FovInDegree = br.ReadInt32();
                rt.Ortho = br.ReadBoolean();
            }
            else
                rt.AngleInterpolation = ReadInterpolationPair(ref br);

            return rt;
            
            static VmdInterpolationPair ReadInterpolationPair(ref BufferReader br)
            {
                var xxyy = br.ReadBytes(4);

                return new(new VmdInterpolationPoint(xxyy[0], xxyy[2]), new VmdInterpolationPoint(xxyy[1], xxyy[3]));
            }
        }

        internal void Write(ref BufferWriter bw, VmdVersion version)
        {
            bw.Write(this.FrameTime);
            bw.Write(this.Radius);
            bw.Write(this.Position);
            bw.Write(this.Angle);

            if (version == VmdVersion.MMDVer3)
            {
                WriteInterpolationPair(ref bw, this.XInterpolation);
                WriteInterpolationPair(ref bw, this.YInterpolation);
                WriteInterpolationPair(ref bw, this.ZInterpolation);
                WriteInterpolationPair(ref bw, this.AngleInterpolation);
                WriteInterpolationPair(ref bw, this.RadiusInterpolation);
                WriteInterpolationPair(ref bw, this.FovInterpolation);
                bw.Write(this.FovInDegree);
                bw.Write(this.Ortho);
            }
            else
                WriteInterpolationPair(ref bw, this.AngleInterpolation);
            
            static void WriteInterpolationPair(ref BufferWriter bw, VmdInterpolationPair pair)
            {
                bw.Write(pair.A.X);
                bw.Write(pair.B.X);
                bw.Write(pair.A.Y);
                bw.Write(pair.B.Y);
            }
        }
    }
}