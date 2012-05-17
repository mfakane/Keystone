using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public class PmdIK
	{
		public short IKBone
		{
			get;
			set;
		}

		public short TargetBone
		{
			get;
			set;
		}

		public ushort LoopCount
		{
			get;
			set;
		}

		public float AngleLimitUnit
		{
			get;
			set;
		}

		public List<short> BindedBones
		{
			get;
			private set;
		}

		public PmdIK()
		{
			this.BindedBones = new List<short>();
		}

		public static PmdIK Parse(BinaryReader br)
		{
			var rt = new PmdIK
			{
				IKBone = br.ReadInt16(),
				TargetBone = br.ReadInt16(),
			};
			var bindedBones = br.ReadByte();

			rt.LoopCount = br.ReadUInt16();
			rt.AngleLimitUnit = br.ReadSingle();
			rt.BindedBones.AddRange(Enumerable.Range(0, bindedBones).Select(_ => br.ReadInt16()));

			return rt;
		}

		public void Write(BinaryWriter bw)
		{
			bw.Write(this.IKBone);
			bw.Write(this.TargetBone);
			bw.Write((byte)this.BindedBones.Count);
			bw.Write(this.LoopCount);
			bw.Write(this.AngleLimitUnit);
			this.BindedBones.ForEach(bw.Write);
		}
	}
}
