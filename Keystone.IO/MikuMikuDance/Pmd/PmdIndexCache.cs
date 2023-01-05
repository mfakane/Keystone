using System.Collections.Generic;
using System.Linq;

namespace Linearstar.Keystone.IO.MikuMikuDance.Pmd
{
    class PmdIndexCache
    {
        readonly Dictionary<PmdVertex, uint> vertices;
        readonly Dictionary<PmdBone, short> bones;

        public uint this[PmdVertex? vertex] =>
            vertex is null ? unchecked((uint)-1) : vertices[vertex];

        public short this[PmdBone? bone] =>
            bone is null ? (short)-1 : bones[bone];

        public PmdIndexCache(PmdDocument doc)
        {
            vertices = doc.Vertices.Select((x, i) => new { Item = x, Index = (uint)i })
                .ToDictionary(x => x.Item, x => x.Index);
            bones = doc.Bones.Select((x, i) => new { Item = x, Index = (short)i })
                .ToDictionary(x => x.Item, x => x.Index);
        }
    }
}