namespace Linearstar.Keystone.IO.Metasequoia
{
	public struct MqVertexColor
	{
		public byte R
		{
			get;
			set;
		}

		public byte G
		{
			get;
			set;
		}

		public byte B
		{
			get;
			set;
		}

		public byte A
		{
			get;
			set;
		}

		public MqVertexColor(byte r, byte g, byte b, byte a)
			: this()
		{
			this.R = r;
			this.G = g;
			this.B = b;
			this.A = a;
		}

		public override string ToString()
		{
			return "0x" + this.A.ToString("X") + this.B.ToString("X") + this.G.ToString("X") + this.R.ToString("X");
		}
	}
}
