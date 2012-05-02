namespace Linearstar.Keystone.IO.MikuMikuMoving
{
	public enum MvdTag : byte
	{
		NameList,
		Bone = 16,
		Morph = 32,
		ModelProperty = 64,
		AccessoryProperty = 80,
		EffectProperty = 88,
		Camera = 96,
		Light = 112,
		Project = 128,
		Eof = 255,
	}
}
