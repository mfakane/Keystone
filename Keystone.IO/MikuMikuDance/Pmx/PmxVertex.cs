using System.IO;
using System.Linq;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public class PmxVertex
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

		/// <summary>
		/// Vector4[]
		/// </summary>
		public float[][] AdditionalUV
		{
			get;
			set;
		}

		public PmxSkinningKind SkinningKind
		{
			get;
			set;
		}

		public PmxSkinningFunction SkinningFunction
		{
			get;
			set;
		}

		public float EdgeSize
		{
			get;
			set;
		}

		public PmxVertex()
		{
			this.Position = new[] { 0f, 0, 0 };
			this.Normal = new[] { 0f, 0, 0 };
			this.UV = new[] { 0f, 0 };
			this.AdditionalUV = new float[0][];
			this.SkinningFunction = new PmxLinearBlendDeforming2();
		}

		public static PmxVertex Parse(BinaryReader br, PmxDocument doc)
		{
			var rt = new PmxVertex
			{
				Position = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				Normal = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				UV = new[] { br.ReadSingle(), br.ReadSingle() },
				AdditionalUV = Enumerable.Range(0, doc.Header.AdditionalUVCount).Select(_ => new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle() }).ToArray(),
				SkinningKind = (PmxSkinningKind)br.ReadByte(),
			};

			rt.SkinningFunction = PmxSkinningFunction.Parse(br, doc, rt.SkinningKind);
			rt.EdgeSize = br.ReadSingle();

			return rt;
		}

		public void Write(BinaryWriter bw, PmxDocument doc)
		{
			if (doc.Version < 2.1f &&
				this.SkinningKind == PmxSkinningKind.DualQuaternionDeforming)
				this.SkinningKind = PmxSkinningKind.LinearBlendDeforming4;

			this.Position.ForEach(bw.Write);
			this.Normal.ForEach(bw.Write);
			this.UV.ForEach(bw.Write);
			this.AdditionalUV.ForEach(_ => _.ForEach(bw.Write));
			bw.Write((byte)this.SkinningKind);
			this.SkinningFunction.Write(bw, doc);
			bw.Write(this.EdgeSize);
		}
	}
}
