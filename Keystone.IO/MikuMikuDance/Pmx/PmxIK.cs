using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public class PmxIK
	{
		public int TargetBone
		{
			get;
			set;
		}

		public int LoopCount
		{
			get;
			set;
		}

		public float AngleLimitUnit
		{
			get;
			set;
		}

		public List<PmxIKBinding> BindedBones
		{
			get;
			set;
		}

		public PmxIK()
		{
			this.BindedBones = new List<PmxIKBinding>();
		}

		public static PmxIK Parse(BinaryReader br, PmxDocument doc)
		{
			return new PmxIK
			{
				TargetBone = doc.ReadIndex(br, PmxIndexKind.Bone),
				LoopCount = br.ReadInt32(),
				AngleLimitUnit = br.ReadSingle(),
				BindedBones = Enumerable.Range(0, br.ReadInt32()).Select(_ => PmxIKBinding.Parse(br, doc)).ToList(),
			};
		}

		public void Write(BinaryWriter bw, PmxDocument doc)
		{
			doc.WriteIndex(bw, PmxIndexKind.Bone, this.TargetBone);
			bw.Write(this.LoopCount);
			bw.Write(this.AngleLimitUnit);
			bw.Write(this.BindedBones.Count);
			this.BindedBones.ForEach(_ => _.Write(bw, doc));
		}
	}
}
