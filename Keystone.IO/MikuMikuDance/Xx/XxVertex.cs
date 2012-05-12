using System.IO;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public class XxVertex
	{
		public float[] Position
		{
			get;
			set;
		}

		public float[] Normal
		{
			get;
			set;
		}

		public float[] UV
		{
			get;
			set;
		}

		public XxVertex()
		{
			this.Position = new[] { 0f, 0, 0 };
			this.Normal = new[] { 0f, 0, 0 };
			this.UV = new[] { 0f, 0 };
		}

		public static XxVertex Parse(BinaryReader br)
		{
			return new XxVertex
			{
				Position = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				Normal = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				UV = new[] { br.ReadSingle(), br.ReadSingle() },
			};
		}

		public void Write(BinaryWriter bw)
		{
			this.Position.ForEach(bw.Write);
			this.Normal.ForEach(bw.Write);
			this.UV.ForEach(bw.Write);
		}
	}
}
