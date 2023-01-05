namespace Linearstar.Keystone.IO.MikuMikuDance.Xx
{
	public class XxMaterial
	{
		public Color4 Diffuse { get; set; }

		public float Power { get; set; } = 5;

		public Color3 Specular { get; set; }

		public Color3 Ambient { get; set; }

		public uint LineCount { get; set; }

		public string Texture { get; set; } = "";
		
		internal static XxMaterial Parse(ref BufferReader br) =>
			new()
			{
				Diffuse = br.ReadColor4(),
				Power = br.ReadSingle(),
				Specular = br.ReadColor3(),
				Ambient = br.ReadColor3(),
				LineCount = br.ReadUInt32(),
				Texture = br.ReadString(),
			};

		internal void Write(ref BufferWriter bw)
		{
			bw.Write(this.Diffuse);
			bw.Write(this.Power);
			bw.Write(this.Specular);
			bw.Write(this.Ambient);
			bw.Write(this.LineCount);
			bw.Write(this.Texture);
		}
	}
}
