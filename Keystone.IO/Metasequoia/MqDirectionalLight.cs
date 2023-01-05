using System.Linq;

namespace Linearstar.Keystone.IO.Metasequoia
{
    /// <summary>
    /// light
    /// </summary>
    public class MqDirectionalLight
    {
        MqChunk baseChunk;

        /// <summary>
        /// dir
        /// </summary>
        public float[] Direction { get; set; } = { 0.408f, 0.408f, 0.816f };

        /// <summary>
        /// color
        /// </summary>
        public float[] Color { get; set; } = { 1, 1, 1 };

        public static MqDirectionalLight Parse(MqChunk chunk)
        {
            var rt = new MqDirectionalLight
            {
                baseChunk = chunk,
            };

            foreach (var i in chunk.Children)
                switch (i.Name.ToLower())
                {
                    case "dir":
                        rt.Direction = i.Arguments.Select(float.Parse).ToArray();

                        break;
                    case "color":
                        rt.Color = i.Arguments.Select(float.Parse).ToArray();

                        break;
                }

            return rt;
        }

        public MqChunk ToChunk()
        {
            baseChunk = baseChunk ?? new MqChunk();
            baseChunk.Child("dir").SetArguments(this.Direction.Select(_ => _.ToString("0.000")));
            baseChunk.Child("color").SetArguments(this.Color.Select(_ => _.ToString("0.000")));

            return baseChunk;
        }

        public string GetFormattedText()
        {
            return this.ToChunk().GetFormattedText();
        }
    }
}