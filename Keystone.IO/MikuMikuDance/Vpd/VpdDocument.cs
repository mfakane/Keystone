﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	/// <summary>
	/// Vocaloid Pose Data file created by Higuchi_U
	/// </summary>
	public class VpdDocument
	{
		public const string DisplayName = "Vocaloid Pose Data file";
		public const string Filter = "*.vpd";
		public static readonly Encoding Encoding = Encoding.GetEncoding(932);

		public string ParentFileName
		{
			get;
			set;
		}

		public List<VpdBone> Bones
		{
			get;
			private set;
		}

		public VpdDocument()
		{
			this.Bones = new List<VpdBone>();
		}

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
					if (i.StartsWith("Bone") && i.Contains("{"))
						rt.Bones.Add(VpdBone.Parse(new[] { i }
							.Concat(Util.Repeat(sr)
										.Select(_ => _.ReadLine())
										.TakeWhile(_ => _.Trim() != "}"))));
			}

			return rt;
		}

		public string GetFormattedText()
		{
			var sb = new StringBuilder();

			sb.AppendLine(DisplayName);
			sb.AppendLine();
			sb.AppendFormat("{0};\t\t// 親ファイル名\r\n", this.ParentFileName);
			sb.AppendFormat("{0};\t\t\t\t// 総ポーズボーン数\r\n", this.Bones.Count);
			sb.AppendLine();

			var idx = 0;

			foreach (var i in this.Bones)
			{
				sb.AppendLine(i.GetFormattedText(idx++));
				sb.AppendLine();
			}

			return sb.ToString();
		}
	}
}
