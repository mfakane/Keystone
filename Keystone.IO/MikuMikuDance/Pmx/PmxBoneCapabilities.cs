using System;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	[Flags]
	public enum PmxBoneCapabilities : uint
	{
		None,
		ConnectToBone,
		Rotatable,
		Movable = 4,
		Visible = 8,
		Controllable = 0x10,
		IK = 0x20,
		RotationAffected = 0x100,
		MovementAffected = 0x200,
		FixedAxis = 0x400,
		LocalAxis = 0x800,
		TransformAfterPhysics = 0x1000,
		TransformByExternalParent = 0x2000,
	}
}
