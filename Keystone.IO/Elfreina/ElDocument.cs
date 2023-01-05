using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Linearstar.Keystone.IO.Elfreina
{
    /// <summary>
    /// エルフレイナ拡張モデルファイル created by オノデラ
    /// </summary>
    public class ElDocument
    {
        public const string DisplayName = "Elfreina Extended Model File";
        public const string Filter = "*.elem";
        public const string NewLine = "\r\n";
        public static readonly Encoding Encoding = Encoding.UTF8;

        public float Version { get; set; }

        public ElSetting Setting { get; set; }

        public IList<ElMeshContainer> MeshDataList { get; set; } = new List<ElMeshContainer>();

        public IList<ElNode> HierarchyList { get; set; } = new List<ElNode>();

        public IDictionary<string, IList<string>> RenderingMeshList { get; set; } = new Dictionary<string, IList<string>>();

        public IList<ElAnimationData> AnimationList { get; set; } = new List<ElAnimationData>();

        public IList<ElData> CustomData { get; set; } = new List<ElData>();

        public static ElDocument Parse(string text)
        {
            var rt = new ElDocument();
            var tokenizer = new ElTokenizer(text);
            var header = new[] { "Elfreina", "Extension", "Model", "File", "\n", "File", "Version" };

            if (!tokenizer.Take(header.Length).Select(_ => _.Text).SequenceEqual(header))
                throw new InvalidOperationException("invalid format");

            rt.Version = float.Parse(tokenizer.MoveNext().EnsureKind(ElTokenizer.ValueTokenKind).Text);

            if (rt.Version >= 2)
                throw new NotSupportedException("specified format version not supported");

            foreach (var i in tokenizer)
                if (i.Kind == ElTokenizer.IdentifierTokenKind)
                {
                    var data = ElData.Parse(tokenizer);

                    switch (data.Name)
                    {
                        case "Setting":
                            rt.Setting = ElSetting.Parse(data);

                            break;
                        case "MeshDataList":
                            rt.MeshDataList = data.Children.Where(_ => _.Name == "MeshContainer")
                                .Select(ElMeshContainer.Parse).ToList();

                            break;
                        case "HierarchyList":
                            rt.HierarchyList = data.Children.Select(ElNode.Parse).ToList();

                            break;
                        case "RenderingMeshList":
                            foreach (var j in data.Children.Where(_ => _.Name == "RenderingMesh"))
                                rt.RenderingMeshList.Add(j.Child("RenderingMeshName").Values.First().Trim('"'),
                                    j.Child("HierarchyNames").Children.Select(_ => _.Values.First().Trim('"'))
                                        .ToList());

                            break;
                        case "AnimationList":
                            rt.AnimationList = data.Children.Where(_ => _.Name == "AnimationData")
                                .Select(ElAnimationData.Parse).ToList();

                            break;
                        default:
                            rt.CustomData.Add(data);

                            break;
                    }
                }

            return rt;
        }

        public string GetFormattedText()
        {
            var sb = new StringBuilder();

            sb.AppendLine("Elfreina Extension Model File");
            sb.Append("File Version ");
            sb.AppendLine(this.Version.ToString("0.00"));

            if (this.Setting != null)
                sb.AppendLine(this.Setting.GetFormattedText());

            if (this.MeshDataList.Any())
                sb.AppendLine(new ElData("MeshDataList")
                {
                    Children = this.MeshDataList.Select(_ => _.ToData())
                        .StartWith(new ElData("MeshContainerCount").SetValues(this.MeshDataList.Count.ToString()))
                        .ToList(),
                }.GetFormattedText());

            if (this.HierarchyList.Any())
                sb.AppendLine(new ElData("HierarchyList")
                {
                    Children = this.MeshDataList.Select(_ => _.ToData()).ToList(),
                }.GetFormattedText());

            if (this.RenderingMeshList.Any())
                sb.AppendLine(new ElData("RenderingMeshList")
                {
                    Children = this.RenderingMeshList.Select(_ =>
                        {
                            var rt = new ElData("RenderingMesh");

                            rt.Child("RenderingMeshName").SetValues("\"" + _.Key + "\"");
                            rt.Child("HierarchyNamesCount").SetValues(_.Value.Count.ToString());
                            rt.Child("HierarchyNames").Children =
                                _.Value.Select(s => new ElData().SetValues("\"" + s + "\"")).ToList();

                            return rt;
                        })
                        .StartWith(new ElData("RenderingMeshCount").SetValues(this.RenderingMeshList.Count.ToString()))
                        .ToList(),
                }.GetFormattedText());

            if (this.AnimationList.Any())
                sb.AppendLine(new ElData("AnimationList")
                {
                    Children = this.AnimationList.Select(_ => _.ToData())
                        .StartWith(new ElData("AnimationCount").SetValues(this.AnimationList.Count.ToString()))
                        .ToList(),
                }.GetFormattedText());

            foreach (var i in this.CustomData)
                sb.AppendLine(i.GetFormattedText());

            return sb.ToString();
        }
    }
}