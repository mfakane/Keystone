using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Linearstar.Keystone.IO.MikuMikuDance.Vpd
{
	public class VpdBone
	{
		public string BoneName { get; set; } = "ボーン";

		public Vector3 Position { get; set; }

		public Quaternion Quaternion { get; set; } = Quaternion.Identity;

		public static VpdBone Parse(IEnumerable<string> block)
		{
			var rt = new VpdBone();

			foreach (var i in block)
				if (i.StartsWith("Bone") && i.Contains("{"))
					rt.BoneName = i.Split(new[] { '{' }, 2).Last();
				else if (i == "}")
					continue;
				else
				{
					var fl = i.Split(new[] { ';' }, 2).First().Split(',').Select(float.Parse).ToArray();

					if (fl.Length == 4)
						rt.Quaternion = new(fl[0], fl[1], fl[2], fl[3]);
					else
						rt.Position = new(fl[0], fl[1], fl[2]);
				}

			return rt;
		}

		public string GetFormattedText(int index) =>
			$$"""
			Bone{{index}}{{{this.BoneName}}
			  {{this.Position.X:0.000000}},{{this.Position.Y:0.000000}},{{this.Position.Z:0.000000}};\t\t\t\t// trans x,y,z
			  {{this.Quaternion.X:0.000000}},{{this.Quaternion.Y:0.000000}},{{this.Quaternion.Z:0.000000}},{{this.Quaternion.W:0.000000}};\t\t// Quaternion x,y,z,w
			}
			""";
	}
}
