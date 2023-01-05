using System.Collections.Generic;

namespace Linearstar.Keystone.IO.MikuMikuDance.Pmd
{
	public class PmdDisplayList
	{
		public string Name { get; set; } = "表示枠";
		
		public string EnglishName { get; set; } = "Display Frame";

		public IList<PmdBone> Bones { get; set; } = new List<PmdBone>();

		internal static PmdDisplayList Parse(ref BufferReader br) =>
			new()
			{
				Name = br.ReadString(50),
			};

		internal void Write(ref BufferWriter bw) => 
			bw.Write(this.Name, 50);
	}
}
