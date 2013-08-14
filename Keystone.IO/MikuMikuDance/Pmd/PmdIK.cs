using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public class PmdIK
	{
		public PmdBone IKBone
		{
			get;
			set;
		}

		public PmdBone TargetBone
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

		public IList<PmdBone> BindedBones
		{
			get;
			set;
		}

		public PmdIK()
		{
			this.BindedBones = new List<PmdBone>();
		}

		public static PmdIK Parse(BinaryReader br, PmdDocument doc)
		{
			var rt = new PmdIK
			{
				IKBone = doc.GetBone(br.ReadInt16()),
				TargetBone = doc.GetBone(br.ReadInt16()),
			};
			var bindedBones = br.ReadByte();

			rt.LoopCount = br.ReadUInt16();
			rt.AngleLimitUnit = br.ReadSingle();
			rt.BindedBones = Enumerable.Range(0, bindedBones).Select(_ => doc.GetBone(br.ReadInt16())).ToList();

			return rt;
		}

		public void Write(BinaryWriter bw, PmdIndexCache cache)
		{
			bw.Write((short)(this.IKBone == null ? -1 : cache.Bones[this.IKBone]));
			bw.Write((short)(this.TargetBone == null ? -1 : cache.Bones[this.TargetBone]));
			bw.Write((byte)this.BindedBones.Count);
			bw.Write(this.LoopCount);
			bw.Write(this.AngleLimitUnit);
			this.BindedBones.Select(_ => (short)(_ == null ? -1 : cache.Bones[_])).ForEach(bw.Write);
		}
	}
}
