using System.Collections.Generic;
using System.IO;

namespace Linearstar.Keystone.IO.MikuMikuMoving
{
	public class MvdMorphData : MvdFixedItemSection
	{
		public List<MvdMorphFrame> Frames
		{
			get;
			private set;
		}

		public int Key
		{
			get
			{
				return this.RawKey;
			}
			set
			{
				this.RawKey = value;
			}
		}

		public MvdMorphData()
			: base(MvdTag.Morph)
		{
			this.Frames = new List<MvdMorphFrame>();
		}

		protected override void ReadItem(MvdDocument document, BinaryReader br)
		{
			this.Frames.Add(MvdMorphFrame.Parse(br));
		}

		public override void Write(MvdDocument document, BinaryWriter bw)
		{
			this.MinorType = 0;
			this.RawCount = this.Frames.Count;

			base.Write(document, bw);
		}

		protected override void WriteItem(MvdDocument document, BinaryWriter bw, int index)
		{
			this.Frames[index].Write(bw);
		}
	}
}
