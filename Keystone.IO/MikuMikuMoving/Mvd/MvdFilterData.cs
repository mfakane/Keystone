using System.Collections.Generic;

namespace Linearstar.Keystone.IO.MikuMikuMoving.Mvd
{
    public class MvdFilterData : MvdFixedItemSection
    {
        public IList<MvdFilterFrame> Frames { get; set; } = new List<MvdFilterFrame>();

        public int Key
        {
            get { return this.RawKey; }
            set { this.RawKey = value; }
        }

        public int ToneCurveControlPointCount { get; set; }

        public MvdFilterData()
            : base(MvdTag.Filter)
        {
        }

        internal override void ReadExtensionRegion(ref BufferReader br, MvdDocument document, MvdObject obj)
        {
            this.ToneCurveControlPointCount = br.ReadInt32();
        }

        internal override void ReadItem(ref BufferReader br, MvdDocument document, MvdObject obj)
        {
            this.Frames.Add(MvdFilterFrame.Parse(ref br));
        }

        internal override void WriteExtensionRegion(ref BufferWriter bw, MvdDocument document)
        {
            bw.Write(this.ToneCurveControlPointCount);
        }

        internal override void Write(ref BufferWriter bw, MvdDocument document)
        {
            this.MinorType = 2;
            this.RawCount = this.Frames.Count;

            base.Write(ref bw, document);
        }

        internal override void WriteItem(ref BufferWriter bw, MvdDocument document, int index)
        {
            this.Frames[index].Write(ref bw);
        }
    }
}