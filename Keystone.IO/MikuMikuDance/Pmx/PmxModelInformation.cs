namespace Linearstar.Keystone.IO.MikuMikuDance.Pmx
{
    public class PmxModelInformation
    {
        public string ModelName { get; set; }

        public string EnglishModelName { get; set; }

        public string Description { get; set; }

        public string EnglishDescription { get; set; }

        internal static PmxModelInformation Parse(ref BufferReader br, PmxDocument doc) =>
            new()
            {
                ModelName = br.ReadString(doc.Header),
                EnglishModelName = br.ReadString(doc.Header),
                Description = br.ReadString(doc.Header),
                EnglishDescription = br.ReadString(doc.Header),
            };

        internal void Write(ref BufferWriter bw, PmxDocument doc)
        {
            bw.Write(this.ModelName, doc.Header);
            bw.Write(this.EnglishModelName, doc.Header);
            bw.Write(this.Description, doc.Header);
            bw.Write(this.EnglishDescription, doc.Header);
        }
    }
}