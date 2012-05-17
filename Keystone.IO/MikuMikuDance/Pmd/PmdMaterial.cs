using System.IO;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public class PmdMaterial
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

		public sbyte ToonIndex
		{
			get;
			set;
		}

		public bool NoEdge
		{
			get;
			set;
		}

		public int IndexCount
		{
			get;
			set;
		}

		public string Texture
		{
			get;
			set;
		}

		public PmdMaterial()
		{
			this.Diffuse = new[] { 0f, 0, 0, 0 };
			this.Power = 5;
			this.Specular = new[] { 0f, 0, 0 };
			this.Ambient = new[] { 0f, 0, 0 };
			this.ToonIndex = -1;
		}

		public static PmdMaterial Parse(BinaryReader br)
		{
			return new PmdMaterial
			{
				Diffuse = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				Power = br.ReadSingle(),
				Specular = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				Ambient = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				ToonIndex = br.ReadSByte(),
				NoEdge = br.ReadBoolean(),
				IndexCount = br.ReadInt32(),
				Texture = PmdDocument.ReadPmdString(br, 20),
			};
		}

		public void Write(BinaryWriter bw)
		{
			this.Diffuse.ForEach(bw.Write);
			bw.Write(this.Power);
			this.Specular.ForEach(bw.Write);
			this.Ambient.ForEach(bw.Write);
			bw.Write(this.ToonIndex);
			bw.Write(this.NoEdge);
			bw.Write(this.IndexCount);
			PmdDocument.WritePmdString(bw, this.Texture, 20);
		}
	}
}
