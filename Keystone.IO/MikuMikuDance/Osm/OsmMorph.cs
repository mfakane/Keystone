using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public class OsmMorph
	{
		public string Name
		{
			get;
			set;
		}

		public OsmMorphKind Kind
		{
			get;
			set;
		}

		public Dictionary<ushort, float[]> Offsets
		{
			get;
			private set;
		}

		public OsmMorph()
		{
			this.Offsets = new Dictionary<ushort, float[]>();
		}

		public static OsmMorph Parse(IEnumerable<string> block)
		{
			var sl = block.Select(_ => _.Trim().Split(new[] { ';' }, 2).First()).ToArray();
			var cc = sl[1].Split(',');

			return new OsmMorph
			{
				Name = sl[0].Split(new[] { '{' }, 2).Last(),
				Kind = (OsmMorphKind)int.Parse(cc[1]),
				Offsets = sl.Skip(2).Select(_ => _.Split(':', ',')).ToDictionary(_ => ushort.Parse(_[0]), _ => new[]
				{
					float.Parse(_[1]),
					float.Parse(_[2]),
					float.Parse(_[3]),
				}),
			};
		}

		public string GetFormattedText(int index)
		{
			var sl = new StringBuilder();

			sl.Append(" Skin");
			sl.Append(index);
			sl.Append("{");
			sl.AppendLine(this.Name);

			sl.Append("  ");
			sl.Append(this.Offsets.Count);
			sl.Append(",");
			sl.Append((int)this.Kind);
			sl.AppendLine(";");

			this.Offsets.ForEach(_ => sl.AppendFormat("  {0}:{1:0.000000},{2:0.000000},{3:0.000000};\r\n", _.Key, _.Value[0], _.Value[1], _.Value[2]));

			sl.Append(" }");

			return sl.ToString();
		}
	}
}
