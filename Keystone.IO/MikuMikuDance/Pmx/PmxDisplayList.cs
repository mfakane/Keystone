using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public class PmxDisplayList
	{
		public string Name
		{
			get;
			set;
		}

		public string EnglishName
		{
			get;
			set;
		}

		public bool IsSpecial
		{
			get;
			set;
		}

		public List<PmxDisplayItem> Items
		{
			get;
			set;
		}

		public PmxDisplayList()
		{
			this.Items = new List<PmxDisplayItem>();
		}

		public static PmxDisplayList Parse(BinaryReader br, PmxDocument doc)
		{
			return new PmxDisplayList
			{
				Name = doc.ReadString(br),
				EnglishName = doc.ReadString(br),
				IsSpecial = br.ReadBoolean(),
				Items = Enumerable.Range(0, br.ReadInt32()).Select(_ => PmxDisplayItem.Parse(br, doc)).ToList(),
			};
		}

		public void Write(BinaryWriter bw, PmxDocument doc, PmxIndexCache cache)
		{
			doc.WriteString(bw, this.Name);
			doc.WriteString(bw, this.EnglishName);
			bw.Write(this.IsSpecial);
			bw.Write(this.Items.Count);
			this.Items.ForEach(_ => _.Write(bw, doc, cache));
		}
	}
}
