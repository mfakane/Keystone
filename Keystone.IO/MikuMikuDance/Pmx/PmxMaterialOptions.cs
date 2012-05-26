using System;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	[Flags]
	public enum PmxMaterialOptions : byte
	{
		None,
		DoubleSided,
		GroundShadow,
		DrawSelfShadowMapping = 4,
		DrawSelfShadow = 8,
		DrawEdge = 16,
	}
}
