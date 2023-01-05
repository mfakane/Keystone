using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Linearstar.Keystone.IO.MikuMikuDance.Vpd
{
	/// <summary>
	/// Vocaloid Pose Data file created by Higuchi_U
	/// </summary>
	public class VpdDocument
	{
		public const string DisplayName = "Vocaloid Pose Data file";
		public const string Filter = "*.vpd";
		public static readonly Encoding Encoding = Encoding.GetEncoding(932);

		public string ParentFileName { get; set; } = "miku.osm";

		public IList<VpdBone> Bones { get; set; } = new List<VpdBone>();

		public IList<VpdMorph> Morphs { get; set; } = new List<VpdMorph>();

		public static VpdDocument Parse(string text)
		{
			var rt = new VpdDocument();

			using (var sr = new StringReader(text))
			{
				var header = sr.ReadLine();

				if (header != DisplayName)
					throw new InvalidOperationException("invalid format");

				sr.ReadLine();
				rt.ParentFileName = sr.ReadLine().Split(new[] { ';' }, 2).First();
				sr.ReadLine();

				for (var i = sr.ReadLine(); i != null; i = sr.ReadLine())
					if (i.Contains("{"))
						if (i.StartsWith("Bone"))
							rt.Bones.Add(VpdBone.Parse(new[] { i }
								.Concat(EnumerableEx.Repeat(sr)
											.Select(_ => _.ReadLine())
											.TakeWhile(_ => _.Trim() != "}"))));
						else if (i.StartsWith("Morph"))
							rt.Morphs.Add(VpdMorph.Parse(new[] { i }
								.Concat(EnumerableEx.Repeat(sr)
											.Select(_ => _.ReadLine())
											.TakeWhile(_ => _.Trim() != "}"))));
			}

			return rt;
		}

		public string GetFormattedText() =>
			$"""
			{DisplayName}

			{this.ParentFileName};\t\t// 親ファイル名
			{this.Bones.Count}\t\t\t\t// 総ポーズボーン数

			{string.Join("\r\n\r\n", this.Bones.Select((x, i) => x.GetFormattedText(i)))}
			
			{string.Join("\r\n\r\n", this.Morphs.Select((x, i) => x.GetFormattedText(i)))}
			""";
	}
}
