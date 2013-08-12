using System.IO;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public class PmxTexture
	{
		public string FileName
		{
			get;
			set;
		}

		public static PmxTexture Parse(BinaryReader br, PmxDocument doc)
		{
			return new PmxTexture
			{
				FileName = doc.ReadString(br),
			};
		}

		public void Write(BinaryWriter bw, PmxDocument doc)
		{
			doc.WriteString(bw, this.FileName);
		}
	}
}
