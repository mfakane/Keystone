using System.IO;

namespace Linearstar.Keystone.IO.MikuMikuDance.Pmx
{
	/// <summary>
	/// (PMX 2.1)
	/// </summary>
	public class PmxSoftBodyAnchor
	{
		public PmxRigidBody Rigid
		{
			get;
			set;
		}

		public PmxVertex Vertex
		{
			get;
			set;
		}

		public bool IsNearMode
		{
			get;
			set;
		}

		public static PmxSoftBodyAnchor Parse(BinaryReader br, PmxDocument doc)
		{
			return new PmxSoftBodyAnchor
			{
				Rigid = doc.ReadRigidBody(br),
				Vertex = doc.ReadVertex(br),
				IsNearMode = br.ReadBoolean(),
			};
		}

		public void Write(BinaryWriter bw, PmxDocument doc, PmxIndexCache cache)
		{
			cache.Write(this.Rigid);
			cache.Write(this.Vertex);
			bw.Write(this.IsNearMode);
		}
	}
}
