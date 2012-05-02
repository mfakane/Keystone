using System;

namespace Linearstar.Keystone.IO.Metasequoia
{
	[Flags]
	public enum MqMirrorAxis
	{
		None,
		X,
		Y,
		Z = 4,
		Local = 8,
	}
}
