using System;

namespace Linearstar.Keystone.IO.MikuMikuDance.Pmx
{
	/// <summary>
	/// (PMX 2.1)
	/// </summary>
	[Flags]
	public enum PmxSoftBodyOptions : byte
	{
		CreateBendingLinks,
		CreateClusters,
		RandomizeConstraints = 4,
	}
}
