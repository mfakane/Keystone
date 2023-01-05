namespace Linearstar.Keystone.IO.MikuMikuMoving.Mvd
{
    public class MvdModelPropertyFrame
    {
        public long FrameTime { get; set; }

        public bool Visible { get; set; } = true;

        public bool Shadow { get; set; }

        public bool AddBlending { get; set; }

        public bool Physics { get; set; } = true;

        public bool PhysicsStillMode { get; set; }

        public float EdgeWidth { get; set; } = 1;

        public Color4B EdgeColor { get; set; }

        public float Scale { get; set; }

        public bool[] IKEnabled { get; set; } = { };

        public MvdModelRelation[] ModelRelation { get; set; } = { };

        internal static MvdModelPropertyFrame Parse(ref BufferReader br, MvdModelPropertyData mpd)
        {
            var rt = new MvdModelPropertyFrame
            {
                FrameTime = br.ReadInt64(),
                Visible = br.ReadBoolean(),
                Shadow = br.ReadBoolean(),
                AddBlending = br.ReadBoolean(),
                Physics = br.ReadBoolean(),
            };

            if (mpd.MinorType >= 1)
            {
                rt.PhysicsStillMode = br.ReadBoolean();
                br.ReadBytes(3); // reserved[3]
            }

            rt.EdgeWidth = br.ReadSingle();
            rt.EdgeColor = br.ReadColor4B();

            if (mpd.MinorType >= 2)
                rt.Scale = br.ReadSingle();

            var ikEnabled = new bool[mpd.IKBones.Length];
            
            for (var i = 0; i < ikEnabled.Length; i++)
                ikEnabled[i] = br.ReadBoolean();

            rt.IKEnabled = ikEnabled;

            if (mpd.MinorType < 3) return rt;
            
            var modelRelations = new MvdModelRelation[mpd.ModelRelationCount];
            
            for (var i = 0; i < modelRelations.Length; i++)
                modelRelations[i] = MvdModelRelation.Parse(ref br);

            rt.ModelRelation = modelRelations;

            return rt;
        }

        internal void Write(ref BufferWriter bw, MvdModelPropertyData mpd)
        {
            bw.Write(this.FrameTime);
            bw.Write(this.Visible);
            bw.Write(this.Shadow);
            bw.Write(this.AddBlending);
            bw.Write(this.Physics);

            if (mpd.MinorType >= 1)
            {
                bw.Write(this.PhysicsStillMode);
                bw.Write(new byte[3]); // reserved[3]
            }

            bw.Write(this.EdgeWidth);
            bw.Write(this.EdgeColor);

            if (mpd.MinorType >= 2)
                bw.Write(this.Scale);

            foreach (var ikEnabled in IKEnabled)
                bw.Write(ikEnabled);

            if (mpd.MinorType < 3) return;
            
            foreach (var modelRelation in this.ModelRelation)
                modelRelation.Write(ref bw);
        }
    }
}