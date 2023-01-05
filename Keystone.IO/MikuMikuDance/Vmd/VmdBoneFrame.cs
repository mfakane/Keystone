using System;
using System.Numerics;

namespace Linearstar.Keystone.IO.MikuMikuDance.Vmd
{
    public class VmdBoneFrame
    {
        public string Name { get; set; }

        public uint FrameTime { get; set; }

        public Vector3 Position { get; set; }

        public Quaternion Quaternion { get; set; } = Quaternion.Identity;

        public VmdInterpolationPair XInterpolation { get; set; } = VmdInterpolationPair.Default;

        public VmdInterpolationPair YInterpolation { get; set; } = VmdInterpolationPair.Default;

        public VmdInterpolationPair ZInterpolation { get; set; } = VmdInterpolationPair.Default;

        public VmdInterpolationPair RotationInterpolation { get; set; } = VmdInterpolationPair.Default;
        
        public VmdBoneFrame(string name)
        {
            this.Name = name;
        }
        
        internal static VmdBoneFrame Parse(ref BufferReader br)
        {
            var rt = new VmdBoneFrame(br.ReadString(15))
            {
                FrameTime = br.ReadUInt32(),
                Position = br.ReadVector3(),
                Quaternion = br.ReadQuaternion(),
            };
            var ipBuffer = br.ReadBytes(64);

            rt.XInterpolation = GetInterpolationPair(ref ipBuffer, 0);
            rt.YInterpolation = GetInterpolationPair(ref ipBuffer, 1);
            rt.ZInterpolation = GetInterpolationPair(ref ipBuffer, 2);
            rt.RotationInterpolation = GetInterpolationPair(ref ipBuffer, 3);

            return rt;

            static VmdInterpolationPoint GetInterpolationPoint(ref ReadOnlySpan<byte> span, int index) => 
                new(span[index], span[index + 4]);
            
            static VmdInterpolationPair GetInterpolationPair(ref ReadOnlySpan<byte> span, int index) => 
                new(GetInterpolationPoint(ref span, index), GetInterpolationPoint(ref span, index + 8));
        }

        internal void Write(ref BufferWriter bw)
        {
            bw.Write(this.Name, 15);
            bw.Write(this.FrameTime);
            bw.Write(this.Position);
            bw.Write(this.Quaternion);

            var x = this.XInterpolation;
            var y = this.YInterpolation;
            var z = this.ZInterpolation;
            var r = this.RotationInterpolation;
            ReadOnlySpan<byte> line = stackalloc byte[]
            {
                x.A.X, y.A.X, z.A.X, r.A.X, x.A.Y, y.A.Y, z.A.Y, r.A.Y,
                x.B.X, y.B.X, z.B.X, r.B.X, x.B.Y, y.B.Y, z.B.Y, r.B.Y,
                01, 00, 00,
            };
            
            bw.Write(line.Slice(0, 16));
            bw.Write(line.Slice(1, 16));
            bw.Write(line.Slice(2, 16));
            bw.Write(line.Slice(3, 16));
        }
    }
}