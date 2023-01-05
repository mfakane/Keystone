using System.Collections.Generic;
using System.Linq;

namespace Linearstar.Keystone.IO.Metasequoia
{
    /// <summary>
    /// Object
    /// </summary>
    public class MqObject
    {
        MqChunk baseChunk;

        public string Name { get; set; }

        /// <summary>
        /// depth
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        /// folding
        /// </summary>
        public bool Folding { get; set; }

        /// <summary>
        /// scale
        /// </summary>
        public float[] Scale { get; set; } = { 1, 1, 1 };

        /// <summary>
        /// rotation
        /// </summary>
        public float[] Rotation { get; set; } = { 0, 0, 0 };

        /// <summary>
        /// translation
        /// </summary>
        public float[] Translation { get; set; } = { 0, 0, 0 };

        /// <summary>
        /// patch
        /// </summary>
        public MqPatchType Patch { get; set; }

        /// <summary>
        /// segment
        /// </summary>
        public int Segment { get; set; }

        /// <summary>
        /// visible
        /// </summary>
        public bool Visible { get; set; } = true;

        /// <summary>
        /// locking
        /// </summary>
        public bool Locking { get; set; }

        /// <summary>
        /// shading
        /// </summary>
        public MqShading Shading { get; set; } = MqShading.Gouraud;

        /// <summary>
        /// facet
        /// </summary>
        public float Facet { get; set; }

        /// <summary>
        /// color
        /// </summary>
        public float[] Color { get; set; } = { 0.502f, 0.502f, 0.502f };

        /// <summary>
        /// color_type
        /// </summary>
        public bool ColorType { get; set; }

        /// <summary>
        /// mirror
        /// </summary>
        public MqMirrorType Mirror { get; set; }

        /// <summary>
        /// mirror_axis
        /// </summary>
        public MqMirrorAxis MirrorAxis { get; set; }

        /// <summary>
        /// mirror_dis
        /// </summary>
        public float MirrorDistance { get; set; }

        /// <summary>
        /// lathe
        /// </summary>
        public MqLatheType Lathe { get; set; }

        /// <summary>
        /// lathe_axis
        /// </summary>
        public MqLatheAxis LatheAxis { get; set; }

        /// <summary>
        /// lathe_seg
        /// </summary>
        public int LatheSegments { get; set; }

        /// <summary>
        /// vertex
        /// </summary>
        public IList<float[]> Vertex { get; private set; } = new List<float[]>();

        /// <summary>
        /// weit
        /// </summary>
        public IDictionary<int, float> Weight { get; private set; } = new Dictionary<int, float>();

        /// <summary>
        /// face
        /// </summary>
        public IList<MqFace> Faces { get; private set; } = new List<MqFace>();

        public static MqObject Parse(MqChunk chunk)
        {
            var rt = new MqObject
            {
                Name = chunk.Arguments.First().Trim('"'),
            };

            foreach (var i in chunk.Children)
                switch (i.Name.ToLower())
                {
                    case "depth":
                        rt.Depth = int.Parse(i.Arguments.First());

                        break;
                    case "folding":
                        rt.Folding = i.Arguments.First() == "1";

                        break;
                    case "scale":
                        rt.Scale = i.Arguments.Select(float.Parse).ToArray();

                        break;
                    case "rotation":
                        rt.Rotation = i.Arguments.Select(float.Parse).ToArray();

                        break;
                    case "translation":
                        rt.Translation = i.Arguments.Select(float.Parse).ToArray();

                        break;
                    case "patch":
                        rt.Patch = (MqPatchType)int.Parse(i.Arguments.First());

                        break;
                    case "segment":
                        rt.Segment = int.Parse(i.Arguments.First());

                        break;
                    case "visible":
                        rt.Visible = i.Arguments.First() == "15";

                        break;
                    case "locking":
                        rt.Locking = i.Arguments.First() == "1";

                        break;
                    case "shading":
                        rt.Shading = (MqShading)int.Parse(i.Arguments.First());

                        break;
                    case "facet":
                        rt.Facet = float.Parse(i.Arguments.First());

                        break;
                    case "color":
                        rt.Color = i.Arguments.Select(float.Parse).ToArray();

                        break;
                    case "color_type":
                        rt.ColorType = i.Arguments.First() == "1";

                        break;
                    case "mirror":
                        rt.Mirror = (MqMirrorType)int.Parse(i.Arguments.First());

                        break;
                    case "mirror_axis":
                        rt.MirrorAxis = (MqMirrorAxis)int.Parse(i.Arguments.First());

                        break;
                    case "mirror_dis":
                        rt.MirrorDistance = float.Parse(i.Arguments.First());

                        break;
                    case "lathe":
                        rt.Lathe = (MqLatheType)int.Parse(i.Arguments.First());

                        break;
                    case "lathe_axis":
                        rt.LatheAxis = (MqLatheAxis)int.Parse(i.Arguments.First());

                        break;
                    case "lathe_seg":
                        rt.LatheSegments = int.Parse(i.Arguments.First());

                        break;
                    case "vertex":
                        rt.Vertex = i.Children.Select(_ => _.Arguments.Select(float.Parse).ToArray()).ToList();

                        break;
                    case "vertexattr":
                        var children = i.Children.ToDictionary(_ => _.Name);

                        if (children.ContainsKey("weit"))
                            foreach (var j in children["weit"].Children)
                                rt.Weight.Add(int.Parse(j.Arguments.First()), float.Parse(j.Arguments[1]));

                        break;
                    case "face":
                        rt.Faces = i.Children.Select(MqFace.Parse).ToList();

                        break;
                }

            return rt;
        }

