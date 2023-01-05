using System.Collections.Generic;

namespace Linearstar.Keystone.IO.MikuMikuMoving.Mvd
{
    public class MvdModelPropertyData : MvdFixedItemSection
    {
        public IList<MvdModelPropertyFrame> Frames { get; set; } = new List<MvdModelPropertyFrame>();

        public int[] IKBones { get; set; } = { };

        public int ModelRelationCount { get; set; }

        public MvdModelPropertyData()
            : base(MvdTag.ModelProperty)
        {
        }

        internal override void Read(ref BufferReader bw, MvdDocument document, MvdObject obj)
        {
            // フォーマットバグ対策
            if (this.MinorType >= 1)
                this.RawItemSize += 4;

            base.Read(ref bw, document, obj);
        }

        internal override void ReadExtensionRegion(ref BufferReader br, MvdDocument document, MvdObject obj)
        {
            if (!br.IsCompleted)
            {
                var ikBones = new int[br.ReadInt32()];

                for (var i = 0; i < ikBones.Length; i++)
                    ikBones[i] = br.ReadInt32();
                
                this.IKBones = ikBones;
            }

            if (this.MinorType >= 3)
                this.ModelRelationCount = br.ReadInt32();
        }

        internal override void ReadItem(ref BufferReader br, MvdDocument document, MvdObject obj)
        {
            this.Frames.Add(MvdModelPropertyFrame.Parse(ref br, this));
        }

        internal override void WriteExtensionRegion(ref BufferWriter bw, MvdDocument document)
        {
            bw.Write(this.IKBones.Length);

            foreach (var ikBone in this.IKBones)
                bw.Write(ikBone);

            if (this.MinorType >= 3)
                bw.Write(this.ModelRelationCount);
        }

        internal override void Write(ref BufferWriter bw, MvdDocument document)
        {
            this.MinorType = 3;
            this.RawCount = this.Frames.Count;

            base.Write(ref bw, document);
        }

        internal override void BeforeWriteHeader(ref BufferWriter bw, MvdDocument document)
        {
            // フォーマットバグ対策
            if (this.MinorType >= 1)
                this.RawItemSize -= 4;
        }

        internal override void WriteItem(ref BufferWriter bw, MvdDocument document, int index)
        {
            this.Frames[index].Write(ref bw, this);
        }
    }
}