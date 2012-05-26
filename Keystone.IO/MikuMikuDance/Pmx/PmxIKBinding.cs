using System.IO;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public class PmxIKBinding
	{
		public int Bone
		{
			get;
			set;
		}

		public bool IsAngleLimitEnabled
		{
			get;
			set;
		}

		public float[] LowerAngleLimit
		{
			get;
			set;
		}

		public float[] HigherAngleLimit
		{
			get;
			set;
		}

		public PmxIKBinding()
		{
			this.LowerAngleLimit = new[] { 0f, 0, 0 };
			this.HigherAngleLimit = new[] { 0f, 0, 0 };
		}

		public static PmxIKBinding Parse(BinaryReader br, PmxDocument doc)
		{
			var rt = new PmxIKBinding
			{
				Bone = doc.ReadIndex(br, PmxIndexKind.Bone),
				IsAngleLimitEnabled = br.ReadBoolean(),
			};

			if (rt.IsAngleLimitEnabled)
			{
				rt.LowerAngleLimit = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
				rt.HigherAngleLimit = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
			}

			return rt;
		}

		public void Write(BinaryWriter bw, PmxDocument doc)
		{
			doc.WriteIndex(bw, PmxIndexKind.Bone, this.Bone);
			bw.Write(this.IsAngleLimitEnabled);

			if (this.IsAngleLimitEnabled)
			{
				this.LowerAngleLimit.ForEach(bw.Write);
				this.HigherAngleLimit.ForEach(bw.Write);
			}
		}
	}
}
