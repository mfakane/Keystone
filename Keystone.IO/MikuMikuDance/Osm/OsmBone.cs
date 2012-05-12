using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public class OsmBone
	{
		public string Name
		{
			get;
			set;
		}

		public int ParentBone
		{
			get;
			set;
		}

		public int ConnectToBone
		{
			get;
			set;
		}

		public OsmBoneKind Kind
		{
			get;
			set;
		}

		public int AffectedBone
		{
			get;
			set;
		}

		public float[] Position
		{
			get;
			set;
		}

		public OsmBone()
		{
			this.Position = new[] { 0f, 0, 0 };
		}

		public static OsmBone Parse(IEnumerable<string> block)
		{
			var sl = block.Select(_ => _.Trim().Split(new[] { ';' }, 2).First()).ToArray();
			var tkk = sl[2].Split(',');

			return new OsmBone
			{
				Name = sl[0].Split(new[] { '{' }, 2).Last(),
				ParentBone = int.Parse(sl[1]),
				ConnectToBone = int.Parse(tkk[0]),
				Kind = (OsmBoneKind)int.Parse(tkk[1]),
				AffectedBone = int.Parse(tkk[2]),
				Position = sl[3].Split(',').Select(float.Parse).ToArray(),
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
			sl.Append(string.Join(",", this.Position.Select(_ => _.ToString("0.000000"))));
			sl.AppendLine(";\t\t// Position");

			sl.Append(" }");

			return sl.ToString();
		}
	}
}
