using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Linearstar.Keystone.IO.MikuMikuDance.Pmx;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	/// <summary>
	/// 拡張モデルファイル created by 極北P
	/// </summary>
	public class PmxDocument
	{
		public const string DisplayName = "Polygon Model Data Extended file";
		public const string Filter = "*.pmx";

		public float Version
		{
			get;
			set;
		}

		public PmxHeader Header
		{
			get;
			set;
		}

		public PmxModelInformation ModelInformation
		{
			get;
			set;
		}

		public List<PmxVertex> Vertices
		{
			get;
			set;
		}

		public List<PmxVertex> Indices
		{
			get;
			set;
		}

		public List<PmxTexture> Textures
		{
			get;
			set;
		}

		public List<PmxMaterial> Materials
		{
			get;
			set;
		}

		public List<PmxBone> Bones
		{
			get;
			set;
		}

		public List<PmxMorph> Morphs
		{
			get;
			set;
		}

		public List<PmxDisplayList> DisplayList
		{
			get;
			set;
		}

		public List<PmxRigidBody> Rigids
		{
			get;
			set;
		}

		public List<PmxConstraint> Constraints
		{
			get;
			set;
		}

		/// <summary>
		/// (PMX 2.1)
		/// </summary>
		public List<PmxSoftBody> SoftBodies
		{
			get;
			set;
		}

		public PmxDocument()
		{
			this.Version = 2;
			this.Header = new PmxHeader();
			this.ModelInformation = new PmxModelInformation();
			this.Vertices = new List<PmxVertex>();
			this.Indices = new List<PmxVertex>();
			this.Textures = new List<PmxTexture>();
			this.Materials = new List<PmxMaterial>();
			this.Bones = new List<PmxBone>();
			this.Morphs = new List<PmxMorph>();
			this.Rigids = new List<PmxRigidBody>();
			this.Constraints = new List<PmxConstraint>();
			this.SoftBodies = new List<PmxSoftBody>();
		}

		public static PmxDocument Parse(Stream stream)
		{
			// leave open
			var br = new BinaryReader(stream);
			var header = Encoding.ASCII.GetString(br.ReadBytes(4));
			var rt = new PmxDocument();

			if (header != "PMX ")
				throw new InvalidOperationException("invalid format");

			rt.Version = br.ReadSingle();

			if (rt.Version != 2 &&
				rt.Version != 2.1f)
				throw new NotSupportedException("specified format version not supported");

			rt.Header = PmxHeader.Parse(br);
			rt.ModelInformation = PmxModelInformation.Parse(br, rt);
			rt.Vertices = Enumerable.Range(0, br.ReadInt32()).Select(_ => PmxVertex.Parse(br, rt)).ToList();
			rt.Indices = Enumerable.Range(0, br.ReadInt32()).Select(_ => rt.ReadVertex(br)).ToList();
			rt.Textures = Enumerable.Range(0, br.ReadInt32()).Select(_ => PmxTexture.Parse(br, rt)).ToList();
			rt.Materials = Enumerable.Range(0, br.ReadInt32()).Select(_ => PmxMaterial.Parse(br, rt)).ToList();
			Enumerable.Range(0, br.ReadInt32()).Select(_ =>
			{
				while (rt.Bones.Count <= _)
					rt.Bones.Add(new PmxBone());

				return rt.Bones[_];
			}).ForEach(_ => _.Parse(br, rt));
			Enumerable.Range(0, br.ReadInt32()).Select(_ =>
			{
				while (rt.Morphs.Count <= _)
					rt.Morphs.Add(new PmxMorph());

				return rt.Morphs[_];
			}).ForEach(_ => _.Parse(br, rt));
			rt.DisplayList = Enumerable.Range(0, br.ReadInt32()).Select(_ => PmxDisplayList.Parse(br, rt)).ToList();
			rt.Rigids = Enumerable.Range(0, br.ReadInt32()).Select(_ => PmxRigidBody.Parse(br, rt)).ToList();
			rt.Constraints = Enumerable.Range(0, br.ReadInt32()).Select(_ => PmxConstraint.Parse(br, rt)).ToList();

			if (rt.Version > 2)
				rt.SoftBodies = Enumerable.Range(0, br.ReadInt32()).Select(_ => PmxSoftBody.Parse(br, rt)).ToList();

			return rt;
		}

		public void Write(Stream stream)
		{
			// leave open
			var bw = new BinaryWriter(stream);
			var cache = new PmxIndexCache(this, bw);

			this.Header.VertexIndexSize =
				cache.Vertices.Count <= byte.MaxValue ? PmxVertexIndexSize.UInt8 :
				cache.Vertices.Count <= ushort.MaxValue ? PmxVertexIndexSize.UInt16 : PmxVertexIndexSize.Int32;
			this.Header.MaterialIndexSize =
				cache.Materials.Count <= sbyte.MaxValue ? PmxIndexSize.Int8 :
				cache.Materials.Count <= short.MaxValue ? PmxIndexSize.Int16 : PmxIndexSize.Int32;
			this.Header.TextureIndexSize =
				cache.Textures.Count <= sbyte.MaxValue ? PmxIndexSize.Int8 :
				cache.Textures.Count <= short.MaxValue ? PmxIndexSize.Int16 : PmxIndexSize.Int32;
			this.Header.BoneIndexSize =
				cache.Bones.Count <= sbyte.MaxValue ? PmxIndexSize.Int8 :
				cache.Bones.Count <= short.MaxValue ? PmxIndexSize.Int16 : PmxIndexSize.Int32;
			this.Header.MorphIndexSize =
				cache.Morphs.Count <= sbyte.MaxValue ? PmxIndexSize.Int8 :
				cache.Morphs.Count <= short.MaxValue ? PmxIndexSize.Int16 : PmxIndexSize.Int32;
			this.Header.RigidIndexSize =
				cache.Rigids.Count <= sbyte.MaxValue ? PmxIndexSize.Int8 :
				cache.Rigids.Count <= short.MaxValue ? PmxIndexSize.Int16 : PmxIndexSize.Int32;

			bw.Write(Encoding.ASCII.GetBytes("PMX "));
			bw.Write(this.Version);
			this.Header.Write(bw);
			this.ModelInformation.Write(bw, this);

			bw.Write(this.Vertices.Count);
			this.Vertices.ForEach(_ => _.Write(bw, this, cache));
			bw.Write(this.Indices.Count);
			this.Indices.ForEach(_ => cache.Write(_));
			bw.Write(this.Textures.Count);
			this.Textures.ForEach(_ => _.Write(bw, this));
			bw.Write(this.Materials.Count);
			this.Materials.ForEach(_ => _.Write(bw, this, cache));
			bw.Write(this.Bones.Count);
			this.Bones.ForEach(_ => _.Write(bw, this, cache));
			bw.Write(this.Morphs.Count);
			this.Morphs.ForEach(_ => _.Write(bw, this, cache));
			bw.Write(this.DisplayList.Count);
			this.DisplayList.ForEach(_ => _.Write(bw, this, cache));
			bw.Write(this.Rigids.Count);
			this.Rigids.ForEach(_ => _.Write(bw, this, cache));
			bw.Write(this.Constraints.Count);
			this.Constraints.ForEach(_ => _.Write(bw, this, cache));

			if (this.Version > 2)
			{
				bw.Write(this.SoftBodies.Count);
				this.SoftBodies.ForEach(_ => _.Write(bw, this, cache));
			}
		}


		#region Vertex

		PmxVertex GetVertexFromIndex(int vertex)
		{
			return vertex == -1 ? null : this.Vertices[vertex];
		}

		internal PmxVertex ReadVertex(BinaryReader br)
		{
			return GetVertexFromIndex(ReadIndex(br, PmxIndexKind.Vertex));
		}

		#endregion
		#region Texture

		PmxTexture GetTextureFromIndex(int texture)
		{
			return texture == -1 ? null : this.Textures[texture];
		}

		internal PmxTexture ReadTexture(BinaryReader br)
		{
			return GetTextureFromIndex(ReadIndex(br, PmxIndexKind.Texture));
		}

		#endregion
		#region Material

		PmxMaterial GetMaterialFromIndex(int material)
		{
			return material == -1 ? null : this.Materials[material];
		}

		internal PmxMaterial ReadMaterial(BinaryReader br)
		{
			return GetMaterialFromIndex(ReadIndex(br, PmxIndexKind.Material));
		}

		#endregion
		#region Bone

		PmxBone GetBoneFromIndex(int bone)
		{
			while (this.Bones.Count <= bone)
				this.Bones.Add(new PmxBone());

			return bone == -1 ? null : this.Bones[bone];
		}

		internal PmxBone ReadBone(BinaryReader br)
		{
			return GetBoneFromIndex(ReadIndex(br, PmxIndexKind.Bone));
		}

		#endregion
		#region Morph

		PmxMorph GetMorphFromIndex(int morph)
		{
			while (this.Morphs.Count <= morph)
				this.Morphs.Add(new PmxMorph());

			return morph == -1 ? null : this.Morphs[morph];
		}

		internal PmxMorph ReadMorph(BinaryReader br)
		{
			return GetMorphFromIndex(ReadIndex(br, PmxIndexKind.Morph));
		}

		#endregion
		#region RigidBody

		PmxRigidBody GetRigidBodyFromIndex(int rigidBody)
		{
			return rigidBody == -1 ? null : this.Rigids[rigidBody];
		}

		internal PmxRigidBody ReadRigidBody(BinaryReader br)
		{
			return GetRigidBodyFromIndex(ReadIndex(br, PmxIndexKind.Rigid));
		}

		#endregion

		internal string ReadString(BinaryReader br)
		{
			return this.Header.Encoding.GetString(br.ReadSizedBuffer());
		}

		internal void WriteString(BinaryWriter bw, string value)
		{
			bw.WriteSizedBuffer(this.Header.Encoding.GetBytes(value));
		}

		internal int ReadIndex(BinaryReader br, PmxIndexKind kind)
		{
			switch (kind)
			{
				case PmxIndexKind.Vertex:
					return this.Header.VertexIndexSize == PmxVertexIndexSize.UInt8
						? br.ReadByte()
						: this.Header.VertexIndexSize == PmxVertexIndexSize.UInt16
							? br.ReadUInt16()
							: br.ReadInt32();
				case PmxIndexKind.Texture:
					return this.Header.TextureIndexSize == PmxIndexSize.Int8
						? br.ReadSByte()
						: this.Header.TextureIndexSize == PmxIndexSize.Int16
							? br.ReadInt16()
							: br.ReadInt32();
				case PmxIndexKind.Material:
					return this.Header.MaterialIndexSize == PmxIndexSize.Int8
						? br.ReadSByte()
						: this.Header.MaterialIndexSize == PmxIndexSize.Int16
							? br.ReadInt16()
							: br.ReadInt32();
				case PmxIndexKind.Bone:
					return this.Header.BoneIndexSize == PmxIndexSize.Int8
						? br.ReadSByte()
						: this.Header.BoneIndexSize == PmxIndexSize.Int16
							? br.ReadInt16()
							: br.ReadInt32();
				case PmxIndexKind.Morph:
					return this.Header.MorphIndexSize == PmxIndexSize.Int8
						? br.ReadSByte()
						: this.Header.MorphIndexSize == PmxIndexSize.Int16
							? br.ReadInt16()
							: br.ReadInt32();
				case PmxIndexKind.Rigid:
					return this.Header.RigidIndexSize == PmxIndexSize.Int8
						? br.ReadSByte()
						: this.Header.RigidIndexSize == PmxIndexSize.Int16
							? br.ReadInt16()
							: br.ReadInt32();
				default:
					throw new ArgumentException();
			}
		}

		internal void WriteIndex(BinaryWriter bw, PmxIndexKind kind, int index)
		{
			switch (kind)
			{
				case PmxIndexKind.Vertex:
					switch (this.Header.VertexIndexSize)
					{
						case PmxVertexIndexSize.UInt8:
							bw.Write((byte)index);

							break;
						case PmxVertexIndexSize.UInt16:
							bw.Write((ushort)index);

							break;
						case PmxVertexIndexSize.Int32:
							bw.Write(index);

							break;
					}

					break;
				case PmxIndexKind.Texture:
					switch (this.Header.TextureIndexSize)
					{
						case PmxIndexSize.Int8:
							bw.Write((byte)index);

							break;
						case PmxIndexSize.Int16:
							bw.Write((short)index);

							break;
						case PmxIndexSize.Int32:
							bw.Write(index);

							break;
					}

					break;
				case PmxIndexKind.Material:
					switch (this.Header.MaterialIndexSize)
					{
						case PmxIndexSize.Int8:
							bw.Write((byte)index);

							break;
						case PmxIndexSize.Int16:
							bw.Write((short)index);

							break;
						case PmxIndexSize.Int32:
							bw.Write(index);

							break;
					}

					break;
				case PmxIndexKind.Bone:
					switch (this.Header.BoneIndexSize)
					{
						case PmxIndexSize.Int8:
							bw.Write((byte)index);

							break;
						case PmxIndexSize.Int16:
							bw.Write((short)index);

							break;
						case PmxIndexSize.Int32:
							bw.Write(index);

							break;
					}

					break;
				case PmxIndexKind.Morph:
					switch (this.Header.MorphIndexSize)
					{
						case PmxIndexSize.Int8:
							bw.Write((byte)index);

							break;
						case PmxIndexSize.Int16:
							bw.Write((short)index);

							break;
						case PmxIndexSize.Int32:
							bw.Write(index);

							break;
					}

					break;
				case PmxIndexKind.Rigid:
					switch (this.Header.RigidIndexSize)
					{
						case PmxIndexSize.Int8:
							bw.Write((byte)index);

							break;
						case PmxIndexSize.Int16:
							bw.Write((short)index);

							break;
						case PmxIndexSize.Int32:
							bw.Write(index);

							break;
					}

					break;
				default:
					throw new ArgumentException();
			}
		}
	}
}
