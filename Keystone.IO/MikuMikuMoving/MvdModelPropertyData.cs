using System.Collections.Generic;
using System.IO;
using System.Linq;
using Linearstar.Keystone.IO;

namespace Linearstar.Keystone.IO.MikuMikuMoving
{
	public class MvdModelPropertyData : MvdFixedItemSection
	{
		public List<MvdModelPropertyFrame> Frames
		{
			get;
			private set;
		}

		public int[] IKBones
		{
			get;
			private set;
		}

		public MvdModelPropertyData()
			: base(MvdTag.ModelProperty)
		{
			this.Frames = new List<MvdModelPropertyFrame>();
			this.IKBones = new int[0];
		}

		protected override void Read(MvdDocument document, BinaryReader br)
		{
			// フォーマットバグ対策
			if (this.MinorType == 1)
				this.RawItemSize += 4;

			base.Read(document, br);
		}

		protected override void ReadExtensionRegion(MvdDocument document, BinaryReader br)
		{
			if (br.GetRemainingLength() >= 4)
				this.IKBones = Enumerable.Range(0, br.ReadInt32()).Select(_ => br.ReadInt32()).ToArray();
		}

		protected override void ReadItem(MvdDocument document, BinaryReader br)
		{
			this.Frames.Add(MvdModelPropertyFrame.Parse(this, br));
		}

		protected override void WriteExtensionRegion(MvdDocument document, BinaryWriter bw)
		{
			bw.Write(this.IKBones.Length);
			this.IKBones.ForEach(bw.Write);
		}

		public override void Write(MvdDocument document, BinaryWriter bw)
		{
			this.MinorType = 1;
			this.RawCount = this.Frames.Count;

			base.Write(document, bw);
		}

		protected override void BeforeWriteHeader(MvdDocument document, BinaryWriter bw)
		{
			// フォーマットバグ対策
			if (this.MinorType == 1)
				this.RawItemSize -= 4;
		}

		protected override void WriteItem(MvdDocument document, BinaryWriter bw, int index)
		{
			this.Frames[index].Write(this, bw);
		}
	}
}
