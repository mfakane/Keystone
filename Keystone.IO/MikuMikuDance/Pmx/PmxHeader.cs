using System.Text;

namespace Linearstar.Keystone.IO.MikuMikuDance.Pmx
{
    public class PmxHeader
    {
        public Encoding Encoding { get; set; }

        public byte AdditionalUVCount { get; set; }

        public PmxVertexIndexSize VertexIndexSize { get; set; }

        public PmxIndexSize TextureIndexSize { get; set; }

        public PmxIndexSize MaterialIndexSize { get; set; }

        public PmxIndexSize BoneIndexSize { get; set; }

        public PmxIndexSize MorphIndexSize { get; set; }

        public PmxIndexSize RigidIndexSize { get; set; }

        public PmxHeader()
        {
            this.Encoding = Encoding.Unicode;
        }

        internal static PmxHeader Parse(ref BufferReader br)
        {
            var bytes = br.ReadBytes(br.ReadByte());

            return new PmxHeader
            {
                Encoding = new[] { Encoding.Unicode, Encoding.UTF8 }[bytes[0]],
                AdditionalUVCount = bytes[1],
                VertexIndexSize = (PmxVertexIndexSize)bytes[2],
                TextureIndexSize = (PmxIndexSize)bytes[3],
                MaterialIndexSize = (PmxIndexSize)bytes[4],
                BoneIndexSize = (PmxIndexSize)bytes[5],
                MorphIndexSize = (PmxIndexSize)bytes[6],
                RigidIndexSize = (PmxIndexSize)bytes[7],
            };
        }

        internal void Write(ref BufferWriter bw)
        {
            var data = new[]
            {
                this.Encoding.CodePage == Encoding.Unicode.CodePage ? (byte)0 : (byte)1,
                this.AdditionalUVCount,
                (byte)this.VertexIndexSize,
                (byte)this.TextureIndexSize,
                (byte)this.MaterialIndexSize,
                (byte)this.BoneIndexSize,
                (byte)this.MorphIndexSize,
                (byte)this.RigidIndexSize,
            };

            bw.Write((byte)data.Length);
            bw.Write(data);
        }
    }
}