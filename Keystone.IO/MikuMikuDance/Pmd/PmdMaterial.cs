namespace Linearstar.Keystone.IO.MikuMikuDance.Pmd
{
	public class PmdMaterial
	{
		public Color4 Diffuse { get; set; }

		public float Power { get; set; } = 5;

		public Color3 Specular { get; set; }

		public Color3 Ambient { get; set; }

		public sbyte ToonIndex { get; set; } = -1;

		public bool NoEdge { get; set; }

		public int IndexCount { get; set; }

		public string Texture { get; set; } = "";

		internal static PmdMaterial Parse(ref BufferReader br) =>
			new()
			{
				Diffuse = br.ReadColor4(),
				Power = br.ReadSingle(),
				Specular = br.ReadColor3(),
				Ambient = br.ReadColor3(),
				ToonIndex = br.ReadSByte(),
				NoEdge = br.ReadBoolean(),
				IndexCount = br.ReadInt32(),
				Texture = br.ReadString(20),
			};

		internal void Write(ref BufferWriter bw)
		{
			bw.Write(this.Diffuse);
			bw.Write(this.Power);
			bw.Write(this.Specular);
			bw.Write(this.Ambient);
			bw.Write(this.ToonIndex);
			bw.Write(this.NoEdge);
			bw.Write(this.IndexCount);
			bw.Write(this.Texture, 20);
		}
	}
}
