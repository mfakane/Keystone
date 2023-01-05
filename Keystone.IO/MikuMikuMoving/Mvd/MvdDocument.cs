using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Linearstar.Keystone.IO.MikuMikuMoving.Mvd
{
    /// <summary>
    /// Motion Vector Data File created by Mogg
    /// </summary>
    public class MvdDocument
    {
        public const string DisplayName = "Motion Vector Data file";
        public const string Filter = "*.mvd";

        public float Version { get; set; }

        public Encoding Encoding { get; set; }

        public IList<MvdObject> Objects { get; set; } = new List<MvdObject>();
        
        public static MvdDocument FromFile(string path)
        {
            using var fs = File.OpenRead(path);

            return Parse(fs);
        }
        
        public static MvdDocument Parse(in ReadOnlySequence<byte> sequence)
        {
            var rt = new MvdDocument();
            var br = new BufferReader(sequence);
            var header = br.ReadBytes(30);
            var headerConstant = "Motion Vector Data file"u8;

            if (!header.Slice(headerConstant.Length).SequenceEqual(header))
                throw new InvalidOperationException("invalid format");

            rt.Version = br.ReadSingle();

            if (rt.Version >= 2)
                throw new NotSupportedException("specified format version not supported");

            switch (br.ReadByte())
            {
                case 0:
                    rt.Encoding = Encoding.Unicode;

                    break;
                case 1:
                default:
                    rt.Encoding = Encoding.UTF8;

                    break;
            }

            while (!br.IsCompleted)
                rt.Objects.Add(MvdObject.Parse(ref br, rt));

            return rt;
        }

        public static MvdDocument Parse(Stream stream) =>
            Parse(stream.AsReadOnlySequence());

        public void Write(IBufferWriter<byte> writer)
        {
            var bw = new BufferWriter(writer);
            Span<byte> buf = stackalloc byte[30];
            var header = "Motion Vector Data file"u8;

            header.CopyTo(buf);
            bw.Write(buf);
            bw.Write(this.Version);
            bw.Write((byte)(this.Encoding.CodePage == Encoding.Unicode.CodePage ? 0 : 1));

            foreach (var i in this.Objects)
                i.Write(ref bw, this);
        }
		
        public void Write(Stream stream)
        {
            using var sbw = new StreamBufferWriter(stream);

            Write(sbw);
        }
    }
}