using System.IO;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public class XxMaterial
	{
		public float[] Diffuse
		{
			get;
			set;
		}

		public float Power
		{
			get;
			set;
		}

		public float[] Specular
		{
			get;
			set;
		}

		public float[] Ambient
		{
			get;
			set;
		}

		public uint LineCount
		{
			get;
			set;
		}

		public string Texture
		{
			get;
			set;
		}

		public XxMaterial()
		{
			this.Diffuse = new[] { 0f, 0, 0, 0 };
			this.Power = 5;
			this.Specular = new[] { 0f, 0, 0 };
			this.Ambient = new[] { 0f, 0, 0 };
		}

		public static XxMaterial Parse(BinaryReader br)
		{
			return new XxMaterial
			{
				Diffuse = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				Power = br.ReadSingle(),
				Specular = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				Ambient = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				LineCount = br.ReadUInt32(),
				Texture = XxDocument.ReadXxString(br, 256),
			};
		}

		public void Write(BinaryWriter bw)
		{
			this.Diffuse.ForEach(bw.Write);
			bw.Write(this.Power);
			this.Specular.ForEach(bw.Write);
			this.Ambient.ForEach(bw.Write);
			bw.Write(this.LineCount);
			XxDocument.WriteXxString(bw, this.Texture, 256, string.IsNullOrEmpty(this.Texture) ? (byte)0 : (byte)0xFD);
		}
	}
}
