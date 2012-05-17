using System.Collections.Generic;
using System.IO;

namespace Linearstar.Keystone.IO.MikuMikuMoving
{
	public class MvdBoneData : MvdFixedItemSection
	{
		public IList<MvdBoneFrame> Frames
		{
			get;
			set;
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

		public int StageCount
		{
			get;
			set;
		}

		public MvdBoneData()
			: base(MvdTag.Bone)
		{
			this.Frames = new List<MvdBoneFrame>();
		}

		protected override void ReadExtensionRegion(MvdDocument document, BinaryReader br)
		{
			if (br.GetRemainingLength() >= 4)
				this.StageCount = br.ReadInt32();
		}

		protected override void ReadItem(MvdDocument document, BinaryReader br)
		{
			this.Frames.Add(MvdBoneFrame.Parse(br));
		}

		protected override void WriteExtensionRegion(MvdDocument document, BinaryWriter bw)
		{
			bw.Write(this.StageCount);
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
