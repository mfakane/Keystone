using System.Numerics;

namespace Linearstar.Keystone.IO.MikuMikuDance.Xx
{
	public class XxVertex
	{
		public Vector3 Position { get; set; }

		public Vector3 Normal { get; set; }

		public Vector2 UV { get; set; }

		internal static XxVertex Parse(ref BufferReader br) =>
			new()
			{
				Position = br.ReadVector3(),
				Normal = br.ReadVector3(),
				UV = br.ReadVector2(),
			};

		internal void Write(ref BufferWriter bw)
		{
			bw.Write(this.Position);
			bw.Write(this.Normal);
			bw.Write(this.UV);
		}
	}
}
