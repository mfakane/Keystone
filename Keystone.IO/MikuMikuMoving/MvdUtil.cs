using System.IO;
using Linearstar.Keystone.IO;

namespace Linearstar.Keystone.IO.MikuMikuMoving
{
	static class MvdUtil
	{
		public static BinaryReader CreateSizedBufferReader(this BinaryReader br)
		{
			return new BinaryReader(new MemoryStream(br.ReadSizedBuffer()));
		}

		public static byte[] ReadSizedBuffer(this BinaryReader br)
		{
			return br.ReadBytes(br.ReadInt32());
		}

		public static void WriteSizedBuffer(this BinaryWriter bw, byte[] buffer)
		{
			bw.Write(buffer.Length);
			bw.Write(buffer);
		}
	}
}
