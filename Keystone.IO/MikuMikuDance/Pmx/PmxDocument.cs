using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;

namespace Linearstar.Keystone.IO.MikuMikuDance.Pmx
{
    /// <summary>
    /// 拡張モデルファイル created by 極北P
    /// </summary>
    public class PmxDocument
    {
        public const string DisplayName = "Polygon Model Data Extended file";
        public const string Filter = "*.pmx";

        public float Version { get; set; } = 2;

        public PmxHeader Header { get; set; } = new();

        public PmxModelInformation ModelInformation { get; set; } = new();

        public IList<PmxVertex> Vertices { get; set; } = new List<PmxVertex>();

        public IList<PmxVertex> Indices { get; set; } = new List<PmxVertex>();

        public IList<PmxTexture> Textures { get; set; } = new List<PmxTexture>();

        public IList<PmxMaterial> Materials { get; set; } = new List<PmxMaterial>();

        public IList<PmxBone> Bones { get; set; } = new List<PmxBone>();

        public IList<PmxMorph> Morphs { get; set; } = new List<PmxMorph>();

        public IList<PmxDisplayList> DisplayList { get; set; } = new List<PmxDisplayList>();

        public IList<PmxRigidBody> Rigids { get; set; } = new List<PmxRigidBody>();

        public IList<PmxConstraint> Constraints { get; set; } = new List<PmxConstraint>();

        /// <summary>
        /// (PMX 2.1)
        /// </summary>
        public IList<PmxSoftBody> SoftBodies { get; set; } = new List<PmxSoftBody>();
        
        public static PmxDocument FromFile(string path)
        {
            using var fs = File.OpenRead(path);

            return Parse(fs);
        }

        public static PmxDocument Parse(in ReadOnlySequence<byte> sequence)
        {
            var br = new BufferReader(sequence);
            var header = br.ReadBytes(4);
            var rt = new PmxDocument();

            if (!header.SequenceEqual("PMX "u8))
                throw new InvalidOperationException("invalid format");

            rt.Version = br.ReadSingle();

            if (rt.Version is < 2 or > 2.1f)
                throw new NotSupportedException("specified format version not supported");

            rt.Header = PmxHeader.Parse(ref br);
            rt.ModelInformation = PmxModelInformation.Parse(ref br, rt);

            for (var i = br.ReadInt32() - 1; i >= 0; i--)
                rt.Vertices.Add(PmxVertex.Parse(ref br, rt));
            
            for (var i = br.ReadInt32() - 1; i >= 0; i--)
                rt.Indices.Add(rt.ReadVertex(ref br)!);
            
            for (var i = br.ReadInt32() - 1; i >= 0; i--)
                rt.Textures.Add(PmxTexture.Parse(ref br, rt));
            
            for (var i = br.ReadInt32() - 1; i >= 0; i--)
                rt.Materials.Add(PmxMaterial.Parse(ref br, rt));

            var bones = br.ReadInt32();
            
            // ensure instance
            for (var i = rt.Bones.Count; i < bones; i++)
                rt.Bones.Add(new PmxBone());
            
            for (var i = 0; i < bones; i++)
                rt.Bones[i].Parse(ref br, rt);
            
            var morphs = br.ReadInt32();
            
            // ensure instance
            for (var i = rt.Morphs.Count; i < morphs; i++)
                rt.Morphs.Add(new PmxMorph());
                
            for (var i = 0; i < morphs; i++)
                rt.Morphs[i].Parse(ref br, rt);

            for (var i = br.ReadInt32() - 1; i >= 0; i--)
                rt.DisplayList.Add(PmxDisplayList.Parse(ref br, rt));

            for (var i = br.ReadInt32() - 1; i >= 0; i--)
                rt.Rigids.Add(PmxRigidBody.Parse(ref br, rt));

            for (var i = br.ReadInt32() - 1; i >= 0; i--)
                rt.Constraints.Add(PmxConstraint.Parse(ref br, rt));

            if (rt.Version <= 2) return rt;
            
            for (var i = br.ReadInt32() - 1; i >= 0; i--)
                rt.SoftBodies.Add(PmxSoftBody.Parse(ref br, rt));

            return rt;
        }

        public static PmxDocument Parse(Stream stream) =>
            Parse(stream.AsReadOnlySequence());

