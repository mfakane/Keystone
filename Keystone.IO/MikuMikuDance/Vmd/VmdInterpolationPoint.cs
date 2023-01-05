﻿using System;
using System.IO;

namespace Linearstar.Keystone.IO.MikuMikuDance.Vmd
{
	public struct VmdInterpolationPoint
	{
		public static readonly VmdInterpolationPoint DefaultA = new(20, 20);
		public static readonly VmdInterpolationPoint DefaultB = new(107, 107);

		public byte X;
		public byte Y;

		public VmdInterpolationPoint(byte x, byte y)
		{
			if (x < 0 || x > 128)
				throw new ArgumentOutOfRangeException("x must be between 0 and 127.");

			if (y < 0 || y > 128)
				throw new ArgumentOutOfRangeException("y must be between 0 and 127.");

			this.X = x;
			this.Y = y;
		}
		
		public static VmdInterpolationPoint Parse(BinaryReader br) =>
			new(br.ReadByte(), br.ReadByte());

		public override string ToString() => $"{{X:{this.X} Y:{this.Y}}}";
	}
}
