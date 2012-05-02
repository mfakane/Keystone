using System.IO;

namespace Linearstar.Keystone.IO.MikuMikuMoving
{
	public struct MvdEffectParameter
	{
		public int Id;
		public MvdEffectParameterType Type;

		public MvdEffectParameter(int id, MvdEffectParameterType type)
		{
			this.Id = id;
			this.Type = type;
		}

		public static MvdEffectParameter Parse(BinaryReader br)
		{
			return new MvdEffectParameter(br.ReadInt32(), (MvdEffectParameterType)br.ReadInt32());
		}

		public void Write(BinaryWriter bw)
		{
			bw.Write(this.Id);
			bw.Write((int)this.Type);
		}
	}
}
