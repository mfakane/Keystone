namespace Linearstar.Keystone.IO.MikuMikuDance.Pmx
{
    /// <summary>
    /// (PMX 2.1)
    /// </summary>
    public class PmxSoftBodyAnchor
    {
        public PmxRigidBody? Rigid { get; set; }

        public PmxVertex? Vertex { get; set; }

        public bool IsNearMode { get; set; }

        internal static PmxSoftBodyAnchor Parse(ref BufferReader br, PmxDocument doc) =>
            new()
            {
                Rigid = doc.ReadRigidBody(ref br),
                Vertex = doc.ReadVertex(ref br),
                IsNearMode = br.ReadBoolean(),
            };

        internal void Write(ref BufferWriter bw, PmxIndexCache cache)
        {
            bw.Write(this.Rigid, cache);
            bw.Write(this.Vertex, cache);
            bw.Write(this.IsNearMode);
        }
    }
}