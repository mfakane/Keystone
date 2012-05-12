using System.IO;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public class XxLine
	{
		public ushort From
		{
			get;
			set;
		}

		public ushort To
		{
			get;
			set;
		}

		public static XxLine Parse(BinaryReader br)
		{
			return new XxLine
			{
				From = br.ReadUInt16(),
				To = br.ReadUInt16(),
			};
		}

		public void Write(BinaryWriter bw)
		{
			bw.Write(this.From);
			bw.Write(this.To);
		}
	}
}
