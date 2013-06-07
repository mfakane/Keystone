using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public class VmdPropertyFrame
	{
		public uint FrameTime
		{
			get;
			set;
		}

		public bool IsVisible
		{
			get;
			set;
		}

		public IDictionary<string, bool> IKEnabled
		{
			get;
			set;
		}

		public VmdPropertyFrame()
		{
			this.IKEnabled = new Dictionary<string, bool>();
		}

		public static VmdPropertyFrame Parse(BinaryReader br)
		{
			return new VmdPropertyFrame
			{
				FrameTime = br.ReadUInt32(),
				IsVisible = br.ReadByte() != 0,
				IKEnabled = Enumerable.Range(0, br.ReadInt32()).Select(_ => Tuple.Create(VmdDocument.ReadVmdString(br, 20), br.ReadByte())).ToDictionary(_ => _.Item1, _ => _.Item2 != 0),
			};
		}

		public void Write(BinaryWriter bw)
		{
			bw.Write(this.FrameTime);
			bw.Write(this.IsVisible ? (byte)1 : (byte)0);
			bw.Write(this.IKEnabled.Count);
			this.IKEnabled.ForEach(_ =>
			{
				VmdDocument.WriteVmdString(bw, _.Key, 20, VmdVersion.MMDVer2);
				bw.Write(_.Value ? (byte)1 : (byte)0);
			});
		}
	}
}
