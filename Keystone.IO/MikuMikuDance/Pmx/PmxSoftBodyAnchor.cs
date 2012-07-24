using System.IO;
namespace Linearstar.Keystone.IO.MikuMikuDance.Pmx
{
	/// <summary>
	/// (PMX 2.1)
	/// </summary>
	public class PmxSoftBodyAnchor
	{
		public int Rigid
		{
			get;
			set;
		}

		public int Vertex
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
				Rigid = doc.ReadIndex(br, PmxIndexKind.Rigid),
				Vertex = doc.ReadIndex(br, PmxIndexKind.Vertex),
				IsNearMode = br.ReadBoolean(),
			};
		}

		public void Write(BinaryWriter bw, PmxDocument doc)
		{
			doc.WriteIndex(bw, PmxIndexKind.Rigid, this.Rigid);
			doc.WriteIndex(bw, PmxIndexKind.Vertex, this.Vertex);
			bw.Write(this.IsNearMode);
		}
	}
}
