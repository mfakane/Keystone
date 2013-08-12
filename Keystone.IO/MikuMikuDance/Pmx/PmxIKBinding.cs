using System.IO;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public class PmxIKBinding
	{
		public PmxBone Bone
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
				Bone = doc.ReadBone(br),
				IsAngleLimitEnabled = br.ReadBoolean(),
			};

			if (rt.IsAngleLimitEnabled)
			{
				rt.LowerAngleLimit = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
				rt.HigherAngleLimit = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
			}

			return rt;
		}

		public void Write(BinaryWriter bw, PmxDocument doc, PmxIndexCache cache)
		{
			cache.Write(this.Bone);
			bw.Write(this.IsAngleLimitEnabled);

			if (this.IsAngleLimitEnabled)
			{
				this.LowerAngleLimit.ForEach(bw.Write);
				this.HigherAngleLimit.ForEach(bw.Write);
			}
		}
	}
}
