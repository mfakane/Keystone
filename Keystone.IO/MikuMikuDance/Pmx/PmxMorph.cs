using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public class PmxMorph
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

		public PmxMorphCategory Category
		{
			get;
			set;
		}

		public PmxMorphKind Kind
		{
			get;
			set;
		}

		public List<PmxMorphOffset> Offsets
		{
			get;
			set;
		}

		public PmxMorph()
		{
			this.Offsets = new List<PmxMorphOffset>();
		}

		public static PmxMorph Parse(BinaryReader br, PmxDocument doc)
		{
			var rt = new PmxMorph
			{
				Name = doc.ReadString(br),
				EnglishName = doc.ReadString(br),
				Category = (PmxMorphCategory)br.ReadByte(),
				Kind = (PmxMorphKind)br.ReadByte(),
			};

			rt.Offsets = Enumerable.Range(0, br.ReadInt32()).Select(_ => PmxMorphOffset.Parse(br, doc, rt.Kind)).ToList();

			return rt;
		}

		public void Write(BinaryWriter bw, PmxDocument doc)
		{
			doc.WriteString(bw, this.Name);
			doc.WriteString(bw, this.EnglishName);
			bw.Write((byte)this.Category);
			bw.Write((byte)this.Kind);
			bw.Write(this.Offsets.Count);
			this.Offsets.ForEach(_ => _.Write(bw, doc));
		}
	}
}
