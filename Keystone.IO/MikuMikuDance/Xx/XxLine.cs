namespace Linearstar.Keystone.IO.MikuMikuDance.Xx
{
	public class XxLine
	{
		public ushort From { get; set; }

		public ushort To { get; set; }

		internal static XxLine Parse(ref BufferReader br) =>
			new()
			{
				From = br.ReadUInt16(),
				To = br.ReadUInt16(),
			};

		internal void Write(ref BufferWriter bw)
		{
			bw.Write(this.From);
			bw.Write(this.To);
		}
	}
}
