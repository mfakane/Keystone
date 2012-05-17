using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public class PmdDocument
	{
		public const string DisplayName = "Polygon Model Data file";
		public const string Filter = "*.pmd";
		public static readonly Encoding Encoding = Encoding.GetEncoding(932);

		public float Version
		{
			get;
			set;
		}

		public string ModelName
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public IList<PmdVertex> Vertices
		{
			get;
			set;
		}

		public IList<ushort> Indices
		{
			get;
			set;
		}

		public IList<PmdMaterial> Materials
		{
			get;
			set;
		}

		public IList<PmdBone> Bones
		{
			get;
			set;
		}

		public IList<PmdIK> IK
		{
			get;
			set;
		}

		public IList<PmdMorph> Morphs
		{
			get;
			set;
		}

		public IList<ushort> VisibleMorphs
		{
			get;
			set;
		}

		public IList<string> VisibleBoneCategories
		{
			get;
			set;
		}

		public IDictionary<short, byte> VisibleBones
		{
			get;
			set;
		}

		public bool EnglishCompatible
		{
			get;
			set;
		}

		public string EnglishModelName
		{
			get;
			set;
		}

		public string EnglishDescription
		{
			get;
			set;
		}

		public IList<string> EnglishBoneNames
		{
			get;
			set;
		}

		public IList<string> EnglishMorphNames
		{
			get;
			set;
		}

		public IList<string> EnglishVisibleBoneCategories
		{
			get;
			set;
		}

		public IList<string> ToonFileNames
		{
			get;
			set;
		}

		public IList<PmdRigidBody> Rigids
		{
			get;
			set;
		}

		public IList<PmdConstraint> Constraints
		{
			get;
			set;
		}

		public PmdDocument()
		{
			this.Vertices = new List<PmdVertex>();
			this.Indices = new List<ushort>();
			this.Materials = new List<PmdMaterial>();
			this.Bones = new List<PmdBone>();
			this.IK = new List<PmdIK>();
			this.Morphs = new List<PmdMorph>();
			this.VisibleMorphs = new List<ushort>();
			this.VisibleBoneCategories = new List<string>();
			this.VisibleBones = new Dictionary<short, byte>();
			this.EnglishBoneNames = new List<string>();
			this.EnglishMorphNames = new List<string>();
			this.EnglishVisibleBoneCategories = new List<string>();
			this.ToonFileNames = new List<string>();
			this.Rigids = new List<PmdRigidBody>();
			this.Constraints = new List<PmdConstraint>();
		}

		public static PmdDocument Parse(Stream stream)
		{
			var rt = new PmdDocument();

			using (var br = new BinaryReader(stream))
			{
				var header = ReadPmdString(br, 3);

				if (header != "Pmd")
					throw new InvalidOperationException("invalid format");

				rt.Version = br.ReadSingle();

				if (rt.Version >= 2)
					throw new NotSupportedException("specified format version not supported");

				rt.ModelName = ReadPmdString(br, 20);
				rt.Description = ReadPmdString(br, 256);

				for (var i = br.ReadInt32() - 1; i >= 0; i--)
					rt.Vertices.Add(PmdVertex.Parse(br));

				for (var i = br.ReadInt32() - 1; i >= 0; i--)
					rt.Indices.Add(br.ReadUInt16());

				for (var i = br.ReadInt32() - 1; i >= 0; i--)
					rt.Materials.Add(PmdMaterial.Parse(br));

				var bones = br.ReadUInt16();

				for (ushort i = 0; i < bones; i++)
					rt.Bones.Add(PmdBone.Parse(br));

				for (var i = br.ReadUInt16() - 1; i >= 0; i--)
					rt.IK.Add(PmdIK.Parse(br));

				var morphs = br.ReadUInt16();

				for (ushort i = 0; i < morphs; i++)
					rt.Morphs.Add(PmdMorph.Parse(br));

				for (var i = br.ReadByte() - 1; i >= 0; i--)
					rt.VisibleMorphs.Add(br.ReadUInt16());

				var visibleBoneCategories = br.ReadByte();

				for (byte i = 0; i < visibleBoneCategories; i++)
					rt.VisibleBoneCategories.Add(ReadPmdString(br, 50));

				for (var i = br.ReadInt32() - 1; i >= 0; i--)
					rt.VisibleBones.Add(br.ReadInt16(), br.ReadByte());

				if (br.GetRemainingLength() > 0)
				{
					rt.EnglishCompatible = br.ReadBoolean();
					rt.EnglishModelName = ReadPmdString(br, 20);
					rt.EnglishDescription = ReadPmdString(br, 256);

					for (ushort i = 0; i < bones; i++)
						rt.EnglishBoneNames.Add(ReadPmdString(br, 20));

					for (ushort i = 0; i < morphs - 1; i++)
						rt.EnglishMorphNames.Add(ReadPmdString(br, 20));

					for (byte i = 0; i < visibleBoneCategories; i++)
						rt.EnglishVisibleBoneCategories.Add(ReadPmdString(br, 50));

					rt.ToonFileNames = Enumerable.Range(0, 10).Select(_ => ReadPmdString(br, 100)).ToList();

					for (var i = br.ReadInt32() - 1; i >= 0; i--)
						rt.Rigids.Add(PmdRigidBody.Parse(br));

					for (var i = br.ReadInt32() - 1; i >= 0; i--)
						rt.Constraints.Add(PmdConstraint.Parse(br));
				}
			}

			return rt;
		}

		internal static string ReadPmdString(BinaryReader br, int count)
		{
			return Encoding.GetString(br.ReadBytes(count).TakeWhile(_ => _ != '\0').ToArray());
		}

		internal static void WritePmdString(BinaryWriter bw, string s, int count, byte padding = 0xFD)
		{
			var bytes = Encoding.GetBytes(s ?? "");

			bw.Write(bytes.Take(count - 1).ToArray());
			bw.Write(Enumerable.Repeat(padding, Math.Max(count - bytes.Length, 1)).Select((_, idx) => idx == 0 ? (byte)0 : _).ToArray());
		}

		public void Write(Stream stream)
		{
			using (var bw = new BinaryWriter(stream))
			{
				bw.Write(Encoding.GetBytes("Pmd"));
				bw.Write(this.Version);
				WritePmdString(bw, this.ModelName, 20);
				WritePmdString(bw, this.Description, 256);

				bw.Write((uint)this.Vertices.Count);
				this.Vertices.ForEach(_ => _.Write(bw));

				bw.Write((uint)this.Indices.Count);
				this.Indices.ForEach(bw.Write);

				bw.Write((uint)this.Materials.Count);
				this.Materials.ForEach(_ => _.Write(bw));

				bw.Write((ushort)this.Bones.Count);
				this.Bones.ForEach(_ => _.Write(bw));

				bw.Write((ushort)this.IK.Count);
				this.IK.ForEach(_ => _.Write(bw));

				bw.Write((ushort)this.Morphs.Count);
				this.Morphs.ForEach(_ => _.Write(bw));

				bw.Write((byte)this.VisibleMorphs.Count);
				this.VisibleMorphs.ForEach(_ => bw.Write(_));

				bw.Write((byte)this.VisibleBoneCategories.Count);
				this.VisibleBoneCategories.ForEach(_ => WritePmdString(bw, _, 50));

				bw.Write((uint)this.VisibleBones.Count);
				this.VisibleBones.ForEach(_ =>
				{
					bw.Write(_.Key);
					bw.Write(_.Value);
				});

				bw.Write(this.EnglishCompatible);
				WritePmdString(bw, this.EnglishModelName, 20);
				WritePmdString(bw, this.EnglishDescription, 256);
				Enumerable.Range(0, this.Bones.Count).Select(_ => _ < this.EnglishBoneNames.Count ? this.EnglishBoneNames[_] : null).ForEach(_ => WritePmdString(bw, _, 20));
				Enumerable.Range(0, this.Morphs.Count - 1).Select(_ => _ < this.EnglishMorphNames.Count ? this.EnglishMorphNames[_] : null).ForEach(_ => WritePmdString(bw, _, 20));
				Enumerable.Range(0, this.VisibleBoneCategories.Count).Select(_ => _ < this.EnglishVisibleBoneCategories.Count ? this.EnglishVisibleBoneCategories[_] : null).ForEach(_ => WritePmdString(bw, _, 50));

				Enumerable.Range(0, 10).Select(_ => _ < this.ToonFileNames.Count ? this.ToonFileNames[_] : null).ForEach(_ => WritePmdString(bw, _, 100));

				bw.Write((uint)this.Rigids.Count);
				this.Rigids.ForEach(_ => _.Write(bw));

				bw.Write((uint)this.Constraints.Count);
				this.Constraints.ForEach(_ => _.Write(bw));
			}
		}
	}
}
