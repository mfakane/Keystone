using System.Numerics;

namespace Linearstar.Keystone.IO.MikuMikuDance.Pmx
{
    public class PmxVertex
    {
        public Vector3 Position { get; set; }

        public Vector3 Normal { get; set; }

        public Vector2 UV { get; set; }

        public Vector4[] AdditionalUV { get; set; } = { };

        public PmxSkinningKind SkinningKind { get; set; }

        public PmxSkinningFunction SkinningFunction { get; set; } = new PmxLinearBlendDeforming2();

        public float EdgeSize { get; set; }

        internal static PmxVertex Parse(ref BufferReader br, PmxDocument doc)
        {
            var rt = new PmxVertex
            {
                Position = br.ReadVector3(),
                Normal = br.ReadVector3(),
                UV = br.ReadVector2(),
                AdditionalUV = new Vector4[doc.Header.AdditionalUVCount],
            };

            for (var i = 0; i < doc.Header.AdditionalUVCount; i++)
                rt.AdditionalUV[i] = br.ReadVector4();
            
            rt.SkinningKind = (PmxSkinningKind)br.ReadByte();
            rt.SkinningFunction = PmxSkinningFunction.Parse(ref br, doc, rt.SkinningKind);
            rt.EdgeSize = br.ReadSingle();

            return rt;
        }

        internal void Write(ref BufferWriter bw, PmxDocument doc, PmxIndexCache cache)
        {
            if (doc.Version < 2.1f &&
                this.SkinningKind == PmxSkinningKind.DualQuaternionDeforming)
                this.SkinningKind = PmxSkinningKind.LinearBlendDeforming4;

            bw.Write(this.Position);
            bw.Write(this.Normal);
            bw.Write(this.UV);
            foreach (var additional in this.AdditionalUV) bw.Write(additional);
            bw.Write((byte)this.SkinningKind);
            this.SkinningFunction.Write(ref bw, doc, cache);
            bw.Write(this.EdgeSize);
        }
    }
}