using System.Collections.Generic;
using System.Linq;

namespace Linearstar.Keystone.IO.MikuMikuDance.Pmx
{
    class PmxIndexCache
    {
        readonly PmxDocument doc;
        readonly Dictionary<PmxVertex, int> vertices;
        readonly Dictionary<PmxTexture, int> textures;
        readonly Dictionary<PmxMaterial, int> materials;
        readonly Dictionary<PmxBone, int> bones;
        readonly Dictionary<PmxMorph, int> morphs;
        readonly Dictionary<PmxRigidBody, int> rigids;

        public PmxHeader Header => doc.Header;

        public int this[PmxVertex? vertex] =>
            vertex != null && vertices.TryGetValue(vertex, out var index) ? index : -1;

        public int this[PmxTexture? texture] =>
            texture != null && textures.TryGetValue(texture, out var index) ? index : -1;
        
        public int this[PmxMaterial? material] =>
            material != null && materials.TryGetValue(material, out var index) ? index : -1;

        public int this[PmxBone? bone] => 
            bone != null && bones.TryGetValue(bone, out var index) ? index : -1;
        
        public int this[PmxMorph? morph] => 
            morph != null && morphs.TryGetValue(morph, out var index) ? index : -1;
        
        public int this[PmxRigidBody? rigid] =>
            rigid != null && rigids.TryGetValue(rigid, out var index) ? index : -1;
        
        public PmxVertexIndexSize VertexIndexSize =>
            vertices.Count <= byte.MaxValue ? PmxVertexIndexSize.UInt8 :
            vertices.Count <= ushort.MaxValue ? PmxVertexIndexSize.UInt16 :
            PmxVertexIndexSize.Int32;
        
        public PmxIndexSize TextureIndexSize =>
            textures.Count <= sbyte.MaxValue ? PmxIndexSize.Int8 :
            textures.Count <= short.MaxValue ? PmxIndexSize.Int16 :
            PmxIndexSize.Int32;
        
        public PmxIndexSize MaterialIndexSize =>
            materials.Count <= sbyte.MaxValue ? PmxIndexSize.Int8 :
            materials.Count <= short.MaxValue ? PmxIndexSize.Int16 :
            PmxIndexSize.Int32;
        
        public PmxIndexSize BoneIndexSize =>
            bones.Count <= sbyte.MaxValue ? PmxIndexSize.Int8 :
            bones.Count <= short.MaxValue ? PmxIndexSize.Int16 :
            PmxIndexSize.Int32;
        
        public PmxIndexSize MorphIndexSize =>
            morphs.Count <= sbyte.MaxValue ? PmxIndexSize.Int8 :
            morphs.Count <= short.MaxValue ? PmxIndexSize.Int16 :
            PmxIndexSize.Int32;
        
        public PmxIndexSize RigidIndexSize =>
            rigids.Count <= sbyte.MaxValue ? PmxIndexSize.Int8 :
            rigids.Count <= short.MaxValue ? PmxIndexSize.Int16 :
            PmxIndexSize.Int32;
        
        public PmxIndexCache(PmxDocument doc)
        {
            this.doc = doc;
            vertices = doc.Vertices.Select((x, i) => new { Item = x, Index = i })
                .ToDictionary(x => x.Item, x => x.Index);
            textures = doc.Textures.Select((x, i) => new { Item = x, Index = i })
                .ToDictionary(x => x.Item, x => x.Index);
            materials = doc.Materials.Select((x, i) => new { Item = x, Index = i })
                .ToDictionary(x => x.Item, x => x.Index);
            bones = doc.Bones.Select((x, i) => new { Item = x, Index = i }).ToDictionary(x => x.Item, x => x.Index);
            morphs = doc.Morphs.Select((x, i) => new { Item = x, Index = i }).ToDictionary(x => x.Item, x => x.Index);
            rigids = doc.Rigids.Select((x, i) => new { Item = x, Index = i }).ToDictionary(x => x.Item, x => x.Index);
        }
    }
}