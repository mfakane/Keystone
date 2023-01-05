using System.Collections.Generic;

namespace Linearstar.Keystone.IO.MikuMikuMoving.Mvd
{
    public class MvdEffectPropertyData : MvdFixedItemSection
    {
        public IList<MvdEffectPropertyFrame> Frames { get; set; } = new List<MvdEffectPropertyFrame>();

        public IList<MvdEffectParameter> Parameters { get; set; } = new List<MvdEffectParameter>();

        public MvdEffectPropertyData()
            : base(MvdTag.EffectProperty)
        {
        }

        internal override void ReadExtensionRegion(ref BufferReader br, MvdDocument document, MvdObject obj)
        {
            if (!br.IsCompleted) return;

            for (var i = br.ReadInt32(); i > 0; i--)
                this.Parameters.Add(MvdEffectParameter.Parse(ref br));
        }

        internal override void ReadItem(ref BufferReader br, MvdDocument document, MvdObject obj)
        {
            this.Frames.Add(MvdEffectPropertyFrame.Parse(ref br, this));
        }

        internal override void WriteExtensionRegion(ref BufferWriter bw, MvdDocument document)
        {
            bw.Write(this.Parameters.Count);
            
            foreach (var parameter in this.Parameters)
                parameter.Write(ref bw);
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