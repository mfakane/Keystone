using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Linearstar.Keystone.IO.MikuMikuDance.Pmd
{
    public class PmdDocument
    {
        public const string DisplayName = "Polygon Model Data file";
        public const string Filter = "*.pmd";
        
        internal static Encoding Encoding => Encoding.GetEncoding(932);

        public float Version { get; set; }

        public string ModelName { get; set; }

        public string Description { get; set; }

        public IList<PmdVertex> Vertices { get; set; }

        public IList<PmdVertex> Indices { get; set; }

        public IList<PmdMaterial> Materials { get; set; }

        public IList<PmdBone> Bones { get; set; }

        public IList<PmdIK> IK { get; set; }

        public IList<PmdMorph> Morphs { get; set; }

        public IList<PmdMorph> MorphDisplayList { get; set; }

        public IList<PmdDisplayList> BoneDisplayList { get; set; }

        public bool EnglishCompatible { get; set; }

        public string EnglishModelName { get; set; }

        public string EnglishDescription { get; set; }

        public IList<string> ToonFileNames { get; set; }

        public IList<PmdRigidBody> Rigids { get; set; }

        public IList<PmdConstraint> Constraints { get; set; }

        static PmdDocument()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
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

        public static PmdDocument Parse(in ReadOnlySequence<byte> sequence)
        {
            var rt = new PmdDocument();
            var br = new BufferReader(sequence);
            var header = br.ReadBytes(3);

            if (!header.SequenceEqual("Pmd"u8))
                throw new InvalidOperationException("invalid format");

            rt.Version = br.ReadSingle();

            if (rt.Version >= 2)
                throw new NotSupportedException("specified format version not supported");

            rt.ModelName = br.ReadString(20);
            rt.Description = br.ReadString(256);

            for (var i = br.ReadInt32() - 1; i >= 0; i--)
                rt.Vertices.Add(PmdVertex.Parse(ref br, rt));

            for (var i = br.ReadInt32() - 1; i >= 0; i--)
                rt.Indices.Add(rt.Vertices[br.ReadUInt16()]);

            for (var i = br.ReadInt32() - 1; i >= 0; i--)
                rt.Materials.Add(PmdMaterial.Parse(ref br));

            var boneCount = br.ReadUInt16();
            
            // ensure instance
            for (var i = rt.Bones.Count; i < boneCount; i++)
                rt.Bones.Add(new PmdBone());

            for (var i = 0; i < boneCount; i++)
                rt.Bones[i].Parse(ref br, rt);

            for (var i = br.ReadUInt16() - 1; i >= 0; i--)
                rt.IK.Add(PmdIK.Parse(ref br, rt));

            var morphs = br.ReadUInt16();
            PmdMorph? morphBase = null;

            for (ushort i = 0; i < morphs; i++)
            {
                var m = PmdMorph.Parse(ref br, rt, morphBase);

                if (m.Kind == PmdMorphKind.None)
                    morphBase = m;
                else
                    rt.Morphs.Add(m);
            }

            for (var i = br.ReadByte() - 1; i >= 0; i--)
                rt.MorphDisplayList.Add(rt.Morphs[br.ReadUInt16() - 1]);

            var visibleBoneCategories = br.ReadByte();

            for (byte i = 0; i < visibleBoneCategories; i++)
                rt.BoneDisplayList.Add(PmdDisplayList.Parse(ref br));

            for (var i = br.ReadInt32() - 1; i >= 0; i--)
            {
                var bone = rt.Bones[br.ReadInt16()];

                rt.BoneDisplayList[br.ReadByte() - 1].Bones.Add(bone);
            }

            if (br.IsCompleted) return rt;
            
            rt.EnglishCompatible = br.ReadBoolean();

            if (rt.EnglishCompatible)
            {
                rt.EnglishModelName = br.ReadString(20);
                rt.EnglishDescription = br.ReadString(256);

                for (ushort i = 0; i < rt.Bones.Count; i++)
                    rt.Bones[i].EnglishName = br.ReadString(20);

                for (ushort i = 0; i < morphs - 1; i++)
                    rt.Morphs[i].EnglishName = br.ReadString(20);

                for (byte i = 0; i < visibleBoneCategories; i++)
                    rt.BoneDisplayList[i].EnglishName = br.ReadString(50);
            }

            if (br.IsCompleted) return rt;

            for (var i = 0; i < 10; i++)
                rt.ToonFileNames.Add(br.ReadString(100));

            if (br.IsCompleted) return rt;
            
            for (var i = br.ReadInt32() - 1; i >= 0; i--)
                rt.Rigids.Add(PmdRigidBody.Parse(ref br));

            for (var i = br.ReadInt32() - 1; i >= 0; i--)
                rt.Constraints.Add(PmdConstraint.Parse(ref br));

            return rt;
        }

        internal PmdBone? GetBone(short idx)
        {
            while (this.Bones.Count <= idx)
                this.Bones.Add(new PmdBone());

            return idx == -1 ? null : this.Bones[idx];
        }

        public void Write(IBufferWriter<byte> writer)
        {
            var bw = new BufferWriter(writer);
            var cache = new PmdIndexCache(this);

            bw.Write("Pmd"u8);
            bw.Write(this.Version);
            bw.Write(this.ModelName, 20);
            bw.Write(this.Description, 256);

            bw.Write((uint)this.Vertices.Count);
            foreach (var vertex in this.Vertices)
                vertex.Write(ref bw, cache);

            bw.Write((uint)this.Indices.Count);
            foreach (var index in this.Indices)
                bw.Write((ushort)cache[index]);

            bw.Write((uint)this.Materials.Count);
            foreach (var material in this.Materials)
                material.Write(ref bw);

            bw.Write((ushort)this.Bones.Count);
            foreach (var bone in this.Bones)
                bone.Write(ref bw, cache);

            bw.Write((ushort)this.IK.Count);
            foreach (var ik in this.IK)
                ik.Write(ref bw, cache);

            bw.Write((ushort)(this.Morphs.Count + 1));

            var morphBase = PmdMorph.CreateMorphBase(this.Morphs);
            var morphBaseIndices = morphBase.Indices
                .Select((x, idx) => Tuple.Create(x, idx))
                .ToDictionary(x => x.Item1, x => x.Item2);

            morphBase.Write(ref bw, cache, morphBaseIndices);
            foreach (var morph in this.Morphs)
                morph.Write(ref bw, cache, morphBaseIndices);

            bw.Write((byte)this.MorphDisplayList.Count);
            foreach (var morph in this.MorphDisplayList)
                bw.Write((short)(this.Morphs.IndexOf(morph) + 1));

            bw.Write((byte)this.BoneDisplayList.Count);
            foreach (var displayList in this.BoneDisplayList)
                displayList.Write(ref bw);

            bw.Write((uint)this.BoneDisplayList.Sum(x => x.Bones.Count));
            foreach (var displayList in this.BoneDisplayList)
            {
                foreach (var item in displayList.Bones.Select((x, i) => new { Bone = x, Index = i }))
                {
                    bw.Write(cache[item.Bone]);
                    bw.Write((byte)item.Index);
                }
            }

            bw.Write(this.EnglishCompatible);

            if (this.EnglishCompatible)
            {
                bw.Write(this.EnglishModelName, 20);
                bw.Write(this.EnglishDescription, 256);

                foreach (var bone in this.Bones)
                    bw.Write(bone.EnglishName, 20);
                
                foreach (var morph in this.Morphs)
                    bw.Write(morph.EnglishName, 20);
                
                foreach (var displayList in this.BoneDisplayList)
                    bw.Write(displayList.EnglishName, 50);
            }

            for (var i = 0; i < 10; i++)
                bw.Write(this.ToonFileNames.ElementAtOrDefault(i) ?? "", 100);

            bw.Write((uint)this.Rigids.Count);
            foreach (var rigid in this.Rigids)
                rigid.Write(ref bw);

            bw.Write((uint)this.Constraints.Count);
            foreach (var constraint in this.Constraints)
                constraint.Write(ref bw);
        }
    }
}