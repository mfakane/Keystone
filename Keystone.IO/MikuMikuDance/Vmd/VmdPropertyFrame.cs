using System.Collections.Generic;

namespace Linearstar.Keystone.IO.MikuMikuDance.Vmd
{
    public class VmdPropertyFrame
    {
        public uint FrameTime { get; set; }

        public bool IsVisible { get; set; }

        public IDictionary<string, bool> IKEnabled { get; set; } = new Dictionary<string, bool>();

        internal static VmdPropertyFrame Parse(ref BufferReader br)
        {
            var rt = new VmdPropertyFrame()
            {
                FrameTime = br.ReadUInt32(),
                IsVisible = br.ReadByte() != 0,
            };

            var iks = br.ReadUInt32();
            for (uint i = 0; i < iks; i++)
            {
                var bone = br.ReadString(20);
                var enabled = br.ReadInt32();
                
                rt.IKEnabled[bone] = enabled != 0;
            }

            return rt;
        }

        internal void Write(ref BufferWriter bw)
        {
            bw.Write(this.FrameTime);
            bw.Write(this.IsVisible);
            
            bw.Write(this.IKEnabled.Count);
            foreach (var ikEnabled in this.IKEnabled)
            {
                bw.Write(ikEnabled.Key, 20);
                bw.Write(ikEnabled.Value);
            }
        }
    }
}