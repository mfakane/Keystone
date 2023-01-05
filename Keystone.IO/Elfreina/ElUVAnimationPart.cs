using System.Collections.Generic;
using System.Linq;

namespace Linearstar.Keystone.IO.Elfreina
{
    public class ElUVAnimationPart
    {
        ElData baseData;

        public string MeshContainerName { get; set; }

        public string MaterialName { get; set; }

        public IList<float> TimeKeys { get; set; } = new List<float>();

        public IList<float[]> TransKeys { get; set; } = new List<float[]>();

        public IList<float[]> RotateKeys { get; set; } = new List<float[]>();

        public IList<float[]> ScaleKeys { get; set; } = new List<float[]>();

        public static ElUVAnimationPart Parse(ElData data)
        {
            var rt = new ElUVAnimationPart
            {
                baseData = data,
            };

            foreach (var i in data.Children)
                switch (i.Name)
                {
                    case "MeshContainerName":
                        rt.MeshContainerName = i.Values.First().Trim('"');

                        break;
                    case "MaterialName":
                        rt.MaterialName = i.Values.First().Trim('"');

                        break;
                    case "TimeKeys":
                        rt.TimeKeys = i.Children.Select(_ => float.Parse(_.Values.First())).ToList();

                        break;
                    case "TransKeys":
                        rt.TransKeys = i.Children.Select(_ => _.Values.Select(float.Parse).ToArray()).ToList();

                        break;
                    case "RotateKeys":
                        rt.RotateKeys = i.Children.Select(_ => _.Values.Select(float.Parse).ToArray()).ToList();

                        break;
                    case "ScaleKeys":
                        rt.ScaleKeys = i.Children.Select(_ => _.Values.Select(float.Parse).ToArray()).ToList();

                        break;
                }

            return rt;
        }

        public ElData ToData()
        {
            baseData = baseData ?? new ElData();
            baseData.Name = "AnimationPart";
            baseData.Child("MeshContainerName").SetValues("\"" + this.MeshContainerName + "\"");
            baseData.Child("MaterialName").SetValues("\"" + this.MaterialName + "\"");
            baseData.Child("TimeKeys").Children =
                this.TimeKeys.Select(_ => new ElData().SetValues(_.ToString("0.000000"))).ToList();
            baseData.Child("TransKeys").Children = this.TransKeys
                .Select(_ => new ElData().SetValues(_.Select(f => f.ToString("0.000000")))).ToList();
            baseData.Child("RotateKeys").Children = this.RotateKeys
                .Select(_ => new ElData().SetValues(_.Select(f => f.ToString("0.000000")))).ToList();
            baseData.Child("ScaleKeys").Children = this.ScaleKeys
                .Select(_ => new ElData().SetValues(_.Select(f => f.ToString("0.000000")))).ToList();

            return baseData;
        }

        public string GetFormattedText()
        {
            return this.ToData().GetFormattedText();
        }
    }
}