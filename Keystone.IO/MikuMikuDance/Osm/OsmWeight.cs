using System.Linq;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public class OsmWeight
	{
		public ushort BoneA
		{
			get;
			set;
		}

		public ushort BoneB
		{
			get;
			set;
		}

		public int Weight
		{
			get;
			set;
		}

		public static OsmWeight Parse(string line)
		{
			var sl = line.TrimStart().TrimEnd(';').Split(',').Select(int.Parse).ToArray();

			return new OsmWeight
			{
				BoneA = (ushort)sl[0],
				BoneB = (ushort)sl[1],
				Weight = sl[2],
			};
		}

		public string GetFormattedText()
		{
			return string.Format("  {0},{1},{2};", this.BoneA, this.BoneB, this.Weight);
		}
	}
}
