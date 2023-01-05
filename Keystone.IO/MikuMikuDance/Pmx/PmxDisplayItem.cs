using System;

namespace Linearstar.Keystone.IO.MikuMikuDance.Pmx
{
    public abstract class PmxDisplayItem
    {
        public abstract PmxDisplayItemKind Kind { get; }

        internal static PmxDisplayItem Parse(ref BufferReader br, PmxDocument doc)
        {
            switch ((PmxDisplayItemKind)br.ReadByte())
            {
                case PmxDisplayItemKind.Bone:
                    return new PmxBoneDisplayItem
                    {
                        Bone = doc.ReadBone(ref br),
                    };
                case PmxDisplayItemKind.Morph:
                    return new PmxMorphDisplayItem
                    {
                        Morph = doc.ReadMorph(ref br),
                    };
                default:
                    throw new InvalidOperationException();
            }
        }

        internal virtual void Write(ref BufferWriter bw, PmxDocument doc, PmxIndexCache cache)
        {
            bw.Write((byte)this.Kind);
        }
    }

    public class PmxBoneDisplayItem : PmxDisplayItem
    {
        public override PmxDisplayItemKind Kind => PmxDisplayItemKind.Bone;

        public PmxBone? Bone { get; set; }

        internal override void Write(ref BufferWriter bw, PmxDocument doc, PmxIndexCache cache)
        {
            base.Write(ref bw, doc, cache);
            bw.Write(this.Bone, cache);
        }
    }

    public class PmxMorphDisplayItem : PmxDisplayItem
    {
        public override PmxDisplayItemKind Kind => PmxDisplayItemKind.Morph;

        public PmxMorph? Morph { get; set; }

        internal override void Write(ref BufferWriter bw, PmxDocument doc, PmxIndexCache cache)
        {
            base.Write(ref bw, doc, cache);
            bw.Write(this.Morph, cache);
        }
    }
}