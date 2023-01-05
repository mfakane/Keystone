namespace Linearstar.Keystone.IO.MikuMikuMoving.Mvd
{
    public class MvdCameraPropertyFrame
    {
        public long FrameTime { get; set; }

        public bool Perspective { get; set; } = true;

        public bool Enabled { get; set; } = true;

        public float Alpha { get; set; } = 1;

        public bool EffectEnabled { get; set; } = true;

        public bool DynamicFov { get; set; } = false;

        public float DynamicFovRate { get; set; } = 0.1f;

        public float DynamicFovCoefficent { get; set; } = 1;

        public int RelatedModelId { get; set; } = -1;

        public int RelatedBoneId { get; set; } = -1;

        internal static MvdCameraPropertyFrame Parse(ref BufferReader br, MvdCameraPropertyData cpd)
        {
            var rt = new MvdCameraPropertyFrame
            {
                FrameTime = br.ReadInt64(),
                Enabled = br.ReadBoolean(),
                Perspective = br.ReadBoolean(),
                Alpha = br.ReadSingle(),
                EffectEnabled = br.ReadBoolean(),
            };

            switch (cpd.MinorType)
            {
                case 0:
                    br.ReadByte();

                    break;
                case 1:
                case 2:
                    rt.DynamicFov = br.ReadBoolean();
                    rt.DynamicFovRate = br.ReadSingle();
                    rt.DynamicFovCoefficent = br.ReadSingle();

                    if (cpd.MinorType == 2)
                    {
                        rt.RelatedModelId = br.ReadInt32();
                        rt.RelatedBoneId = br.ReadInt32();
                    }

                    break;
            }

            return rt;
        }

        internal void Write(ref BufferWriter bw, MvdCameraPropertyData cpd)
        {
            bw.Write(this.FrameTime);
            bw.Write(this.Enabled);
            bw.Write(this.Perspective);
            bw.Write(this.Alpha);
            bw.Write(this.EffectEnabled);

            switch (cpd.MinorType)
            {
                case 0:
                    bw.Write(false);

                    break;
                case 1:
                case 2:
                    bw.Write(this.DynamicFov);
                    bw.Write(this.DynamicFovRate);
                    bw.Write(this.DynamicFovCoefficent);

                    if (cpd.MinorType == 2)
                    {
                        bw.Write(this.RelatedModelId);
                        bw.Write(this.RelatedBoneId);
                    }

                    break;
            }
        }
    }
}