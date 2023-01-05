using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
    /// <summary>
    /// Vocaloid Accessory Connection file created by Higuchi_U
    /// </summary>
    public class VacDocument
    {
        public const string DisplayName = "Vocaloid Accessory Connection file";
        public const string Filter = "*.vac";
        public static readonly Encoding Encoding = Encoding.GetEncoding(932);

        public string Name { get; set; } = "";

        public string FileName { get; set; } = "";

        public float Scale { get; set; }

        public Vector3 Position { get; set; }

        public Vector3 Angle { get; set; }

        public string BoneName { get; set; } = "ボーン";

        public static VacDocument FromFile(string path) =>
            Parse(File.ReadAllText(path, Encoding));
        
        public static VacDocument Parse(string text)
        {
            using var sr = new StringReader(text);
            
            return new VacDocument
            {
                Name = sr.ReadLine() ?? throw new InvalidOperationException(),
                FileName = sr.ReadLine() ?? throw new InvalidOperationException(),
                Scale = float.Parse(sr.ReadLine() ?? throw new InvalidOperationException()),
                Position = ParseVector3(sr.ReadLine() ?? throw new InvalidOperationException()),
                Angle = ParseVector3(sr.ReadLine() ?? throw new InvalidOperationException()),
                BoneName = sr.ReadLine() ?? throw new InvalidOperationException(),
            };

            Vector3 ParseVector3(string str)
            {
                var sl = str.Split(',').Select(float.Parse).ToArray();

                return new(sl[0], sl[1], sl[2]);
            }
        }

        public string GetFormattedText() =>
            string.Join("\r\n",
                this.Name,
                this.FileName,
                this.Scale.ToString("0.000000"),
                $"{this.Position.X:0.000000},{this.Position.Y:0.000000},{this.Position.Z:0.000000}",
                $"{this.Angle.X:0.000000},{this.Angle.Y:0.000000},{this.Angle.Z:0.000000}",
                this.BoneName
            );
    }
}