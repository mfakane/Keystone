using System.Collections.Generic;

namespace Linearstar.Keystone.IO.MikuMikuDance.Pmx
{
    public class PmxMorph
    {
        public string Name { get; set; }

        public string EnglishName { get; set; }

        public PmxMorphCategory Category { get; set; }

        public PmxMorphKind Kind { get; set; }

        public IList<PmxMorphOffset> Offsets { get; set; } = new List<PmxMorphOffset>();

        internal void Parse(ref BufferReader br, PmxDocument doc)
        {
            this.Name = br.ReadString(doc.Header);
            this.EnglishName = br.ReadString(doc.Header);
            this.Category = (PmxMorphCategory)br.ReadByte();
            this.Kind = (PmxMorphKind)br.ReadByte();

            var count = br.ReadInt32();
            for (var i = 0; i < count; i++)
                this.Offsets.Add(PmxMorphOffset.Parse(ref br, doc, this.Kind));
        }

        internal void Write(ref BufferWriter bw, PmxDocument doc, PmxIndexCache cache)
        {
            if (doc.Version < 2.1f)
                if (this.Kind == PmxMorphKind.Flip)
                    this.Kind = PmxMorphKind.Group;
                else if (this.Kind == PmxMorphKind.Impulse)
                    return;

            bw.Write(this.Name, doc.Header);
            bw.Write(this.EnglishName, doc.Header);
            bw.Write((byte)this.Category);
            bw.Write((byte)this.Kind);
            bw.Write(this.Offsets.Count);
            foreach (var offset in this.Offsets) offset.Write(ref bw, doc, cache);
        }
    }
}