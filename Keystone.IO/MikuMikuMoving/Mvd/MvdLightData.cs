using System.Collections.Generic;

namespace Linearstar.Keystone.IO.MikuMikuMoving.Mvd
{
    public class MvdLightData : MvdFixedItemSection
    {
        public IList<MvdLightFrame> Frames { get; } = new List<MvdLightFrame>();

        public MvdLightData()
            : base(MvdTag.Light)
        {
        }

        internal override void ReadItem(ref BufferReader br, MvdDocument document, MvdObject obj)
        {
            this.Frames.Add(MvdLightFrame.Parse(ref br));
        }

        internal override void Write(ref BufferWriter bw, MvdDocument document)
        {
            this.MinorType = 0;
            this.RawCount = this.Frames.Count;

            base.Write(ref bw, document);
        }

        internal override void WriteItem(ref BufferWriter bw, MvdDocument document, int index)
        {
            this.Frames[index].Write(ref bw);
        }
    }
}