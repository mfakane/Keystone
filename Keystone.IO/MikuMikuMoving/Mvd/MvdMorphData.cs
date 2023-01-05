using System.Collections.Generic;

namespace Linearstar.Keystone.IO.MikuMikuMoving.Mvd
{
    public class MvdMorphData : MvdFixedItemSection
    {
        public IList<MvdMorphFrame> Frames { get; set; } = new List<MvdMorphFrame>();

        public int Key
        {
            get { return this.RawKey; }
            set { this.RawKey = value; }
        }

        public int ParentClipId { get; set; }

        public MvdMorphData()
            : base(MvdTag.Morph)
        {
        }

        internal override void ReadExtensionRegion(ref BufferReader br, MvdDocument document, MvdObject obj)
        {
            if (this.MinorType >= 1)
                this.ParentClipId = br.ReadInt32();
        }

        internal override void ReadItem(ref BufferReader br, MvdDocument document, MvdObject obj)
        {
            this.Frames.Add(MvdMorphFrame.Parse(ref br));
        }

        internal override void WriteExtensionRegion(ref BufferWriter bw, MvdDocument document)
        {
            if (this.MinorType >= 1)
                bw.Write(this.ParentClipId);
        }

        internal override void Write(ref BufferWriter bw, MvdDocument document)
        {
            this.MinorType = 1;
            this.RawCount = this.Frames.Count;

            base.Write(ref bw, document);
        }

        internal override void WriteItem(ref BufferWriter bw, MvdDocument document, int index)
        {
            this.Frames[index].Write(ref bw);
        }
    }
}