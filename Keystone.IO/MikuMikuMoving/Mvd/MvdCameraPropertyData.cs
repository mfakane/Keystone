using System.Collections.Generic;

namespace Linearstar.Keystone.IO.MikuMikuMoving.Mvd
{
    public class MvdCameraPropertyData : MvdFixedItemSection
    {
        public IList<MvdCameraPropertyFrame> Frames { get; set; } = new List<MvdCameraPropertyFrame>();

        public MvdCameraPropertyData()
            : base(MvdTag.CameraProperty)
        {
        }

        internal override void ReadItem(ref BufferReader br, MvdDocument document, MvdObject obj)
        {
            this.Frames.Add(MvdCameraPropertyFrame.Parse(ref br, this));
        }

        internal override void Write(ref BufferWriter bw, MvdDocument document)
        {
            this.MinorType = 2;
            this.RawCount = this.Frames.Count;

            base.Write(ref bw, document);
        }

        internal override void WriteItem(ref BufferWriter bw, MvdDocument document, int index)
        {
            this.Frames[index].Write(ref bw, this);
        }
    }
}