using System;
using System.IO;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public abstract class PmxMorphOffset
	{
		public abstract void Read(BinaryReader br, PmxDocument doc);
		public abstract void Write(BinaryWriter bw, PmxDocument doc);

		public static PmxMorphOffset Parse(BinaryReader br, PmxDocument doc, PmxMorphKind kind)
		{
			PmxMorphOffset rt;

			switch (kind)
			{
				case PmxMorphKind.Group:
					rt = new PmxGroupMorphOffset();

					break;
				case PmxMorphKind.Vertex:
					rt = new PmxVertexMorphOffset();

					break;
				case PmxMorphKind.Bone:
					rt = new PmxBoneMorphOffset();

					break;
				case PmxMorphKind.UV:
					rt = new PmxUVMorphOffset();
				
					break;
				case PmxMorphKind.AdditionalUV1:
					rt = new PmxUVMorphOffset();
				
					break;
				case PmxMorphKind.AdditionalUV2:
					rt = new PmxUVMorphOffset();
				
					break;
				case PmxMorphKind.AdditionalUV3:
					rt = new PmxUVMorphOffset();
				
					break;
				case PmxMorphKind.AdditionalUV4:
					rt = new PmxUVMorphOffset();
					
					break;
				case PmxMorphKind.Material:
					rt = new PmxMaterialMorphOffset();
				
					break;
				default:
					throw new ArgumentException();
			}

			rt.Read(br, doc);

			return rt;
		}
	}

	public class PmxVertexMorphOffset : PmxMorphOffset
	{
		public int Vertex
		{
			get;
			set;
		}

		/// <summary>
		/// Vector3
		/// </summary>
		public float[] Offset
		{
			get;
			set;
		}

		public PmxVertexMorphOffset()
		{
			this.Offset = new[] { 0f, 0, 0 };
		}

		public override void Read(BinaryReader br, PmxDocument doc)
		{
			this.Vertex = doc.ReadIndex(br, PmxIndexKind.Vertex);
			this.Offset = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
		}

		public override void Write(BinaryWriter bw, PmxDocument doc)
		{
			doc.WriteIndex(bw, PmxIndexKind.Vertex, this.Vertex);
			this.Offset.ForEach(bw.Write);
		}
	}

	public class PmxUVMorphOffset : PmxMorphOffset
	{
		public int Vertex
		{
			get;
			set;
		}

		/// <summary>
		/// Vector4
		/// </summary>
		public float[] Offset
		{
			get;
			set;
		}

		public PmxUVMorphOffset()
		{
			this.Offset = new[] { 0f, 0, 0, 0 };
		}

		public override void Read(BinaryReader br, PmxDocument doc)
		{
			this.Vertex = doc.ReadIndex(br, PmxIndexKind.Vertex);
			this.Offset = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
		}

		public override void Write(BinaryWriter bw, PmxDocument doc)
		{
			doc.WriteIndex(bw, PmxIndexKind.Vertex, this.Vertex);
			this.Offset.ForEach(bw.Write);
		}
	}

	public class PmxBoneMorphOffset : PmxMorphOffset
	{
		public int Bone
		{
			get;
			set;
		}

		/// <summary>
		/// Vector3
		/// </summary>
		public float[] MovementOffset
		{
			get;
			set;
		}

		/// <summary>
		/// Vector4
		/// </summary>
		public float[] RotationOffset
		{
			get;
			set;
		}

		public PmxBoneMorphOffset()
		{
			this.MovementOffset = new[] { 0f, 0, 0 };
			this.RotationOffset = new[] { 0f, 0, 0, 0 };
		}

		public override void Read(BinaryReader br, PmxDocument doc)
		{
			this.Bone = doc.ReadIndex(br, PmxIndexKind.Bone);
			this.MovementOffset = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
			this.RotationOffset = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
		}

		public override void Write(BinaryWriter bw, PmxDocument doc)
		{
			doc.WriteIndex(bw, PmxIndexKind.Bone, this.Bone);
			this.MovementOffset.ForEach(bw.Write);
			this.RotationOffset.ForEach(bw.Write);
		}
	}

	public class PmxMaterialMorphOffset : PmxMorphOffset
	{
		public int Material
		{
			get;
			set;
		}

		public PmxMaterialMorphKind Kind
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

		/// <summary>
		/// r, g, b, a
		/// </summary>
		public float[] EdgeColor
		{
			get;
			set;
		}

		public float EdgeSize
		{
			get;
			set;
		}

		/// <summary>
		/// r, g, b, a
		/// </summary>
		public float[] Texture
		{
			get;
			set;
		}

		/// <summary>
		/// r, g, b, a
		/// </summary>
		public float[] SubTexture
		{
			get;
			set;
		}

		/// <summary>
		/// r, g, b, a
		/// </summary>
		public float[] ToonTexture
		{
			get;
			set;
		}

		public PmxMaterialMorphOffset()
		{
			this.Diffuse = new[] { 1f, 1, 1, 1 };
			this.Specular = new[] { 1f, 1, 1 };
			this.Ambient = new[] { 1f, 1, 1 };
			this.EdgeColor = new[] { 1f, 1, 1, 1 };
			this.EdgeSize = 1;
			this.Texture = new[] { 1f, 1, 1, 1 };
			this.SubTexture = new[] { 1f, 1, 1, 1 };
			this.ToonTexture = new[] { 1f, 1, 1, 1 };
		}

		public override void Read(BinaryReader br, PmxDocument doc)
		{
			this.Material = doc.ReadIndex(br, PmxIndexKind.Material);
			this.Kind = (PmxMaterialMorphKind)br.ReadByte();
			this.Diffuse = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
			this.Specular = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
			this.Power = br.ReadSingle();
			this.Ambient = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
			this.EdgeColor = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
			this.EdgeSize = br.ReadSingle();
			this.Texture = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
			this.SubTexture = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
			this.ToonTexture = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
		}

		public override void Write(BinaryWriter bw, PmxDocument doc)
		{
			doc.WriteIndex(bw, PmxIndexKind.Material, this.Material);
			bw.Write((byte)this.Kind);
			this.Diffuse.ForEach(bw.Write);
			this.Specular.ForEach(bw.Write);
			bw.Write(this.Power);
			this.Ambient.ForEach(bw.Write);
			this.EdgeColor.ForEach(bw.Write);
			bw.Write(this.EdgeSize);
			this.Texture.ForEach(bw.Write);
			this.SubTexture.ForEach(bw.Write);
			this.ToonTexture.ForEach(bw.Write);
		}
	}

	public class PmxGroupMorphOffset : PmxMorphOffset
	{
		public int Morph
		{
			get;
			set;
		}

		public float Weight
		{
			get;
			set;
		}

		public PmxGroupMorphOffset()
		{
		}

		public override void Read(BinaryReader br, PmxDocument doc)
		{
			this.Morph = doc.ReadIndex(br, PmxIndexKind.Morph);
			this.Weight = br.ReadSingle();
		}

		public override void Write(BinaryWriter bw, PmxDocument doc)
		{
			doc.WriteIndex(bw, PmxIndexKind.Morph, this.Morph);
			bw.Write(this.Weight);
		}
	}
}
