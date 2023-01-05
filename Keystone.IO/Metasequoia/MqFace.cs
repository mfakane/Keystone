using System.Linq;

namespace Linearstar.Keystone.IO.Metasequoia
{
    /// <summary>
    /// face
    /// </summary>
    public class MqFace
    {
        MqChunk baseChunk;

        /// <summary>
        /// V
        /// </summary>
        public int[] Vertices { get; set; }

        /// <summary>
        /// M
        /// </summary>
        public int Material { get; set; }

        /// <summary>
        /// UV
        /// </summary>
        public float[][] UV { get; set; }

        /// <summary>
        /// COL
        /// </summary>
        public MqVertexColor[] VertexColor { get; set; }

        public static MqFace Parse(MqChunk chunk)
        {
            var rt = new MqFace
            {
                baseChunk = chunk,
            };

            foreach (var i in chunk.Attributes)
                switch (i.Name.ToLower())
                {
                    case "v":
                        rt.Vertices = i.Arguments.Select(int.Parse).ToArray();

                        break;
                    case "m":
                        rt.Material = int.Parse(i.Arguments.First());

                        break;
                    case "uv":
                        rt.UV = i.Arguments.Select(float.Parse).Buffer(2).Select(_ => _.ToArray()).ToArray();

                        break;
                    case "col":
                        rt.VertexColor = i.Arguments.Select(uint.Parse).Select(_ =>
                            new MqVertexColor((byte)(_ & 0xFF), (byte)((_ >> 8) & 0xFF), (byte)((_ >> 16) & 0xFF),
                                (byte)((_ >> 24) & 0xFF))).ToArray();

                        break;
                }

            return rt;
        }

        public MqChunk ToChunk()
        {
            baseChunk = baseChunk ?? new MqChunk();
            baseChunk.SetArguments(this.Vertices.Length.ToString());
            baseChunk.SetAttributes(new[]
                {
                    new MqChunkAttribute("V", this.Vertices.Select(_ => _.ToString())),
                    new MqChunkAttribute("M", this.Material.ToString()),
                    this.UV == null
                        ? null
                        : new MqChunkAttribute("UV", this.UV.SelectMany(_ => _).Select(f => f.ToString("0.00000"))),
                    this.VertexColor == null
                        ? null
                        : new MqChunkAttribute("COL", this.VertexColor.Select(_ => _.ToString())),
                }
                .Where(_ => _ != null));

            return baseChunk;
        }

        public string GetFormattedText()
        {
            return this.ToChunk().GetFormattedText();
        }
    }
}