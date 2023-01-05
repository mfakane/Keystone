using System.Collections.Generic;
using System.Linq;

namespace Linearstar.Keystone.IO.Metasequoia
{
    /// <summary>
    /// Scene
    /// </summary>
    public class MqScene
    {
        MqChunk baseChunk;

        /// <summary>
        /// pos
        /// </summary>
        public float[] Position { get; set; }

        /// <summary>
        /// lookat
        /// </summary>
        public float[] LookAt { get; set; }

        /// <summary>
        /// head
        /// </summary>
        public float Head { get; set; }

        /// <summary>
        /// pich
        /// </summary>
        public float Pitch { get; set; }

        /// <summary>
        /// bank
        /// </summary>
        public float Bank { get; set; }

        /// <summary>
        /// ortho
        /// </summary>
        public bool Ortho { get; set; }

        /// <summary>
        /// zoom2
        /// </summary>
        public float Zoom2 { get; set; }

        /// <summary>
        /// amb
        /// </summary>
        public float[] Ambient { get; set; }

        /// <summary>
        /// dirlights
        /// </summary>
        public IList<MqDirectionalLight> DirectionalLights { get; set; }

        public MqScene()
        {
            this.Position = new[] { 0f, 0, 1500 };
            this.LookAt = new[] { 0f, 0, 0 };
            this.Head = -0.5236f;
            this.Pitch = 0.5236f;
            this.Zoom2 = 5;
            this.Ambient = new[] { 0.25f, 0.25f, 0.25f };
            this.DirectionalLights = new List<MqDirectionalLight>();
        }

        public static MqScene Parse(MqChunk chunk)
        {
            var rt = new MqScene
            {
                baseChunk = chunk,
            };

            foreach (var i in chunk.Children)
                switch (i.Name.ToLower())
                {
                    case "pos":
                        rt.Position = i.Arguments.Select(float.Parse).ToArray();

                        break;
                    case "lookat":
                        rt.LookAt = i.Arguments.Select(float.Parse).ToArray();

                        break;
                    case "head":
                        rt.Head = float.Parse(i.Arguments.First());

                        break;
                    case "pich":
                        rt.Pitch = float.Parse(i.Arguments.First());

                        break;
                    case "bank":
                        rt.Bank = float.Parse(i.Arguments.First());

                        break;
                    case "ortho":
                        rt.Ortho = i.Arguments.First() == "1";

                        break;
                    case "zoom2":
                        rt.Zoom2 = float.Parse(i.Arguments.First());

                        break;
                    case "amb":
                        rt.Ambient = i.Arguments.Select(float.Parse).ToArray();

                        break;
                    case "dirlights":
                        rt.DirectionalLights = i.Children.Select(MqDirectionalLight.Parse).ToList();

                        break;
                }

            return rt;
        }

        public MqChunk ToChunk()
        {
            baseChunk = baseChunk ?? new MqChunk();
            baseChunk.Name = "Scene";
            baseChunk.Child("pos").SetArguments(this.Position.Select(_ => _.ToString("0.0000")));
            baseChunk.Child("lookat").SetArguments(this.LookAt.Select(_ => _.ToString("0.0000")));
            baseChunk.Child("head").SetArguments(this.Head.ToString("0.0000"));
            baseChunk.Child("pich").SetArguments(this.Pitch.ToString("0.0000"));
            baseChunk.Child("bank").SetArguments(this.Bank.ToString("0.0000"));
            baseChunk.Child("ortho").SetArguments(this.Ortho ? "1" : "0");
            baseChunk.Child("zoom2").SetArguments(this.Zoom2.ToString("0.0000"));
            baseChunk.Child("amb").SetArguments(this.Ambient.Select(_ => _.ToString("0.000")));

            if (this.DirectionalLights.Any())
                baseChunk.Child("dirlights").SetArguments(this.DirectionalLights.Count.ToString())
                    .SetChildren(this.DirectionalLights.Select(_ => _.ToChunk()));
            else
                baseChunk.RemoveChildren("dirlights");

            return baseChunk;
        }

        public string GetFormattedText()
        {
            return this.ToChunk().GetFormattedText();
        }
    }
}