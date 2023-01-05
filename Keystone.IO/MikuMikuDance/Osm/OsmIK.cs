using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Linearstar.Keystone.IO.MikuMikuDance.Osm
{
	public class OsmIK
	{
		public int IKBone { get; set; }

		public int TargetBone { get; set; }

		public int LoopCount { get; set; }

		public float AngleLimitUnit { get; set; }

		public IList<int> BindedBones { get; set; } = new List<int>();

		public static OsmIK Parse(IEnumerable<string> block)
		{
			var sl = block.Select(_ => _.Trim().Split(new[] { ';' }, 2).First()).ToArray();
			var def = sl[1].Split(',');

			return new OsmIK
			{
				IKBone = int.Parse(def[0]),
				TargetBone = int.Parse(def[1]),
				LoopCount = int.Parse(def[3]),
				AngleLimitUnit = float.Parse(def[4]),
				BindedBones = sl.Skip(2).Select(int.Parse).ToList(),
			};
		}

		public string GetFormattedText(int index)
		{
			var sl = new StringBuilder();

			sl.AppendFormat(" IK{0}{{\r\n", index);
			sl.AppendFormat("  {0},{1},{2},{3},{4};\t\t\t\t// IK,目標点,ボーン数,精度\r\n", this.IKBone, this.TargetBone,this.BindedBones.Count, this.LoopCount, this.AngleLimitUnit);
			this.BindedBones.Select(_ => "  " + _).ForEach(_ => sl.AppendLine(_));
			sl.Append(" }");

			return sl.ToString();
		}
	}
}
