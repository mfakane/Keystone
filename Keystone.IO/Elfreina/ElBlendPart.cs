using System.Collections.Generic;
using System.Linq;

namespace Linearstar.Keystone.IO.Elfreina
{
    public class ElBlendPart
    {
        ElData baseData;

        public string BoneName { get; set; }

        public int TransformIndex { get; set; }

        public IDictionary<int, float> VertexBlend { get; set; }

        public ElBlendPart()
        {
            this.VertexBlend = new Dictionary<int, float>();
        }

        public static ElBlendPart Parse(ElData data)
        {
            var rt = new ElBlendPart
            {
                baseData = data,
            };

            foreach (var i in data.Children)
                switch (i.Name)
                {
                    case "BoneName":
                        rt.BoneName = i.Values.First().Trim('"');

                        break;
                    case "TransformIndex":
                        rt.TransformIndex = int.Parse(i.Values.First());

                        break;
                    case "VertexBlend":
                        foreach (var j in i.Children)
                        {
                            var sl = j.Values.First().Split(',');

                            rt.VertexBlend.Add(int.Parse(sl[0]), float.Parse(sl[1]));
                        }

                        break;
                }

            return rt;
        }

        public ElData ToData()
        {
            baseData = baseData ?? new ElData();
            baseData.Name = "BlendPart";
            baseData.Child("BoneName").SetValues("\"" + this.BoneName + "\"");
            baseData.Child("TransformIndex").SetValues(this.TransformIndex.ToString());
            baseData.Child("VertexBlend").Children = this.VertexBlend
                .Select(_ => new ElData().SetValues(_.Key.ToString() + "," + _.Value.ToString("0.000000"))).ToList();

            return baseData;
        }

        public string GetFormattedText()
        {
            return this.ToData().GetFormattedText();
        }
    }
}