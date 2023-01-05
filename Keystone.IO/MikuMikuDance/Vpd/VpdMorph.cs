using System.Collections.Generic;
using System.Linq;

namespace Linearstar.Keystone.IO.MikuMikuDance.Vpd
{
    public class VpdMorph
    {
        public string MorphName { get; set; } = "モーフ";

        public float Weight { get; set; }

        public static VpdMorph Parse(IEnumerable<string> block)
        {
            var rt = new VpdMorph();

            foreach (var i in block)
                if (i.StartsWith("Morph") && i.Contains("{"))
                    rt.MorphName = i.Split(new[] { '{' }, 2).Last();
                else if (i == "}")
                    continue;
                else
                    rt.Weight = float.Parse(i.Split(';').First().Trim());

            return rt;
        }

        public string GetFormattedText(int index) => 
            $"Morph{index}{{{this.MorphName}\r\n  {this.Weight:0.000000};\t\t\t\t// weight\r\n}}";
    }
}