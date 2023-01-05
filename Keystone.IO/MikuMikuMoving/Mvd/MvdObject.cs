using System.Collections.Generic;

namespace Linearstar.Keystone.IO.MikuMikuMoving.Mvd
{
    public class MvdObject
    {
        public string ObjectName { get; set; }

        public string EnglishObjectName { get; set; }

        public float KeyFps { get; set; }

        public IList<MvdSection> Sections { get; set; } = new List<MvdSection>();

        internal static MvdObject Parse(ref BufferReader br, MvdDocument document)
        {
            var rt = new MvdObject
            {
                ObjectName = br.ReadString(document),
                EnglishObjectName = br.ReadString(document),
                KeyFps = br.ReadSingle(),
            };

            br.ReadBytes(br.ReadInt32()); // reservedSize / reserved

            while (!br.IsCompleted)
            {
                var section = MvdSection.Parse(ref br, document, rt);

                if (section == null)
                    break;

                rt.Sections.Add(section);
            }

            return rt;
        }

        internal void Write(ref BufferWriter bw, MvdDocument document)
        {
            bw.Write(this.ObjectName, document);
            bw.Write(this.EnglishObjectName, document);
            bw.Write(this.KeyFps);
            bw.Write("", document);

            foreach (var i in this.Sections)
                i.Write(ref bw, document);

            bw.Write((byte)MvdTag.Eof);
            bw.Write((byte)0);
        }
    }
}