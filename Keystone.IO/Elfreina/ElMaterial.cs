using System.Linq;

namespace Linearstar.Keystone.IO.Elfreina
{
    public class ElMaterial
    {
        ElData baseData;

        public string Name { get; set; }

        public float[] Diffuse { get; set; } = { 0, 0, 0, 0 };

        public float[] Ambient { get; set; } = { 0, 0, 0, 0 };

        public float[] Emissive { get; set; } = { 0, 0, 0, 0 };

        public float[] Specular { get; set; } = { 0, 0, 0, 0 };

        public float SpecularSharpness { get; set; }

        public string TextureFilename { get; set; } = "";

        public static ElMaterial Parse(ElData data)
        {
            var rt = new ElMaterial
            {
                baseData = data,
            };

            foreach (var i in data.Children)
                switch (i.Name)
                {
                    case "Name":
                        rt.Name = i.Values.First().Trim('"');

                        break;
                    case "Diffuse":
                        rt.Diffuse = i.Values.Select(float.Parse).ToArray();

                        break;
                    case "Ambient":
                        rt.Ambient = i.Values.Select(float.Parse).ToArray();

                        break;
                    case "Emissive":
                        rt.Emissive = i.Values.Select(float.Parse).ToArray();

                        break;
                    case "Specular":
                        rt.Specular = i.Values.Select(float.Parse).ToArray();

                        break;
                    case "SpecularSharpness":
                        rt.SpecularSharpness = float.Parse(i.Values.First());

                        break;
                    case "TextureFilename":
                        rt.TextureFilename = i.Values.First().Trim('"');

                        break;
                }

            return rt;
        }

        public ElData ToData()
        {
            baseData = baseData ?? new ElData();
            baseData.Name = "Material";
            baseData.Child("Name").SetValues("\"" + this.Name + "\"");
            baseData.Child("Diffuse").SetValues(this.Diffuse.Select(_ => _.ToString("0.000000")));
            baseData.Child("Ambient").SetValues(this.Ambient.Select(_ => _.ToString("0.000000")));
            baseData.Child("Emissive").SetValues(this.Emissive.Select(_ => _.ToString("0.000000")));
            baseData.Child("Specular").SetValues(this.Emissive.Select(_ => _.ToString("0.000000")));
            baseData.Child("SpecularSharpness").SetValues(this.SpecularSharpness.ToString("0.000000"));
            baseData.Child("TextureFilename").SetValues("\"" + this.TextureFilename + "\"");

            return baseData;
        }

        public string GetFormattedText()
        {
            return this.ToData().GetFormattedText();
        }
    }
}