using System.Collections.Generic;

namespace Linearstar.Keystone.IO.MikuMikuMoving.Mvd
{
    public class MvdBoneData : MvdFixedItemSection
    {
        public IList<MvdBoneFrame> Frames { get; set; } = new List<MvdBoneFrame>();

        public int Key
        {
            get { return this.RawKey; }
            set { this.RawKey = value; }
        }

        public int StageCount { get; set; }

        public int ParentClipId { get; set; }

        public MvdBoneData()
            : base(MvdTag.Bone)
        {
        }

        internal override void ReadExtensionRegion(ref BufferReader br, MvdDocument document, MvdObject obj)
        {
            if (!br.IsCompleted)
                this.StageCount = br.ReadInt32();

            if (this.MinorType >= 2)
                this.ParentClipId = br.ReadInt32();
        }

        internal override void ReadItem(ref BufferReader br, MvdDocument document, MvdObject obj)
        {
            this.Frames.Add(MvdBoneFrame.Parse(ref br, this));
        }

        internal override void WriteExtensionRegion(ref BufferWriter bw, MvdDocument document)
        {
            bw.Write(this.StageCount);

            if (this.MinorType >= 2)
                bw.Write(this.ParentClipId);
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