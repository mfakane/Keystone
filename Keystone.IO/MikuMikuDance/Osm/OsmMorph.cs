using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Linearstar.Keystone.IO.MikuMikuDance.Osm
{
	public class OsmMorph
	{
		public string Name { get; set; } = "モーフ";

		public OsmMorphKind Kind { get; set; }

		/// <summary>
		/// 移動させる頂点のインデックス一覧を取得します。
		/// これは Kind == None の場合、モデルの頂点インデックスです。
		/// それ以外の場合、Kind == None の Indices におけるインデックスです。
		/// </summary>
		public IList<ushort> Indices { get; set; } = new List<ushort>();

		public IList<float[]> Offsets { get; set; } = new List<float[]>();

		public static OsmMorph Parse(IEnumerable<string> block)
		{
			var sl = block.Select(_ => _.Trim().Split(new[] { ';' }, 2).First()).ToArray();
			var cc = sl[1].Split(',');
			var ol = sl.Skip(2).Select(_ => _.Split(':', ',')).Select(_ => new
			{
				Index = ushort.Parse(_[0]),
				Offset = new[]
				{
					float.Parse(_[1]),
					float.Parse(_[2]),
					float.Parse(_[3]),
				}
			}).ToArray();

			return new OsmMorph
			{
				Name = sl[0].Split(new[] { '{' }, 2).Last(),
				Kind = (OsmMorphKind)int.Parse(cc[1]),
				Indices = ol.Select(_ => _.Index).ToList(),
				Offsets = ol.Select(_ => _.Offset).ToList(),
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

			this.Indices.Zip(this.Offsets, Tuple.Create)
						.ForEach(_ => sl.AppendFormat("  {0}:{1:0.000000},{2:0.000000},{3:0.000000};\r\n", _.Item1, _.Item2[0], _.Item2[1], _.Item2[2]));

			sl.Append(" }");

			return sl.ToString();
		}
	}
}
