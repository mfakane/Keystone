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

		public void Parse(BinaryReader br, PmxDocument doc)
		{
			this.Name = doc.ReadString(br);
			this.EnglishName = doc.ReadString(br);
			this.Category = (PmxMorphCategory)br.ReadByte();
			this.Kind = (PmxMorphKind)br.ReadByte();
			this.Offsets = Enumerable.Range(0, br.ReadInt32()).Select(_ => PmxMorphOffset.Parse(br, doc, this.Kind)).ToList();
		}

		public void Write(BinaryWriter bw, PmxDocument doc, PmxIndexCache cache)
		{
			if (doc.Version < 2.1f)
				if (this.Kind == PmxMorphKind.Flip)
					this.Kind = PmxMorphKind.Group;
				else if (this.Kind == PmxMorphKind.Impulse)
					return;

			doc.WriteString(bw, this.Name);
			doc.WriteString(bw, this.EnglishName);
			bw.Write((byte)this.Category);
			bw.Write((byte)this.Kind);
			bw.Write(this.Offsets.Count);
			this.Offsets.ForEach(_ => _.Write(bw, doc, cache));
		}
	}
}
