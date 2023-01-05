using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Linearstar.Keystone.IO.MikuMikuDance.Osm
{
    /// <summary>
    /// One Skin Model file created by Higuchi_U
    /// </summary>
    public class OsmDocument
    {
        public const string DisplayName = "One Skin Model file";
        public const string Filter = "*.osm";
        public static readonly Encoding Encoding = Encoding.GetEncoding(932);

        public string ParentFileName { get; set; }

        public IList<OsmBone> Bones { get; set; }

        public IList<OsmIK> IK { get; set; }

        public IList<OsmWeight> Weights { get; set; }

        public IList<OsmMorph> Morphs { get; set; }

        public OsmDocument()
        {
            this.Bones = new List<OsmBone>();
            this.IK = new List<OsmIK>();
            this.Weights = new List<OsmWeight>();
            this.Morphs = new List<OsmMorph>();
        }

        public static OsmDocument Parse(string text)
        {
            var rt = new OsmDocument();

            using (var sr = new StringReader(text))
            {
                var header = sr.ReadLine();

                if (!header.StartsWith(DisplayName))
                    throw new InvalidOperationException("invalid format");

                sr.ReadLine();
                rt.ParentFileName = sr.ReadLine().Split(new[] { ';' }, 2).First();

                for (var i = sr.ReadLine(); i != null; i = sr.ReadLine())
                {
                    var key = i.Split(new[] { '{' }, 2).First().Trim();

                    switch (key)
                    {
                        case "Bone":
                            ParseBones(rt, sr);

                            break;
                        case "IK":
                            ParseIK(rt, sr);

                            break;
                        case "Mesh":
                            ParseWeights(rt, sr);

                            break;
                        case "Skin":
                            ParseMorphs(rt, sr);

                            break;
                    }
                }
            }

            return rt;
        }

        static IEnumerable<string> ReadBlock(string current, StringReader sr)
        {
            return new[] { current }.Concat(EnumerableEx.Repeat(sr)
                .Select(_ => _.ReadLine())
                .TakeWhile(_ => _.Trim() != "}"));
        }

        static void ParseBones(OsmDocument doc, StringReader sr)
        {
            sr.ReadLine();

            for (var i = sr.ReadLine(); i != null && i.TrimEnd() != "}"; i = sr.ReadLine())
                if (i.TrimStart().StartsWith("Bone") && i.Contains("{"))
                    doc.Bones.Add(OsmBone.Parse(ReadBlock(i, sr)));

            sr.ReadLine();
        }

        static void ParseIK(OsmDocument doc, StringReader sr)
        {
            sr.ReadLine();

            for (var i = sr.ReadLine(); i != null && i.TrimEnd() != "}"; i = sr.ReadLine())
                if (i.TrimStart().StartsWith("IK") && i.Contains("{"))
                    doc.IK.Add(OsmIK.Parse(ReadBlock(i, sr)));

            sr.ReadLine();
        }

        static void ParseWeights(OsmDocument doc, StringReader sr)
        {
            sr.ReadLine();

            for (var i = sr.ReadLine(); i != null && i.TrimEnd() != "}"; i = sr.ReadLine())
                if (!i.Contains("{"))
                    doc.Weights = ReadBlock(i, sr).Select(_ => OsmWeight.Parse(_)).ToList();

            sr.ReadLine();
        }

        static void ParseMorphs(OsmDocument doc, StringReader sr)
        {
            sr.ReadLine();

            for (var i = sr.ReadLine(); i != null && i.TrimEnd() != "}"; i = sr.ReadLine())
                if (i.TrimStart().StartsWith("Skin") && i.Contains("{"))
                    doc.Morphs.Add(OsmMorph.Parse(ReadBlock(i, sr)));

            sr.ReadLine();
        }

        public string GetFormattedText()
        {
            var sb = new StringBuilder();

            sb.AppendFormat("{0} for {1}\r\n", DisplayName, this.ParentFileName);
            sb.AppendLine();
            sb.AppendFormat("{0}\t\t\t// 親Xファイル名\r\n", this.ParentFileName);

            sb.AppendLine("Bone{");
            sb.AppendFormat(" {0};\t\t\t// 総bone数\r\n", this.Bones.Count);
            this.Bones.ForEach((_, idx) => sb.AppendLine(_.GetFormattedText(idx)));
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine("IK{");
            sb.AppendFormat(" {0};\t\t\t// 総IK数\r\n", this.IK.Count);
            this.IK.ForEach((_, idx) => sb.AppendLine(_.GetFormattedText(idx + 1)));
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine("Mesh{");
            sb.AppendFormat(" {0};\t\t\t// 総頂点数\r\n", this.Weights.Count);
            sb.AppendLine(" { // (Bone No.1,Bone No.2,weight)");
            this.Weights.ForEach(_ => sb.AppendLine(_.GetFormattedText()));
            sb.AppendLine(" }");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine("Skin{");
            sb.AppendFormat(" {0};\t\t\t// 総スキンデータ数\r\n", this.Morphs.Count);
            this.Morphs.ForEach((_, idx) => sb.AppendLine(_.GetFormattedText(idx)));
            sb.AppendLine("}");

            return sb.ToString();
        }
    }
}