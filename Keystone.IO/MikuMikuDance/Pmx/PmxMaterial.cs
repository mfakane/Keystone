using System.IO;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public class PmxMaterial
	{
		public string Name
		{
			get;
			set;
		}

		public string EnglishName
		{
			get;
			set;
		}

		/// <summary>
		/// r, g, b, a
		/// </summary>
		public float[] Diffuse
		{
			get;
			set;
		}

		public float[] Specular
		{
			get;
			set;
		}

		public float Power
		{
			get;
			set;
		}

		public float[] Ambient
		{
			get;
			set;
		}

		public PmxMaterialOptions Options
		{
			get;
			set;
		}

		/// <summary>
		/// r, g, b, a
		/// </summary>
		public float[] EdgeColor
		{
			get;
			set;
		}

		/// <summary>
		/// this.Options.HasFlag(PmxMaterialOptions.DrawPoint) ? point size : edge size
		/// </summary>
		public float EdgeSize
		{
			get;
			set;
		}

		public PmxTexture MainTexture
		{
			get;
			set;
		}

		public PmxTexture SubTexture
		{
			get;
			set;
		}

		public PmxTextureMode SubTextureMode
		{
			get;
			set;
		}

		public bool UseSharedToonTexture
		{
			get;
			set;
		}

		public int SharedToonTexture
		{
			get;
			set;
		}

		public PmxTexture CustomToonTexture
		{
			get;
			set;
		}

		public string Comment
		{
			get;
			set;
		}

		public int IndexCount
		{
			get;
			set;
		}

		public PmxMaterial()
		{
			this.Diffuse = new[] { 0f, 0, 0, 0 };
			this.Specular = new[] { 0f, 0, 0 };
			this.Ambient = new[] { 0f, 0, 0 };
			this.EdgeColor = new[] { 0f, 0, 0, 0 };
		}

		public static PmxMaterial Parse(BinaryReader br, PmxDocument doc)
		{
			var rt = new PmxMaterial
			{
				Name = doc.Header.Encoding.GetString(br.ReadSizedBuffer()),
				EnglishName = doc.Header.Encoding.GetString(br.ReadSizedBuffer()),
				Diffuse = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				Specular = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				Power = br.ReadSingle(),
				Ambient = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				Options = (PmxMaterialOptions)br.ReadByte(),
				EdgeColor = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				EdgeSize = br.ReadSingle(),
				MainTexture = doc.ReadTexture(br),
				SubTexture = doc.ReadTexture(br),
				SubTextureMode = (PmxTextureMode)br.ReadByte(),
				UseSharedToonTexture = br.ReadBoolean(),
			};

			if (rt.UseSharedToonTexture)
				rt.SharedToonTexture = br.ReadByte();
			else
				rt.CustomToonTexture = doc.ReadTexture(br);

			rt.Comment = doc.Header.Encoding.GetString(br.ReadSizedBuffer());
			rt.IndexCount = br.ReadInt32();

			return rt;
		}

		public void Write(BinaryWriter bw, PmxDocument doc, PmxIndexCache cache)
		{
			bw.WriteSizedBuffer(doc.Header.Encoding.GetBytes(this.Name));
			bw.WriteSizedBuffer(doc.Header.Encoding.GetBytes(this.EnglishName));
			this.Diffuse.ForEach(bw.Write);
			this.Specular.ForEach(bw.Write);
			bw.Write(this.Power);
			this.Ambient.ForEach(bw.Write);
			bw.Write((byte)this.Options);
			this.EdgeColor.ForEach(bw.Write);
			bw.Write(this.EdgeSize);
			cache.Write(this.MainTexture);
			cache.Write(this.SubTexture);
			bw.Write((byte)this.SubTextureMode);
			bw.Write(this.UseSharedToonTexture);

			if (this.UseSharedToonTexture)
				bw.Write((byte)this.SharedToonTexture);
			else
				cache.Write(this.CustomToonTexture);

			bw.WriteSizedBuffer(doc.Header.Encoding.GetBytes(this.Comment));
			bw.Write(this.IndexCount);
		}
	}
}
