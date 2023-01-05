using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Linearstar.Keystone.IO.MikuMikuDance.Osm
{
	public class OsmBone
	{
		public string Name { get; set; } = "名前";

		public int ParentBone { get; set; }

		public int ConnectToBone { get; set; }

		public OsmBoneKind Kind { get; set; }
		
		public int AffectedBone { get; set; }
		
		public Vector3 Position { get; set; }

		public static OsmBone Parse(IEnumerable<string> block)
		{
			var sl = block.Select(_ => _.Trim().Split(new[] { ';' }, 2).First()).ToArray();
			var tkk = sl[2].Split(',');
			var fl = sl[3].Split(',').Select(float.Parse).ToArray();

			return new OsmBone
			{
				Name = sl[0].Split(new[] { '{' }, 2).Last(),
				ParentBone = int.Parse(sl[1]),
				ConnectToBone = int.Parse(tkk[0]),
				Kind = (OsmBoneKind)int.Parse(tkk[1]),
				AffectedBone = int.Parse(tkk[2]),
				Position = new Vector3(fl[0], fl[1], fl[2]),
			};
		}

		public string GetFormattedText(int index)
		{
			var sl = new StringBuilder();

			sl.Append(" Bone ");
			sl.Append(index);
			sl.Append("{");
			sl.AppendLine(this.Name);

			sl.Append("  ");
			sl.Append(this.ParentBone);
			sl.AppendLine(";\t\t\t\t\t// Parent No.");

			sl.Append("  ");
			sl.Append(this.ConnectToBone);
			sl.Append(",");
			sl.Append((int)this.Kind);
			sl.Append(",");
			sl.Append((int)this.AffectedBone);
			sl.AppendLine(";\t\t\t\t\t\t// To,kind,knum");

			sl.Append("  ");
			sl.Append($"{this.Position.X:0.000000},{this.Position.Y:0.000000},{this.Position.Z:0.000000}");
			sl.AppendLine(";\t\t// Position");

			sl.Append(" }");

			return sl.ToString();
		}
	}
}
