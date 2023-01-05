using System.Collections.Generic;

namespace Linearstar.Keystone.IO.MikuMikuMoving.Mvd
{
    public class MvdNameList : MvdSection
    {
        public IDictionary<int, string> Names { get; set; } = new Dictionary<int, string>();

        public MvdNameList()
            : base(MvdTag.NameList)
        {
        }

        internal override void Read(ref BufferReader br, MvdDocument document, MvdObject obj)
        {
            for (var i = 0; i < this.RawCount; i++)
                this.Names.Add(br.ReadInt32(), br.ReadString(document));
        }

        internal override void Write(ref BufferWriter bw, MvdDocument document)
        {
            this.MinorType = 0;
            this.RawCount = this.Names.Count;

            base.Write(ref bw, document);

            foreach (var i in this.Names)
            {
                var buf = document.Encoding.GetBytes(i.Value);

                bw.Write(i.Key);
                bw.Write(buf.Length);
                bw.Write(buf);
            }
        }
    }
}