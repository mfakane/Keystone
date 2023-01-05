namespace Linearstar.Keystone.IO.MikuMikuMoving.Mvd
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

		internal static MvdEffectParameter Parse(ref BufferReader br) =>
			new(br.ReadInt32(), (MvdEffectParameterType)br.ReadInt32());

		internal void Write(ref BufferWriter bw)
		{
			bw.Write(this.Id);
			bw.Write((int)this.Type);
		}
	}
}
