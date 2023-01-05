namespace Linearstar.Keystone.IO.MikuMikuMoving.Mvd
{
    public class MvdEffectPropertyFrame : MvdAccessoryPropertyFrame
    {
        public MvdEffectParameterData[] Parameters { get; set; } = { };

        internal static MvdEffectPropertyFrame Parse(ref BufferReader br, MvdEffectPropertyData epd)
        {
            var frame = new MvdEffectPropertyFrame()
            {
                FrameTime = br.ReadInt64(),
                Visible = br.ReadBoolean(),
                Shadow = br.ReadBoolean(),
                AddBlending = br.ReadBoolean(),
                Reserved = br.ReadBoolean(),
                Scaling = br.ReadSingle(),
                Alpha = br.ReadSingle(),
                RelatedModelId = br.ReadInt32(),
                RelatedBoneId = br.ReadInt32(),
            };

            var parameters = new MvdEffectParameterData[epd.Parameters.Count];
            
            for (var i = 0; i < parameters.Length; i++)
                parameters[i] = MvdEffectParameterData.Parse(ref br, epd, i);
            
            frame.Parameters = parameters;

            return frame;
        }

        internal override void Write(ref BufferWriter bw)
        {
            base.Write(ref bw);

            foreach (var parameter in this.Parameters)
                parameter.Write(ref bw);
        }
    }
}