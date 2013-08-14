using System.Collections.Generic;
using System.IO;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public class PmdDisplayList
	{
		public string Name
		{
			get;
			set;
		}

		public string EnglishName
		{
			get;
			set;
		}

		public IList<PmdBone> Bones
		{
			get;
			set;
		}

		public PmdDisplayList()
		{
			this.Bones = new List<PmdBone>();
		}
		public static PmdDisplayList Parse(BinaryReader br)
		{
			return new PmdDisplayList
			{
				Name = PmdDocument.ReadPmdString(br, 50),
			};
		}

		public void Write(BinaryWriter bw)
		{
			PmdDocument.WritePmdString(bw, this.Name, 50);
		}
	}
}
