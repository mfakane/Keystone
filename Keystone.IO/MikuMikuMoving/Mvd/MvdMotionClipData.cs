namespace Linearstar.Keystone.IO.MikuMikuMoving.Mvd
{
    public class MvdMotionClipData : MvdFixedItemSection
    {
        // MMM が RawItemSize と実際の 1 項目ごとの長さが一致しないファイルを吐くので、RawItemSize を信用してはいけない
        protected override bool IgnoreRawItemSize => true;

        public int Key
        {
            get { return this.RawKey; }
            set { this.RawKey = value; }
        }

        public int ParentClipId { get; set; }

        public MvdMotionClip MotionClip { get; set; }

        public MvdMotionClipData()
            : base(MvdTag.MotionClip)
        {
        }

        internal override void Read(ref BufferReader bw, MvdDocument document, MvdObject obj)
        {
            // フォーマットバグ対策
            if (this.MinorType == 0)
                this.RawItemSize -= 4;

            base.Read(ref bw, document, obj);
        }

        internal override void ReadExtensionRegion(ref BufferReader br, MvdDocument document, MvdObject obj)
        {
            this.ParentClipId = br.ReadInt32();
            br.ReadInt32();
        }

        internal override void ReadItem(ref BufferReader br, MvdDocument document, MvdObject obj)
        {
            this.MotionClip = MvdMotionClip.Parse(ref br);
        }

        internal override void WriteExtensionRegion(ref BufferWriter bw, MvdDocument document)
        {
            bw.Write(this.ParentClipId);
        }

        internal override void Write(ref BufferWriter bw, MvdDocument document)
        {
            this.MinorType = 0;
            this.RawCount = 1;

            base.Write(ref bw, document);
        }

        internal override void BeforeWriteHeader(ref BufferWriter bw, MvdDocument document)
        {
            // フォーマットバグ対策
            if (this.MinorType == 0)
                this.RawItemSize += 4;
        }

        internal override void WriteItem(ref BufferWriter bw, MvdDocument document, int index)
        {
            this.MotionClip.Write(ref bw);
        }
    }
}