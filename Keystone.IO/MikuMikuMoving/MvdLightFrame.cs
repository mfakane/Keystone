using System.IO;
using System.Linq;

namespace Linearstar.Keystone.IO.MikuMikuMoving
{
	public class MvdLightFrame
	{
		public long FrameTime
		{
			get;
			set;
		}

		public float[] Position
		{
			get;
			set;
		}

		public float[] Color
		{
			get;
			set;
		}

		public bool Enabled
		{
			get;
			set;
		}

		public MvdLightFrame()
		{
			this.Position = new[] { -0.5f, -1, 0.5f };
			this.Color = new[] { 0.6f, 0.6f, 0.6f };
			this.Enabled = true;
		}

		public static MvdLightFrame Parse(BinaryReader br)
		{
			return new MvdLightFrame
			{
				FrameTime = br.ReadInt64(),
				Position = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				Color = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				Enabled = br.ReadBoolean(),
			};
		}

		public void Write(BinaryWriter bw)
		{
			bw.Write(this.FrameTime);
			this.Position.ForEach(bw.Write);
			this.Color.ForEach(bw.Write);
			bw.Write(this.Enabled);
		}
	}
}