        public MqChunk ToChunk()
        {
            baseChunk = baseChunk ?? new MqChunk();
            baseChunk.Name = "Object";
            baseChunk.SetArguments("\"" + this.Name + "\"");
            baseChunk.Child("depth").SetArguments(this.Depth.ToString());
            baseChunk.Child("folding").SetArguments(this.Folding ? "1" : "0");
            baseChunk.Child("scale").SetArguments(this.Scale.Select(_ => _.ToString("0.000000")));
            baseChunk.Child("rotation").SetArguments(this.Scale.Select(_ => _.ToString("0.000000")));
            baseChunk.Child("translation").SetArguments(this.Scale.Select(_ => _.ToString("0.000000")));

            if (this.Patch == MqPatchType.None)
            {
                baseChunk.RemoveChildren("patch");
                baseChunk.RemoveChildren("segment");
            }
            else
            {
                baseChunk.Child("patch").SetArguments(((int)this.Patch).ToString());
                baseChunk.Child("segment").SetArguments(this.Segment.ToString());
            }

            baseChunk.Child("visible").SetArguments(this.Visible ? "15" : "0");
            baseChunk.Child("locking").SetArguments(this.Locking ? "1" : "0");
            baseChunk.Child("shading").SetArguments(((int)this.Shading).ToString());
            baseChunk.Child("facet").SetArguments(this.Facet.ToString("0.0"));
            baseChunk.Child("color").SetArguments(this.Color.Select(_ => _.ToString("0.000")));
            baseChunk.Child("color_type").SetArguments(this.ColorType ? "1" : "0");

            if (this.Mirror == MqMirrorType.None)
                baseChunk.RemoveChildren("mirror");
            else
            {
                baseChunk.Child("mirror").SetArguments(((int)this.Mirror).ToString());
                baseChunk.Child("mirror_axis").SetArguments(((int)this.MirrorAxis).ToString());
                baseChunk.Child("mirror_dis").SetArguments(this.MirrorDistance.ToString("0.000"));
            }

            if (this.Lathe == MqLatheType.None)
                baseChunk.RemoveChildren("lathe");
            else
            {
                baseChunk.Child("lathe").SetArguments(((int)this.Lathe).ToString());
                baseChunk.Child("lathe_axis").SetArguments(((int)this.LatheAxis).ToString());
                baseChunk.Child("lathe_seg").SetArguments(this.LatheSegments.ToString());
            }

            baseChunk.Child("vertex").SetArguments(this.Vertex.Count.ToString()).SetChildren(
                this.Vertex.Select(v => new MqChunk().SetArguments(v.Select(_ => _.ToString("0.0000")))));

            if (this.Weight.Any())
                baseChunk.Child("vertexattr").Child("weit").SetChildren(this.Weight.Select(_ =>
                    new MqChunk().SetArguments(_.Key.ToString(), _.Value.ToString("0.000"))));
            else
            {
                var attr = baseChunk.Child("vertexattr");

                attr.RemoveChildren("weit");

                if (!attr.Children.Any())
                    baseChunk.RemoveChildren("vertexattr");
            }

            baseChunk.Child("face").SetArguments(this.Faces.Count.ToString())
                .SetChildren(this.Faces.Select(_ => _.ToChunk()));

            return baseChunk;
        }

        public string GetFormattedText()
        {
            return this.ToChunk().GetFormattedText();
        }
    }
}