using System.Collections.Generic;
using System.IO;
using Linearstar.Keystone.IO;

namespace Linearstar.Keystone.IO.MikuMikuMoving
{
	public class MvdCameraData : MvdFixedItemSection
	{
		public IList<MvdCameraFrame> Frames
		{
			get;
			set;
		}

		public int StageCount
		{
			get;
			set;
		}

		public MvdCameraData()
			: base(MvdTag.Camera)
		{
			this.Frames = new List<MvdCameraFrame>();
		}

		protected override void ReadItem(MvdDocument document, BinaryReader br)
		{
			this.Frames.Add(MvdCameraFrame.Parse(br));
		}

		protected override void ReadExtensionRegion(MvdDocument document, BinaryReader br)
		{
			if (br.GetRemainingLength() >= 4)
				this.StageCount = br.ReadInt32();
		}

		public override void Write(MvdDocument document, BinaryWriter bw)
		{
			this.MinorType = 0;
			this.RawCount = this.Frames.Count;

			base.Write(document, bw);
		}

		protected override void WriteExtensionRegion(MvdDocument document, BinaryWriter bw)
		{
			bw.Write(this.StageCount);
		}

		protected override void WriteItem(MvdDocument document, BinaryWriter bw, int index)
		{
			this.Frames[index].Write(bw);
		}
	}
}
