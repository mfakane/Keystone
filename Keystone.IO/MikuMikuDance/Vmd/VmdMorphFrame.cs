using System.IO;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public class VmdMorphFrame
	{
		public string Name
		{
			get;
			set;
		}

		public uint FrameTime
		{
			get;
			set;
		}

		public float Weight
		{
			get;
			set;
		}

		public static VmdMorphFrame Parse(BinaryReader br)
		{
			return new VmdMorphFrame
			{
				Name = VmdDocument.ReadVmdString(br, 15),
				FrameTime = br.ReadUInt32(),
				Weight = br.ReadSingle(),
			};
		}

		public void Write(BinaryWriter bw)
		{
			VmdDocument.WriteVmdString(bw, this.Name, 15);
			bw.Write(this.FrameTime);
			bw.Write(this.Weight);
		}
	}
}
