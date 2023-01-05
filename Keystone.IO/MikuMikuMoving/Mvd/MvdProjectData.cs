using System.Collections.Generic;

namespace Linearstar.Keystone.IO.MikuMikuMoving.Mvd
{
    public class MvdProjectData : MvdFixedItemSection
    {
        public IList<MvdProjectFrame> Frames { get; set; } = new List<MvdProjectFrame>();

        public MvdProjectData()
            : base(MvdTag.Project)
        {
        }

        internal override void ReadItem(ref BufferReader br, MvdDocument document, MvdObject obj)
        {
            this.Frames.Add(MvdProjectFrame.Parse(ref br, this));
        }

        internal override void Write(ref BufferWriter bw, MvdDocument document)
        {
            this.MinorType = 1;
            this.RawCount = this.Frames.Count;

            base.Write(ref bw, document);
        }

        internal override void WriteItem(ref BufferWriter bw, MvdDocument document, int index)
        {
            this.Frames[index].Write(ref bw, this);
        }
    }
}