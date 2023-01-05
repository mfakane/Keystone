namespace Linearstar.Keystone.IO.MikuMikuDance.Pmx
{
    public class PmxMaterial
    {
        public string Name { get; set; }

        public string EnglishName { get; set; }

        /// <summary>
        /// r, g, b, a
        /// </summary>
        public Color4 Diffuse { get; set; }

        public Color3 Specular { get; set; }

        public float Power { get; set; }

        public Color3 Ambient { get; set; }

        public PmxMaterialOptions Options { get; set; }

        /// <summary>
        /// r, g, b, a
        /// </summary>
        public Color4 EdgeColor { get; set; }

        /// <summary>
        /// this.Options.HasFlag(PmxMaterialOptions.DrawPoint) ? point size : edge size
        /// </summary>
        public float EdgeSize { get; set; }

        public PmxTexture? MainTexture { get; set; }

        public PmxTexture? SubTexture { get; set; }

        public PmxTextureMode SubTextureMode { get; set; }

        public bool UseSharedToonTexture { get; set; }

        public int SharedToonTexture { get; set; }

        public PmxTexture? CustomToonTexture { get; set; }

        public string Comment { get; set; }

        public int IndexCount { get; set; }

        internal static PmxMaterial Parse(ref BufferReader br, PmxDocument doc)
        {
            var rt = new PmxMaterial
            {
                Name = br.ReadString(doc.Header),
                EnglishName = br.ReadString(doc.Header),
                Diffuse = br.ReadColor4(),
                Specular = br.ReadColor3(),
                Power = br.ReadSingle(),
                Ambient = br.ReadColor3(),
                Options = (PmxMaterialOptions)br.ReadByte(),
                EdgeColor = br.ReadColor4(),
                EdgeSize = br.ReadSingle(),
                MainTexture = doc.ReadTexture(ref br),
                SubTexture = doc.ReadTexture(ref br),
                SubTextureMode = (PmxTextureMode)br.ReadByte(),
                UseSharedToonTexture = br.ReadBoolean(),
            };

            if (rt.UseSharedToonTexture)
                rt.SharedToonTexture = br.ReadByte();
            else
                rt.CustomToonTexture = doc.ReadTexture(ref br);

            rt.Comment = br.ReadString(doc.Header);
            rt.IndexCount = br.ReadInt32();

            return rt;
        }

        internal void Write(ref BufferWriter bw, PmxDocument doc, PmxIndexCache cache)
        {
            bw.Write(this.Name, doc.Header);
            bw.Write(this.EnglishName, doc.Header);
            bw.Write(this.Diffuse);
            bw.Write(this.Specular);
            bw.Write(this.Power);
            bw.Write(this.Ambient);
            bw.Write((byte)this.Options);
            bw.Write(this.EdgeColor);
            bw.Write(this.EdgeSize);
            bw.Write(this.MainTexture, cache);
            bw.Write(this.SubTexture, cache);
            bw.Write((byte)this.SubTextureMode);
            bw.Write(this.UseSharedToonTexture);

            if (this.UseSharedToonTexture)
                bw.Write((byte)this.SharedToonTexture);
            else
                bw.Write(this.CustomToonTexture, cache);

            bw.Write(this.Comment, doc.Header);
            bw.Write(this.IndexCount);
        }
    }
}