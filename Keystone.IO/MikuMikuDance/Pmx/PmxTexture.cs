namespace Linearstar.Keystone.IO.MikuMikuDance.Pmx
{
    public class PmxTexture
    {
        public string FileName { get; set; } = "";

        internal static PmxTexture Parse(ref BufferReader br, PmxDocument doc) =>
            new()
            {
                FileName = br.ReadString(doc.Header),
            };

        internal void Write(ref BufferWriter bw, PmxDocument doc) =>
            bw.Write(this.FileName, doc.Header);
    }
}