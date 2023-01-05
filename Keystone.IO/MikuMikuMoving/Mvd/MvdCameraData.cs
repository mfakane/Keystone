using System.Collections.Generic;
using System.Linq;

namespace Linearstar.Keystone.IO.MikuMikuMoving.Mvd
{
    public class MvdCameraData : MvdFixedItemSection
    {
        public IList<MvdCameraFrame> Frames { get; set; }

        public int Key
        {
            get { return this.RawKey; }
            set { this.RawKey = value; }
        }

        public int StageCount { get; set; }

        public MvdCameraData()
            : base(MvdTag.Camera)
        {
            this.Frames = new List<MvdCameraFrame>();
        }

        internal override void ReadItem(ref BufferReader br, MvdDocument document, MvdObject obj)
        {
            this.Frames.Add(MvdCameraFrame.Parse(ref br, this, out var cpf));

            if (cpf == null) return;
            
            var cpd = obj.Sections.OfType<MvdCameraPropertyData>().FirstOrDefault();

            if (cpd == null)
                obj.Sections.Add(cpd = new MvdCameraPropertyData());

            cpd.Frames.Add(cpf);
        }

        internal override void ReadExtensionRegion(ref BufferReader br, MvdDocument document, MvdObject obj)
        {
            if (!br.IsCompleted)
                this.StageCount = br.ReadInt32();
        }

        internal override void Write(ref BufferWriter bw, MvdDocument document)
        {
            this.MinorType = 3;
            this.RawCount = this.Frames.Count;

            base.Write(ref bw, document);
        }

        internal override void WriteExtensionRegion(ref BufferWriter bw, MvdDocument document)
        {
            bw.Write(this.StageCount);
        }

        internal override void WriteItem(ref BufferWriter bw, MvdDocument document, int index)
        {
            this.Frames[index].Write(ref bw, this);
        }
    }
}