        public void Write(IBufferWriter<byte> writer)
        {
            var bw = new BufferWriter(writer);
            var cache = new PmxIndexCache(this);

            this.Header.VertexIndexSize = cache.VertexIndexSize;
            this.Header.MaterialIndexSize = cache.MaterialIndexSize;
            this.Header.TextureIndexSize = cache.TextureIndexSize;
            this.Header.BoneIndexSize = cache.BoneIndexSize;
            this.Header.MorphIndexSize = cache.MorphIndexSize;
            this.Header.RigidIndexSize = cache.RigidIndexSize;

            bw.Write("PMX "u8);
            bw.Write(this.Version);
            this.Header.Write(ref bw);
            this.ModelInformation.Write(ref bw, this);

            bw.Write(this.Vertices.Count);
            foreach (var vertex in this.Vertices)
                vertex.Write(ref bw, this, cache);
            
            bw.Write(this.Indices.Count);
            foreach (var vertex in this.Indices)
                bw.Write(vertex, cache);
            
            bw.Write(this.Textures.Count);
            foreach (var texture in this.Textures)
                texture.Write(ref bw, this);
            
            bw.Write(this.Materials.Count);
            foreach (var material in this.Materials)
                material.Write(ref bw, this, cache);
            
            bw.Write(this.Bones.Count);
            foreach (var bone in this.Bones)
                bone.Write(ref bw, this, cache);
            
            bw.Write(this.Morphs.Count);
            foreach (var morph in this.Morphs)
                morph.Write(ref bw, this, cache);
            
            bw.Write(this.DisplayList.Count);
            foreach (var display in this.DisplayList)
                display.Write(ref bw, this, cache);
            
            bw.Write(this.Rigids.Count);
            foreach (var rigid in this.Rigids)
                rigid.Write(ref bw, this, cache);
            
            bw.Write(this.Constraints.Count);
            foreach (var constraint in this.Constraints)
                constraint.Write(ref bw, this, cache);

            if (this.Version <= 2) return;
            
            bw.Write(this.SoftBodies.Count);
            foreach (var softBody in this.SoftBodies)
                softBody.Write(ref bw, this, cache);
        }

        #region Vertex

        PmxVertex? GetVertexFromIndex(int vertex) => vertex == -1 ? null : this.Vertices[vertex];

        internal PmxVertex? ReadVertex(ref BufferReader br) => GetVertexFromIndex(ReadIndex(ref br, PmxIndexKind.Vertex));

        #endregion

        #region Texture

        PmxTexture? GetTextureFromIndex(int texture) => texture == -1 ? null : this.Textures[texture];

        internal PmxTexture? ReadTexture(ref BufferReader br) => GetTextureFromIndex(ReadIndex(ref br, PmxIndexKind.Texture));

        #endregion

        #region Material

        PmxMaterial? GetMaterialFromIndex(int material) => material == -1 ? null : this.Materials[material];

        internal PmxMaterial? ReadMaterial(ref BufferReader br) => GetMaterialFromIndex(ReadIndex(ref br, PmxIndexKind.Material));

        #endregion

        #region Bone

        PmxBone? GetBoneFromIndex(int bone)
        {
            while (this.Bones.Count <= bone)
                this.Bones.Add(new PmxBone());

            return bone == -1 ? null : this.Bones[bone];
        }

        internal PmxBone? ReadBone(ref BufferReader br) => GetBoneFromIndex(ReadIndex(ref br, PmxIndexKind.Bone));

        #endregion

        #region Morph

        PmxMorph? GetMorphFromIndex(int morph)
        {
            while (this.Morphs.Count <= morph)
                this.Morphs.Add(new PmxMorph());

            return morph == -1 ? null : this.Morphs[morph];
        }

        internal PmxMorph? ReadMorph(ref BufferReader br) => GetMorphFromIndex(ReadIndex(ref br, PmxIndexKind.Morph));

        #endregion

        #region RigidBody

        PmxRigidBody? GetRigidBodyFromIndex(int rigidBody) => rigidBody == -1 ? null : this.Rigids[rigidBody];

        internal PmxRigidBody? ReadRigidBody(ref BufferReader br) => GetRigidBodyFromIndex(ReadIndex(ref br, PmxIndexKind.Rigid));

        #endregion

        int ReadIndex(ref BufferReader br, PmxIndexKind kind)
        {
            if (kind == PmxIndexKind.Vertex)
            {
                return this.Header.VertexIndexSize == PmxVertexIndexSize.UInt8
                    ? br.ReadByte()
                    : this.Header.VertexIndexSize == PmxVertexIndexSize.UInt16
                        ? br.ReadUInt16()
                        : br.ReadInt32();
            }

            var indexSize = kind switch
            {
                PmxIndexKind.Texture => this.Header.TextureIndexSize,
                PmxIndexKind.Material => this.Header.MaterialIndexSize,
                PmxIndexKind.Bone => this.Header.BoneIndexSize,
                PmxIndexKind.Morph => this.Header.MorphIndexSize,
                PmxIndexKind.Rigid => this.Header.RigidIndexSize,
                _ => throw new InvalidOperationException(),
            };

            return indexSize switch
            {
                PmxIndexSize.Int8 => br.ReadSByte(),
                PmxIndexSize.Int16 => br.ReadInt16(),
                PmxIndexSize.Int32 => br.ReadInt32(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
		
        public void Write(Stream stream)
        {
            using var sbw = new StreamBufferWriter(stream);

            Write(sbw);
        }
    }
}