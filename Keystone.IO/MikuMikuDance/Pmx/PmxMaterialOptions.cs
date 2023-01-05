using System;

namespace Linearstar.Keystone.IO.MikuMikuDance.Pmx
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

		/// <summary>
		/// (PMX 2.1)
		/// </summary>
		VertexColor = 32,
		/// <summary>
		/// (PMX 2.1)
		/// </summary>
		DrawPoint = 64,
		/// <summary>
		/// (PMX 2.1)
		/// </summary>
		DrawLine = 128,
	}
}
