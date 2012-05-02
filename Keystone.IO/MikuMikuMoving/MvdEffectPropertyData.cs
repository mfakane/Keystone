using System.Collections.Generic;
using System.IO;
using System.Linq;
using Linearstar.Keystone.IO;

namespace Linearstar.Keystone.IO.MikuMikuMoving
{
	public class MvdEffectPropertyData : MvdFixedItemSection
	{
		public List<MvdEffectPropertyFrame> Frames
		{
			get;
			private set;
		}

		public List<MvdEffectParameter> Parameters
		{
			get;
			private set;
		}

		public MvdEffectPropertyData()
			: base(MvdTag.EffectProperty)
		{
			this.Frames = new List<MvdEffectPropertyFrame>();
			this.Parameters = new List<MvdEffectParameter>();
		}

		protected override void ReadExtensionRegion(MvdDocument document, BinaryReader br)
		{
			if (br.GetRemainingLength() >= 4)
				this.Parameters.AddRange(Enumerable.Range(0, br.ReadInt32()).Select(_ => MvdEffectParameter.Parse(br)));
		}

		protected override void ReadItem(MvdDocument document, BinaryReader br)
		{
			this.Frames.Add(MvdEffectPropertyFrame.Parse(this, br));
		}

		protected override void WriteExtensionRegion(MvdDocument document, BinaryWriter bw)
		{
			bw.Write(this.Parameters.Count);
			this.Parameters.ForEach(_ => _.Write(bw));
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
