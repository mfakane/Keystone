using System.Collections.Generic;

namespace Linearstar.Keystone.IO.MikuMikuDance.Pmd
{
	public class PmdIK
	{
		public PmdBone? IKBone { get; set; }

		public PmdBone? TargetBone { get; set; }

		public ushort LoopCount { get; set; }

		public float AngleLimitUnit { get; set; }

		public IList<PmdBone> BoundBones { get; set; } = new List<PmdBone>();

		internal static PmdIK Parse(ref BufferReader br, PmdDocument doc)
		{
			var rt = new PmdIK
			{
				IKBone = doc.GetBone(br.ReadInt16()),
				TargetBone = doc.GetBone(br.ReadInt16()),
			};
			var boundBones = br.ReadByte();

			rt.LoopCount = br.ReadUInt16();
			rt.AngleLimitUnit = br.ReadSingle();

			for (var i = 0; i < boundBones; i++)
				rt.BoundBones.Add(doc.GetBone(br.ReadInt16())!);

			return rt;
		}

		internal void Write(ref BufferWriter bw, PmdIndexCache cache)
		{
			bw.Write(this.IKBone, cache);
			bw.Write(this.TargetBone, cache);
			bw.Write((byte)this.BoundBones.Count);
			bw.Write(this.LoopCount);
			bw.Write(this.AngleLimitUnit);

			foreach (var boundBone in this.BoundBones)
				bw.Write(boundBone, cache);
		}
	}
}
