using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	/// <summary>
	/// 拡張モデルファイル created by 極北P
	/// </summary>
	public class PmxDocument
	{
		public const string DisplayName = "Polygon Model Data Extended file";
		public const string Filter = "*.pmd";
		
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

		public List<int> Indices
		{
			get;
			set;
		}

		public List<string> Textures
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

		public PmxDocument()
		{
			this.Version = 2;
			this.Header = new PmxHeader();
			this.ModelInformation = new PmxModelInformation();
			this.Vertices = new List<PmxVertex>();
			this.Indices = new List<int>();
			this.Textures = new List<string>();
			this.Materials = new List<PmxMaterial>();
			this.Bones = new List<PmxBone>();
			this.Morphs = new List<PmxMorph>();
			this.Rigids = new List<PmxRigidBody>();
			this.Constraints = new List<PmxConstraint>();
		}

		public static PmxDocument Parse(Stream stream)
		{
			using (var br = new BinaryReader(stream))
			{
				var header = Encoding.ASCII.GetString(br.ReadBytes(4));
				var rt = new PmxDocument();

				if (header != "PMX ")
					throw new InvalidOperationException("invalid format");

				rt.Version = br.ReadSingle();

				if (rt.Version != 2)
					throw new NotSupportedException("specified format version not supported");

				rt.Header = PmxHeader.Parse(br);
				rt.ModelInformation = PmxModelInformation.Parse(br, rt);
				rt.Vertices = Enumerable.Range(0, br.ReadInt32()).Select(_ => PmxVertex.Parse(br, rt)).ToList();
				rt.Indices = Enumerable.Range(0, br.ReadInt32()).Select(_ => rt.ReadIndex(br, PmxIndexKind.Vertex)).ToList();
				rt.Textures = Enumerable.Range(0, br.ReadInt32()).Select(_ => rt.ReadString(br)).ToList();
				rt.Materials = Enumerable.Range(0, br.ReadInt32()).Select(_ => PmxMaterial.Parse(br, rt)).ToList();
				rt.Bones = Enumerable.Range(0, br.ReadInt32()).Select(_ => PmxBone.Parse(br, rt)).ToList();
				rt.Morphs = Enumerable.Range(0, br.ReadInt32()).Select(_ => PmxMorph.Parse(br, rt)).ToList();
				rt.DisplayList = Enumerable.Range(0, br.ReadInt32()).Select(_ => PmxDisplayList.Parse(br, rt)).ToList();
				rt.Rigids = Enumerable.Range(0, br.ReadInt32()).Select(_ => PmxRigidBody.Parse(br, rt)).ToList();
				rt.Constraints = Enumerable.Range(0, br.ReadInt32()).Select(_ => PmxConstraint.Parse(br, rt)).ToList();

				return rt;
			}
		}

		public void Write(Stream stream)
		{
			using (var bw = new BinaryWriter(stream))
			{
				bw.Write(Encoding.ASCII.GetBytes("PMX "));
				bw.Write(this.Version);
				this.Header.Write(bw);
				this.ModelInformation.Write(bw, this);

				bw.Write(this.Vertices.Count);
				this.Vertices.ForEach(_ => _.Write(bw, this));
				bw.Write(this.Indices.Count);
				this.Indices.ForEach(_ => this.WriteIndex(bw, PmxIndexKind.Bone, _));
				bw.Write(this.Textures.Count);
				this.Textures.ForEach(_ => this.WriteString(bw, _));
				bw.Write(this.Materials.Count);
				this.Materials.ForEach(_ => _.Write(bw, this));
				bw.Write(this.Bones.Count);
				this.Bones.ForEach(_ => _.Write(bw, this));
				bw.Write(this.Morphs.Count);
				this.Morphs.ForEach(_ => _.Write(bw, this));
				bw.Write(this.DisplayList.Count);
				this.DisplayList.ForEach(_ => _.Write(bw, this));
				bw.Write(this.Rigids.Count);
				this.Rigids.ForEach(_ => _.Write(bw, this));
				bw.Write(this.Constraints.Count);
				this.Constraints.ForEach(_ => _.Write(bw, this));
			}
		}

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
