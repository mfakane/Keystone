namespace Linearstar.Keystone.IO.MikuMikuMoving.Mvd
{
    public class MvdMotionBlendLinkData : MvdFixedItemSection
    {
        public int Key
        {
            get { return this.RawKey; }
            set { this.RawKey = value; }
        }

        public int ParentClipId { get; set; }

        public MvdMotionBlendLink MotionBlendLink { get; set; }

        public MvdMotionBlendLinkData()
            : base(MvdTag.MotionBlend)
        {
        }

        internal override void ReadExtensionRegion(ref BufferReader br, MvdDocument document, MvdObject obj)
        {
            this.ParentClipId = br.ReadInt32();
        }

        internal override void ReadItem(ref BufferReader br, MvdDocument document, MvdObject obj)
        {
            this.MotionBlendLink = MvdMotionBlendLink.Parse(ref br);
        }

        internal override void WriteExtensionRegion(ref BufferWriter bw, MvdDocument document)
        {
            bw.Write(this.ParentClipId);
        }

        internal override void Write(ref BufferWriter bw, MvdDocument document)
        {
            this.MinorType = 1;
            this.RawCount = 1;

            base.Write(ref bw, document);
        }

        internal override void WriteItem(ref BufferWriter bw, MvdDocument document, int index)
        {
            this.MotionBlendLink.Write(ref bw);
        }
    }
}