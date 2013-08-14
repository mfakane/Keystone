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

		public IList<PmdVertex> Indices
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

		public IList<PmdMorph> MorphDisplayList
		{
			get;
			set;
		}

		public IList<PmdDisplayList> BoneDisplayList
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
			this.Indices = new List<PmdVertex>();
			this.Materials = new List<PmdMaterial>();
			this.Bones = new List<PmdBone>();
			this.IK = new List<PmdIK>();
			this.Morphs = new List<PmdMorph>();
			this.MorphDisplayList = new List<PmdMorph>();
			this.BoneDisplayList = new List<PmdDisplayList>();
			this.ToonFileNames = new List<string>();
			this.Rigids = new List<PmdRigidBody>();
			this.Constraints = new List<PmdConstraint>();
		}

		public static PmdDocument Parse(Stream stream)
		{
			var rt = new PmdDocument();

			// leave open
			var br = new BinaryReader(stream);
			var header = ReadPmdString(br, 3);

			if (header != "Pmd")
				throw new InvalidOperationException("invalid format");

			rt.Version = br.ReadSingle();

			if (rt.Version >= 2)
				throw new NotSupportedException("specified format version not supported");

			rt.ModelName = ReadPmdString(br, 20);
			rt.Description = ReadPmdString(br, 256);

			for (var i = br.ReadInt32() - 1; i >= 0; i--)
				rt.Vertices.Add(PmdVertex.Parse(br, rt));

			for (var i = br.ReadInt32() - 1; i >= 0; i--)
				rt.Indices.Add(rt.Vertices[br.ReadUInt16()]);

			for (var i = br.ReadInt32() - 1; i >= 0; i--)
				rt.Materials.Add(PmdMaterial.Parse(br));

			Enumerable.Range(0, br.ReadUInt16()).Select(_ =>
			{
				while (rt.Bones.Count <= _)
					rt.Bones.Add(new PmdBone());

				return rt.Bones[_];
			}).ForEach(_ => _.Parse(br, rt));

			for (var i = br.ReadUInt16() - 1; i >= 0; i--)
				rt.IK.Add(PmdIK.Parse(br, rt));

			var morphs = br.ReadUInt16();
			PmdMorph morphBase = null;

			for (ushort i = 0; i < morphs; i++)
			{
				var m = PmdMorph.Parse(br, rt, morphBase);

				if (m.Kind == PmdMorphKind.None)
					morphBase = m;
				else
					rt.Morphs.Add(m);
			}

			for (var i = br.ReadByte() - 1; i >= 0; i--)
				rt.MorphDisplayList.Add(rt.Morphs[br.ReadUInt16() - 1]);

			var visibleBoneCategories = br.ReadByte();

			for (byte i = 0; i < visibleBoneCategories; i++)
				rt.BoneDisplayList.Add(PmdDisplayList.Parse(br));

			for (var i = br.ReadInt32() - 1; i >= 0; i--)
			{
				var bone = rt.Bones[br.ReadInt16()];

				rt.BoneDisplayList[br.ReadByte() - 1].Bones.Add(bone);
			}

			if (br.GetRemainingLength() > 0)
			{
				rt.EnglishCompatible = br.ReadBoolean();

				if (rt.EnglishCompatible)
				{
					rt.EnglishModelName = ReadPmdString(br, 20);
					rt.EnglishDescription = ReadPmdString(br, 256);

					for (ushort i = 0; i < rt.Bones.Count; i++)
						rt.Bones[i].EnglishName = ReadPmdString(br, 20);

					for (ushort i = 0; i < morphs - 1; i++)
						rt.Morphs[i].EnglishName = ReadPmdString(br, 20);

					for (byte i = 0; i < visibleBoneCategories; i++)
						rt.BoneDisplayList[i].EnglishName = ReadPmdString(br, 50);
				}

				if (br.GetRemainingLength() > 0)
					rt.ToonFileNames = Enumerable.Range(0, 10).Select(_ => ReadPmdString(br, 100)).ToList();

				if (br.GetRemainingLength() > 0)
				{
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

		internal PmdBone GetBone(short idx)
		{
			while (this.Bones.Count <= idx)
				this.Bones.Add(new PmdBone());

			return idx == -1 ? null : this.Bones[idx];
		}

		public void Write(Stream stream)
		{
			// leave open
			var bw = new BinaryWriter(stream);
			var cache = new PmdIndexCache(this);

			bw.Write(Encoding.GetBytes("Pmd"));
			bw.Write(this.Version);
			WritePmdString(bw, this.ModelName, 20);
			WritePmdString(bw, this.Description, 256);

			bw.Write((uint)this.Vertices.Count);
			this.Vertices.ForEach(_ => _.Write(bw, cache));

			bw.Write((uint)this.Indices.Count);
			this.Indices.Select(_ => (ushort)cache.Vertices[_]).ForEach(bw.Write);

			bw.Write((uint)this.Materials.Count);
			this.Materials.ForEach(_ => _.Write(bw));

			bw.Write((ushort)this.Bones.Count);
			this.Bones.ForEach(_ => _.Write(bw, cache));

			bw.Write((ushort)this.IK.Count);
			this.IK.ForEach(_ => _.Write(bw, cache));

			bw.Write((ushort)(this.Morphs.Count + 1));

			var morphBase = PmdMorph.CreateMorphBase(this.Morphs);
			var morphBaseIndices = morphBase.Indices.Select((_, idx) => Tuple.Create(_, idx)).ToDictionary(_ => _.Item1, _ => _.Item2);

			morphBase.Write(bw, cache, morphBaseIndices);
			this.Morphs.ForEach(_ => _.Write(bw, cache, morphBaseIndices));

			bw.Write((byte)this.MorphDisplayList.Count);
			this.MorphDisplayList.Select(_ => this.Morphs.IndexOf(_) + 1).ForEach(_ => bw.Write((short)_));

			bw.Write((byte)this.BoneDisplayList.Count);
			this.BoneDisplayList.ForEach(_ => _.Write(bw));

			bw.Write((uint)this.BoneDisplayList.Sum(_ => _.Bones.Count));
			this.BoneDisplayList.SelectMany((_, idx) => _.Bones.Select(b => Tuple.Create(b, idx + 1))).ForEach(_ =>
			{
				bw.Write((short)this.Bones.IndexOf(_.Item1));
				bw.Write((byte)_.Item2);
			});

			bw.Write(this.EnglishCompatible);

			if (this.EnglishCompatible)
			{
				WritePmdString(bw, this.EnglishModelName, 20);
				WritePmdString(bw, this.EnglishDescription, 256);
				this.Bones.Select(_ => _.EnglishName).ForEach(_ => WritePmdString(bw, _, 20));
				this.Morphs.Select(_ => _.EnglishName).ForEach(_ => WritePmdString(bw, _, 20));
				this.BoneDisplayList.Select(_ => _.EnglishName).ForEach(_ => WritePmdString(bw, _, 50));
			}

			Enumerable.Range(0, 10).Select(_ => _ < this.ToonFileNames.Count ? this.ToonFileNames[_] : null).ForEach(_ => WritePmdString(bw, _, 100));

			bw.Write((uint)this.Rigids.Count);
			this.Rigids.ForEach(_ => _.Write(bw));

			bw.Write((uint)this.Constraints.Count);
			this.Constraints.ForEach(_ => _.Write(bw));
		}
	}
}
