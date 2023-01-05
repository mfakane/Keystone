using System;

namespace Linearstar.Keystone.IO.MikuMikuDance.Pmd
{
	[Flags]
	public enum PmdRigidGroups : ushort
	{
		None,
		Group1,
		Group2,
		Group3 = 1 << 2,
		Group4 = 1 << 3,
		Group5 = 1 << 4,
		Group6 = 1 << 5,
		Group7 = 1 << 6,
		Group8 = 1 << 7,
		Group9 = 1 << 8,
		Group10 = 1 << 9,
		Group11 = 1 << 10,
		Group12 = 1 << 11,
		Group13 = 1 << 12,
		Group14 = 1 << 13,
		Group15 = 1 << 14,
		Group16 = 1 << 15,
	}
}
