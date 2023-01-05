using System.Numerics;

namespace Linearstar.Keystone.IO.MikuMikuMoving.Mvd
{
    public class MvdBoneFrame
    {
        public int StageId { get; set; }

        public long FrameTime { get; set; }

        public Vector3 Position { get; set; }

        public Quaternion Quaternion { get; set; } = Quaternion.Identity;

        public MvdInterpolationPair XInterpolation { get; set; } = MvdInterpolationPair.Default;

        public MvdInterpolationPair YInterpolation { get; set; } = MvdInterpolationPair.Default;

        public MvdInterpolationPair ZInterpolation { get; set; } = MvdInterpolationPair.Default;

        public MvdInterpolationPair RotationInterpolation { get; set; } = MvdInterpolationPair.Default;

        public bool Spline { get; set; }

        internal static MvdBoneFrame Parse(ref BufferReader br, MvdBoneData bd)
        {
            var rt = new MvdBoneFrame
            {
                StageId = br.ReadInt32(),
                FrameTime = br.ReadInt64(),
                Position = br.ReadVector3(),
                Quaternion = br.ReadQuaternion(),
                XInterpolation = MvdInterpolationPair.Parse(ref br),
                YInterpolation = MvdInterpolationPair.Parse(ref br),
                ZInterpolation = MvdInterpolationPair.Parse(ref br),
                RotationInterpolation = MvdInterpolationPair.Parse(ref br),
            };

            if (bd.MinorType < 1) return rt;
            
            rt.Spline = br.ReadBoolean();
            br.ReadBytes(3);

            return rt;
        }

        internal void Write(ref BufferWriter bw, MvdBoneData bd)
        {
            bw.Write(this.StageId);
            bw.Write(this.FrameTime);
            bw.Write(this.Position);
            bw.Write(this.Quaternion);
            this.XInterpolation.Write(ref bw);
            this.YInterpolation.Write(ref bw);
            this.ZInterpolation.Write(ref bw);
            this.RotationInterpolation.Write(ref bw);

            if (bd.MinorType >= 1)
            {
                bw.Write(this.Spline);
                bw.Write(new byte[] { 0, 0, 0 });
            }
        }

        public string GetName(MvdNameList names, MvdBoneData boneData)
        {
            if (this.StageId == 0)
                return names.Names[boneData.Key];
            else
            {
                var key = boneData.Key * -1000 - this.StageId;

                return names.Names.ContainsKey(key)
                    ? names.Names[key]
                    : this.StageId.ToString("000");
            }
        }
    }
}