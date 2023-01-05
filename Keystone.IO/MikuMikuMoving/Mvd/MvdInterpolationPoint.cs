using System;

namespace Linearstar.Keystone.IO.MikuMikuMoving.Mvd
{
	public struct MvdInterpolationPoint
	{
		public static readonly MvdInterpolationPoint DefaultA = new(20, 20);
		public static readonly MvdInterpolationPoint DefaultB = new(107, 107);

		public byte X;
		public byte Y;

		public MvdInterpolationPoint(byte x, byte y)
		{
			if (x < 0 || x > 128)
				throw new ArgumentOutOfRangeException("x must be between 0 and 127.");

			if (y < 0 || y > 128)
				throw new ArgumentOutOfRangeException("y must be between 0 and 127.");

			this.X = x;
			this.Y = y;
		}

		internal static MvdInterpolationPoint Parse(ref BufferReader br) => new(br.ReadByte(), br.ReadByte());

		internal void Write(ref BufferWriter bw)
		{
			bw.Write(this.X);
			bw.Write(this.Y);
		}

		public override string ToString() => "{X:" + this.X + " Y:" + this.Y + "}";
	}
}
