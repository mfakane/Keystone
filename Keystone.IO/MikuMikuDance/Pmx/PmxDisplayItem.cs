using System.IO;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public class PmxDisplayItem
	{
		public PmxDisplayItemKind Kind
		{
			get;
			set;
		}

		public int Index
		{
			get;
			set;
		}

		public static PmxDisplayItem Parse(BinaryReader br, PmxDocument doc)
		{
			var rt = new PmxDisplayItem
			{
				Kind = (PmxDisplayItemKind)br.ReadByte(),
			};

			rt.Index = rt.Kind == PmxDisplayItemKind.Bone ? doc.ReadIndex(br, PmxIndexKind.Bone) : doc.ReadIndex(br, PmxIndexKind.Morph);

			return rt;
		}

		public void Write(BinaryWriter bw, PmxDocument doc)
		{
			bw.Write((byte)this.Kind);
			doc.WriteIndex(bw, this.Kind == PmxDisplayItemKind.Bone ? PmxIndexKind.Bone : PmxIndexKind.Morph, this.Index);
		}
	}
}
